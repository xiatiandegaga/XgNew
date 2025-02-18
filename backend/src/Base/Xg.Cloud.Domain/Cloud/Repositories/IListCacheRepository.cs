using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloud.Repositories
{
    public interface IListCacheRepository<T> : IRepository<T> where T : BaseEntity<long>
    {
        Task<List<T>> GetListAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);

        //IQueryable<T> Paginate(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> orderAscSelector = null, Expression<Func<T, object>> orderDescSelector = null, params Expression<Func<T, object>>[] includeProperties);

        //void UpdateCache(params Expression<Func<T, object>>[] includeProperties);

        //Task UpdateCacheAsync(params Expression<Func<T, object>>[] includeProperties);

    }
}
