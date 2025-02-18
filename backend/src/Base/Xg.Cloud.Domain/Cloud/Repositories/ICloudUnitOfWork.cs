using Cloud.Domain.Entities;
using Cloud.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Repositories
{
    public interface ICloudUnitOfWork
    {
        #region 查询方法
     IQueryable<T> Query<T>(
            Expression<Func<T, bool>> predicate = null,
            bool noTracking = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class;

        Task<T> GetSingleAsync<T>(
            Expression<Func<T, bool>> predicate,
            bool noTracking = false,
            params Expression<Func<T, object>>[] includeProperties) where T : class;

        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate= default) where T : class;
        #endregion

        #region 分页与统计
        IQueryable<T> Paginate<T>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            string orderBy = "Id DESC",
            params Expression<Func<T, object>>[] includeProperties) where T : class;

        Task<int> CountAsync<T>(
            Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] includeProperties) where T : class;
        #endregion

        #region 增删改
        void Add<T>(T entity) where T : BaseEntity<long>;
        void AddRange<T>(IEnumerable<T> entities) where T : BaseEntity<long>;
        void Update<T>(T entity, IEnumerable<string> excludedProperties = null) where T : class;
        void UpdateRange<T>(IEnumerable<T> entities) where T : class;
        void Delete<T>(T entity) where T : class;

        void ExecuteDelete<T>(Expression<Func<T, bool>> predicate) where T : class;

        void ExecuteUpdate<T>(
           Expression<Func<T, bool>> predicate,
           Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls) where T : class;


       Task<int> CommitAsync(CancellationToken cancellationToken = default);
        #endregion

        #region 事务与缓存
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task RemoveSingleCacheAsync<T>(T entity) where T : BaseEntity<long>;
        Task RemoveCacheAsync<T>(IEnumerable<T> entities) where T : BaseEntity<long>;
        Task UpdateSingleCacheAsync<T>(T entity) where T : BaseEntity<long>;
        Task UpdateCacheAsync<T>(IEnumerable<T> entities) where T : BaseEntity<long>;
        Task RemoveListCacheAsync<T>() where T : BaseEntity<long>;
        #endregion

        #region SQL操作
        Task<List<T>> SqlQueryAsync<T>(FormattableString sql);
        Task ExecuteSqlAsync(FormattableString sql);
        #endregion

    }
}
