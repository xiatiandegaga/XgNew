using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloud.Repositories
{
    public interface IRepository<T> where T : class
    {
        #region 查询方法
        IQueryable<T> Query(
            Expression<Func<T, bool>> predicate = null,
            bool noTracking = false,
            params Expression<Func<T, object>>[] includeProperties);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            bool noTracking = false,
            params Expression<Func<T, object>>[] includeProperties);
        #endregion

        #region 分页与统计
        IQueryable<T> Paginate(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            string orderBy = "Id DESC",
            params Expression<Func<T, object>>[] includeProperties);

        Task<int> CountAsync(
            Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] includeProperties);
        #endregion

        #region 增删改操作
        Task<T> AddAsync(T entity);

        Task BatchAddAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T entity, IList<string> excludeColumnNames = null);

        Task ExecuteUpdateAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);

        Task DeleteAsync(T entity);

        Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate);
        #endregion
    }
}
