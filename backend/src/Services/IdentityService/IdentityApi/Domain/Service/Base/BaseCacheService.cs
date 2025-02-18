using Cloud.Domain.Entities;
using Cloud.Extensions;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.Service.Base
{
    public class BaseCacheService<T, Tdto> : IBaseCacheService<T, Tdto> where T : BaseEntity<long>
    {
        private readonly ICacheRepository<T> _cacheRepository;


        public BaseCacheService(ICacheRepository<T> cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<PagingData<IEnumerable<Tdto>>> PaginateAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            var list = _cacheRepository.Paginate(input.PageNumber, input.PageSize, express, null, x => x.Id).ProjectToType<Tdto>();
            var totalCount = await _cacheRepository.CountAsync(express);
            return new PagingData<IEnumerable<Tdto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }

        public async Task<PagingData<IEnumerable<Tdto>>> PaginateAsync(int pageNumber = 1, int pageSize = 10, Expression<Func<T, bool>> express = null, string orderBy = "Id DESC")
        {
            var list = _cacheRepository.Paginate(pageNumber, pageSize, express).MapToIEnumerable<T, Tdto>();
            var totalCount = await _cacheRepository.CountAsync(express);
            return new PagingData<IEnumerable<Tdto>> { PageIndex = pageNumber, TotalCount = totalCount, List = list };
        }

        public virtual async Task<int> GetCountAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            var totalCount = await _cacheRepository.CountAsync(express);
            return totalCount;
        }

        public virtual async Task<Tdto> GetSingleByIdAsync(IdQueryCommonInput input)
        {
            return await _cacheRepository.Query(x => x.Id == input.Id).ProjectToType<Tdto>().FirstOrDefaultAsync();
        }

        public virtual async Task AddOrUpdateAsync(Tdto input)
        {
            var entity = input.MapTo<T>();
            if (entity.Id == default)
            {
                await _cacheRepository.AddAsync(entity);
            }
            else
            {
                await _cacheRepository.UpdateAsync(entity);
            }
        }

        public virtual async Task LogicDeleteAsync(IdQueryCommonInput input)
        {
            var entity = await _cacheRepository.GetSingleAsync(x => x.Id == input.Id);
            entity.DeletedStatus = CommonConst.DeletedStatus_Deleted;
            await _cacheRepository.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(IdQueryCommonInput input)
        {
            await _cacheRepository.ExecuteDeleteAsync(x => x.Id == input.Id);
        }

        public virtual IEnumerable<Tdto> GetAllList(AllQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            return _cacheRepository.Query(express).ProjectToType<Tdto>(); ;
        }
    }
}
