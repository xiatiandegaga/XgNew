using Cloud.Caching;
using Cloud.Domain.Entities;
using Cloud.EntityFrameworkCore;
using Cloud.Models;
using Cloud.Snowflake;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Repositories.EntityFrameworkCore
{
    public class CloudUnitOfWork(
         ILogger<CloudUnitOfWork> logger,
        ApplicationDbContext context,
        ICache cacheService,
        ISnowflakeIdWorker snowflakeIdWorker
       ) : ICloudUnitOfWork
    {
        private readonly ILogger<CloudUnitOfWork> _logger=logger;
        private readonly ApplicationDbContext _context = context;
        private readonly ICache _cacheService = cacheService;
        private readonly ISnowflakeIdWorker _snowflakeIdWorker = snowflakeIdWorker;
        private readonly List<Func<CancellationToken, Task<int>>> _pendingBatchOperations = [];

        #region 查询方法
        public IQueryable<T> Query<T>(
            Expression<Func<T, bool>> predicate = null,
            bool noTracking = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return BuildQuery(predicate, noTracking, includeProperties);
        }


        public async Task<T> GetSingleAsync<T>(
            Expression<Func<T, bool>> predicate,
            bool noTracking = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return await BuildQuery(predicate, noTracking, includeProperties)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate= default) where T : class
        {
            return await _context.Set<T>().AnyAsync(predicate??(x=>true));
        }
        #endregion

        #region 分页与统计
        public IQueryable<T> Paginate<T>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            string orderBy = "Id DESC",
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize > 1000) pageSize = 1000;
            return BuildQuery(predicate, true, includeProperties)
                .OrderBy(orderBy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<int> CountAsync<T>(
            Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return await BuildQuery(predicate, true, includeProperties).CountAsync();
        }
        #endregion

        #region 增删改

        public void Add<T>(T entity) where T : BaseEntity<long>
        {
            SetEntityId(entity);
           _context.Set<T>().Add(entity);
        }

        public  void AddRange<T>(IEnumerable<T> entities) where T : BaseEntity<long>
        {
            foreach (var entity in entities)
            {
                SetEntityId(entity);
            }
            _context.Set<T>().AddRange(entities);
        }

        public void Update<T>(T entity, IEnumerable<string> excludedProperties = null) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            entry.State = EntityState.Modified;

            if (excludedProperties != null)
            {
                foreach (var prop in excludedProperties)
                {
                    entry.Property(prop).IsModified = false;
                }
            }
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            if (entities != null && entities.Count() > 0)
            {
                _context.Set<T>().UpdateRange(entities);
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void ExecuteDelete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            _pendingBatchOperations.Add(async ct =>
                await _context.Set<T>().Where(predicate).ExecuteDeleteAsync(ct)
            );
        }

        public void ExecuteUpdate<T>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls) where T : class
        {
            _pendingBatchOperations.Add(async ct =>
                await _context.Set<T>()
                    .Where(predicate)
                    .ExecuteUpdateAsync(setPropertyCalls, ct)
            );
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            //没有ExecuteUpdate、ExecuteDelete时默认提交事务即可
            if (_pendingBatchOperations.Count == 0)
            {
                var output=await _context.SaveChangesAsync();
                return output;
            }
            // 自动开启事务
            await using var transaction = await BeginTransactionAsync(cancellationToken);
            try
            {
                int totalAffectedRows = 0;

                // 1. 执行所有延迟的批量操作（ExecuteDelete/ExecuteUpdate）
                foreach (var operation in _pendingBatchOperations)
                {
                    totalAffectedRows += await operation(cancellationToken);
                }
                _pendingBatchOperations.Clear();

                // 2. 执行普通操作（Add/Update/Delete）
                totalAffectedRows += await _context.SaveChangesAsync(cancellationToken);

                // 提交事务
                await transaction.CommitAsync(cancellationToken);
                return totalAffectedRows;
            }
            catch(Exception ex)
            {
                //_logger.LogError(ex.Message);
                //await transaction.RollbackAsync(CancellationToken.None);
                //return 0;
                await transaction.RollbackAsync(CancellationToken.None);
                throw new MyException(ex.Message, ex.InnerException);
            }
            finally
            {
                _pendingBatchOperations.Clear();
            }
        }
        #endregion

        #region 事务与缓存

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            await _context.Database.BeginTransactionAsync(cancellationToken);


        public async Task RemoveSingleCacheAsync<T>(T entity) where T : BaseEntity<long>
        {
            var cacheConfig = new CacheConfig<T>(CachingExpireType.Invariable);
            await _cacheService.RemoveAsync(cacheConfig.GetCacheKeyOfEntity(entity.Id));
        }

        public async Task RemoveCacheAsync<T>(IEnumerable<T> entities) where T : BaseEntity<long>
        {
            var cacheConfig = new CacheConfig<T>(CachingExpireType.Invariable);
            var listIds= entities.Select(x => cacheConfig.GetCacheKeyOfEntity(x.Id));
            await _cacheService.RemoveAllAsync(listIds);
        }


        public async Task UpdateSingleCacheAsync<T>(T entity) where T : BaseEntity<long>
        {
            var cacheConfig = new CacheConfig<T>(CachingExpireType.Invariable);
            await _cacheService.SetAsync(cacheConfig.GetCacheKeyOfEntity(entity.Id), entity, cacheConfig.CachingExpirationType);
        }

        public async Task UpdateCacheAsync<T>(IEnumerable<T> entities) where T : BaseEntity<long>
        {
            var cacheConfig = new CacheConfig<T>(CachingExpireType.Invariable);
            var dicQuery = entities.ToDictionary(x => cacheConfig.GetCacheKeyOfEntity(x.Id), x => x as object);
            await _cacheService.SetAllAsync(dicQuery, cacheConfig.CachingExpirationType);
        }

        public async Task RemoveListCacheAsync<T>() where T : BaseEntity<long>
        {
            var cacheConfig = new CacheConfig<T>(CachingExpireType.Invariable);
            await _cacheService.RemoveAsync(cacheConfig.GetListCacheKey());
        }
        #endregion

        private IQueryable<T> BuildQuery<T>(
            Expression<Func<T, bool>> predicate,
            bool noTracking,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = _context.Set<T>();
            if (includeProperties != default)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return (noTracking ? query.AsNoTracking() : query).Where(predicate ?? (x => true));
        }

        private void SetEntityId<T>(T entity) where T : BaseEntity<long>
        {
            if (entity.Id == default)
                entity.Id = _snowflakeIdWorker.NextId();
        }


        #region SQL操作
        public async Task<List<T>> SqlQueryAsync<T>(FormattableString sql) =>
            await _context.Database.SqlQuery<T>(sql).ToListAsync();

        public async Task ExecuteSqlAsync(FormattableString sql) =>
            await _context.Database.ExecuteSqlAsync(sql);
        #endregion
    }
}
