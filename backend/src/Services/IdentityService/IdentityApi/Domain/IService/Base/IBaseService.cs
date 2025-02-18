using Cloud.Domain.Entities;
using Cloud.Models;
using Identity.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.IService.Base
{
    public interface IBaseService<T, Tdto> : ICloudService where T : BaseEntity<long>
    {
        Task<PagingData<IEnumerable<Tdto>>> GetPageListAsync(PageQueryCommonInput input);

        Task<PagingData<IEnumerable<Tdto>>> GetPageListWithDetailsAsync(int pageNumber = 1, int pageSize = 10, Expression<Func<T, bool>> express = null, string orderBy = "Id DESC", params Expression<Func<T, object>>[] includeProperties);

        Task<int> GetCountAsync(PageQueryCommonInput input);

        Task<Tdto> GetSingleByIdAsync(IdQueryCommonInput input);

        Task AddOrUpdateAsync(Tdto dto);

        Task LogicDeleteAsync(IdQueryCommonInput input);

        IEnumerable<Tdto> GetAllList(AllQueryCommonInput input);
    }
}
