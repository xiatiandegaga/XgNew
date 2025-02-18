using Cloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloud.Repositories
{
    public interface ICacheRepository<T> : IRepository<T> where T : BaseEntity<long>
    {
        #region 扩展的缓存查询方法

        /// <summary>
        /// 获取实体列表（优先从缓存读取，根据ids去找缓存）
        /// </summary>
        Task<IEnumerable<T>> GetListAsync(
            Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// 根据ID获取单个实体（优先从缓存读取）
        /// </summary>
        Task<T> GetSingleByIdAsync(
            long entityId,
            params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// 分页查询（优先从缓存读取）
        /// </summary>
        //new IEnumerable<T> Paginate(
        //    int pageNumber,
        //    int pageSize,
        //    Expression<Func<T, bool>> predicate = null,
        //    string orderBy = "Id DESC",
        //    params Expression<Func<T, object>>[] includeProperties);

        #endregion

        #region 覆盖的增删改方法（带缓存同步）

        //new Task<T> AddAsync(T entity);

        //new Task BatchAddAsync(IEnumerable<T> entities);

        //new Task<T> UpdateAsync(T entity, IList<string> excludeColumnNames = null);

        //new Task ExecuteUpdateAsync(
        //    Expression<Func<T, bool>> predicate,
        //    Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);

        //new Task DeleteAsync(T entity);

        //new Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate);

        #endregion

    }
}
