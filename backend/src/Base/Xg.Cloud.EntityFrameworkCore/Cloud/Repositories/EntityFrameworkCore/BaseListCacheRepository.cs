using Cloud.Caching;
using Cloud.Domain.Entities;
using Cloud.EntityFrameworkCore;
using Cloud.Snowflake;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Polly;

namespace Cloud.Repositories.EntityFrameworkCore
{
    public class BaseListCacheRepository<T>(ICloudUnitOfWork unitOfWork, ICache cacheService, ICacheConfig<T> cacheConfig, ILogger<BaseListCacheRepository<T>> logger) : BaseRepository<T>(unitOfWork), IListCacheRepository<T> where T : BaseEntity<long>
    {
        private List<T> _listT;
        private readonly ICloudUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICache _cache = cacheService;
        private readonly ICacheConfig<T> _cacheConfig = cacheConfig;
        private readonly ILogger<BaseListCacheRepository<T>> _logger = logger;


        public async Task<List<T>> GetListAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            await CheckCacheAsync(includeProperties);
            return _listT;
        }

        public  override IQueryable<T> Query(
         Expression<Func<T, bool>> predicate = null,
         bool noTracking = false,
         params Expression<Func<T, object>>[] includeProperties)
        {
            CheckCache(includeProperties);
            var query = _listT.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);
            return query;

        }

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="exp">The exp.</param>
        public virtual async Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            await CheckCacheAsync(includeProperties);
            var query = _listT.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }


        /// <summary>
        /// 查找单个
        /// </summary>
        public  async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            await CheckCacheAsync(includeProperties);
            return _listT.AsQueryable().FirstOrDefault(predicate);
        }


        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageIndex">The pageIndex.</param>
        /// <param name="pageSize">The pageSize. limit max 100</param>
        /// <param name="orderBy">排序，格式如："Id"/"Id desc"</param>
        public override IQueryable<T> Paginate(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null, string orderBy = "Id DESC", params Expression<Func<T, object>>[] includeProperties)
        {
            CheckCache(includeProperties);
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize > 1000) pageSize = 1000;
            var query = _listT.AsQueryable();
            if (predicate != default)
                query = query.Where(predicate);
            if (orderBy != default)
                query = query.OrderBy(orderBy);
            return query.Skip(pageSize * (pageNumber - 1)).Take(pageSize).AsNoTracking();
        }

        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        public override async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            await CheckCacheAsync(includeProperties);
            if (predicate != default)
            {
                return _listT.AsQueryable().Where(predicate).Count();

            }
            return _listT.Count();
        }

        public override async Task<T> AddAsync(T entity)
        {
            await base.AddAsync(entity);
            await RemoveCacheAsync();
            return entity;
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">The entities.</param>
        public override async Task BatchAddAsync(IEnumerable<T> entities)
        {
            await base.BatchAddAsync(entities);
            await RemoveCacheAsync();
        }

        public override async Task<T> UpdateAsync(T entity, IList<string> excludeColumnNames = null)
        {
            await base.UpdateAsync(entity, excludeColumnNames);
            await RemoveCacheAsync();
            return entity;
        }


        public override async Task ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            await base.ExecuteUpdateAsync(predicate, setPropertyCalls);
            await RemoveCacheAsync();
        }

        public override async Task DeleteAsync(T entity)
        {
            await base.DeleteAsync(entity);
            await RemoveCacheAsync();
        }


        public override async Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate)
        {
            await base.ExecuteDeleteAsync(predicate);
            await RemoveCacheAsync();
        }


        private void CheckCache(params Expression<Func<T, object>>[] includeProperties)
        {
            if (_listT == null)
                _listT = _cache.Get<List<T>>(_cacheConfig.GetListCacheKey());
            if (_listT == null)
                UpdateCache(includeProperties);
        }

        private async Task CheckCacheAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            if (_listT == null)
                _listT =await _cache.GetAsync<List<T>>(_cacheConfig.GetListCacheKey());
            if (_listT == null)
                await UpdateCacheAsync(includeProperties);
        }

        public void UpdateCache(params Expression<Func<T, object>>[] includeProperties)
        {
            _listT = base.Query(null, true,includeProperties).ToList();
            _cache.Set(_cacheConfig.GetListCacheKey(), _listT, _cacheConfig.CachingExpirationType);
        }

        public async Task UpdateCacheAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            _listT = await base.Query(null, true, includeProperties).ToListAsync();
            await _cache.SetAsync(_cacheConfig.GetListCacheKey(), _listT, _cacheConfig.CachingExpirationType);
        }

        public async Task RemoveCacheAsync()
        {
            var cacheHelper = new CacheConfig<T>(CachingExpireType.Invariable);
            await _cache.RemoveAsync(cacheHelper.GetListCacheKey());
        }

    }
}
