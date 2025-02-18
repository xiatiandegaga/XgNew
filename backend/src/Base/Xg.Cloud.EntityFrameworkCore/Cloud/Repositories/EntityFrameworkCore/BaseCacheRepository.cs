using Cloud.Caching;
using Cloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloud.Repositories.EntityFrameworkCore
{
    public class BaseCacheRepository<T>(ICloudUnitOfWork unitOfWork, ICache cacheService, ICacheConfig<T> cacheConfig) : BaseRepository<T>(unitOfWork), ICacheRepository<T> where T : BaseEntity<long>
    {
        private readonly ICloudUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICache _cache = cacheService;
        private readonly ICacheConfig<T> _cacheConfig = cacheConfig;

        /// <summary>
        /// 查找单个
        /// </summary>
        public override async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool noTracking = false, params Expression<Func<T, object>>[] includeProperties)
        {
            T entity = await base.GetSingleAsync(predicate, noTracking, includeProperties);
            if (entity!=default)
            {
               await _unitOfWork.UpdateSingleCacheAsync(entity);
            }
            return entity;
        }

        /// <summary>
        /// 根据过滤条件，获取记录（谨慎使用）
        /// </summary>
        public  virtual async  Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            long[] entityIds = base.Query(predicate,true, includeProperties).Select(e => e.Id).ToArray();
            return await PopulateEntitiesByEntityIdsAsync(entityIds, includeProperties);
        }

        /// <summary>
        /// 查找单个
        /// </summary>
        public virtual async Task<T> GetSingleByIdAsync(long entityId, params Expression<Func<T, object>>[] includeProperties)
        {
            T entity = _cache.Get<T>(_cacheConfig.GetCacheKeyOfEntity(entityId));
            if (entity==default)
            {
                entity = Query(e => e.Id == entityId,true, includeProperties).FirstOrDefault();
                if (entity == default)
                {
                    return default;
                }
                await _unitOfWork.UpdateSingleCacheAsync(entity);
            }
            return entity;
        }


        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageIndex">The pageIndex.</param>
        /// <param name="pageSize">The pageSize.</param>
        /// <param name="orderBy">排序，格式如："Id"/"Id desc"</param>
        /// <param name="includeProperties">一对多的实体.</param>
        public new virtual IEnumerable<T> Paginate(int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            string orderBy = "Id DESC",
            params Expression<Func<T, object>>[] includeProperties)
        {
            long[] entityIds = base.Paginate(pageNumber, pageSize, predicate, orderBy,includeProperties).Select(e => e.Id).ToArray();
            return PopulateEntitiesByEntityIds(entityIds, includeProperties);
        }


        public override async Task<T> AddAsync (T entity)
        {
            entity =await  base.AddAsync(entity);
            await _unitOfWork.UpdateSingleCacheAsync(entity);
            return entity;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">The entities.</param>
        public override async Task BatchAddAsync(IEnumerable<T> entities)
        {
            await base.BatchAddAsync(entities);
            await _unitOfWork.UpdateCacheAsync(entities);
        }

        public override async Task<T> UpdateAsync(T entity, IList<string> excludeColumnNames = null)
        {
            await base.UpdateAsync(entity, excludeColumnNames);
            await _unitOfWork.UpdateSingleCacheAsync(entity);
            return entity;
        }

        public override async Task ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            await base.ExecuteUpdateAsync(predicate, setPropertyCalls);
            var entities =Query(predicate).ToArray();
            await _unitOfWork.UpdateCacheAsync(entities);
        }

        public override async Task DeleteAsync(T entity)
        {
            await base.DeleteAsync(entity);
            await _unitOfWork.RemoveSingleCacheAsync(entity);
        }



        public override async Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate)
        {
            await base.ExecuteDeleteAsync(predicate);
            var entities = Query(predicate).ToArray();
            await _unitOfWork.RemoveCacheAsync(entities);
        }


        protected virtual IEnumerable<T> PopulateEntitiesByEntityIds(long[] entityIds, params Expression<Func<T, object>>[] includeProperties)
        {
            var entityArr = new T[entityIds.Count()];
            Dictionary<long, int> entityIdsNoCache = new Dictionary<long, int>();
            //先从缓存里取，没有的再去数据库取
            for (int i = 0; i < entityIds.Count(); i++)
            {
                T entity = _cache.Get<T>(_cacheConfig.GetCacheKeyOfEntity(entityIds[i]));
                if (entity != null)
                {
                    entityArr[i] = entity;
                }
                else
                {
                    entityArr[i] = null;
                    entityIdsNoCache[entityIds[i]] = i;
                }
            }
            if (entityIdsNoCache.Count() > 0)
            {
                IEnumerable<T> entitiesFromDb = Query(e => entityIdsNoCache.Keys.Contains(e.Id),true, includeProperties);
                foreach (var entityFromDb in entitiesFromDb)
                {
                    entityArr[entityIdsNoCache[entityFromDb.Id]] = entityFromDb;
                    _cache.SetAsync(_cacheConfig.GetCacheKeyOfEntity(entityFromDb.Id), entityFromDb, _cacheConfig.CachingExpirationType);
                }
            }
            return entityArr.AsEnumerable();
        }

        protected virtual async Task<IEnumerable<T>> PopulateEntitiesByEntityIdsAsync(long[] entityIds, params Expression<Func<T, object>>[] includeProperties)
        {
            var entityArr = new T[entityIds.Count()];
            Dictionary<long, int> entityIdsNoCache = new Dictionary<long, int>();
            //先从缓存里取，没有的再去数据库取
            for (int i = 0; i < entityIds.Count(); i++)
            {
                T entity = await _cache.GetAsync<T>(_cacheConfig.GetCacheKeyOfEntity(entityIds[i]));
                if (entity != null)
                {
                    entityArr[i] = entity;
                }
                else
                {
                    entityArr[i] = null;
                    entityIdsNoCache[entityIds[i]] = i;
                }
            }
            if (entityIdsNoCache.Count() > 0)
            {
                IEnumerable<T> entitiesFromDb = Query(e => entityIdsNoCache.Keys.Contains(e.Id),true, includeProperties);
                foreach (var entityFromDb in entitiesFromDb)
                {
                    entityArr[entityIdsNoCache[entityFromDb.Id]] = entityFromDb;
                    await _cache.SetAsync(_cacheConfig.GetCacheKeyOfEntity(entityFromDb.Id), entityFromDb, _cacheConfig.CachingExpirationType);
                }
            }
            return entityArr.AsEnumerable();
        }

    }
}
