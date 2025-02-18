using Cloud.Domain.Entities;
using Cloud.EntityFrameworkCore;
using Cloud.Snowflake;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloud.Repositories.EntityFrameworkCore
{
    public class BaseRepository<T>(ICloudUnitOfWork unitOfWork) : IRepository<T> where T : BaseEntity<long>
    {
        private readonly ICloudUnitOfWork _unitOfWork = unitOfWork;

        public virtual IQueryable<T> Query(
           Expression<Func<T, bool>> predicate = null,
           bool noTracking = false,
           params Expression<Func<T, object>>[] includeProperties)
        {
            return _unitOfWork.Query(predicate, noTracking, includeProperties);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await  _unitOfWork.ExistsAsync(predicate);
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate,
            bool noTracking = false,
            params Expression<Func<T, object>>[] includeProperties)
        {
            return await _unitOfWork.GetSingleAsync(predicate,noTracking,includeProperties);
        }

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageIndex">The pageIndex.</param>
        /// <param name="pageSize">The pageSize. limit max 100</param>
        public virtual IQueryable<T> Paginate(int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            string orderBy = "Id DESC",
            params Expression<Func<T, object>>[] includeProperties)
        {
            return _unitOfWork.Paginate(pageNumber,pageSize,predicate,orderBy,includeProperties);
        }


        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return await _unitOfWork.CountAsync(predicate, includeProperties);
        }


        #region 增删改
        public virtual  async Task<T> AddAsync(T entity)
        {
            _unitOfWork.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }


        public virtual async Task BatchAddAsync(IEnumerable<T> entities)
        {
            _unitOfWork.AddRange(entities);
            await _unitOfWork.CommitAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity, IList<string> excludeColumnNames = null)
        {
            _unitOfWork.Update(entity, excludeColumnNames);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public virtual async Task ExecuteUpdateAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            await _unitOfWork.Query(predicate, true).ExecuteUpdateAsync(setPropertyCalls);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _unitOfWork.Delete(entity);
            await _unitOfWork.CommitAsync();
        }


        public virtual async Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate)
        {
            await _unitOfWork.Query(predicate, true).ExecuteDeleteAsync();
        }
        #endregion
    }
}
