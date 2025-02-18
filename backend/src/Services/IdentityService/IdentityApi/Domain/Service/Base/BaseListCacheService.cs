using Cloud.Mapster;
using Cloud.Domain.Entities;
using Cloud.Extensions;
using Cloud.Models;
using Cloud.Repositories;
using Domain.IService.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;
using System.Linq.Expressions;
using System;
using Identity.Shared.Dto;

namespace Domain.Service.Base
{
    public class BaseListCacheService<T, Tdto> : IBaseListCacheService<T, Tdto> where T : BaseEntity<long>
    {
        private readonly IListCacheRepository<T> _listCacheRepository;


        public BaseListCacheService(IListCacheRepository<T> listCacheRepository)
        {
            _listCacheRepository = listCacheRepository;
        }

        public async Task<PagingData<IEnumerable<Tdto>>> GetPageListAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            var list = _listCacheRepository.Paginate(input.PageNumber, input.PageSize, express, null, x => x.Id).MapToIEnumerable<T, Tdto>();
            var totalCount = await _listCacheRepository.CountAsync(express);
            return new PagingData<IEnumerable<Tdto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }

        public async Task<PagingData<IEnumerable<Tdto>>> GetPageListAsync(int pageNumber = 1, int pageSize = 10, Expression<Func<T, bool>> express = null, Expression<Func<T, object>> orderAscSelector = null, Expression<Func<T, object>> orderDescSelector = null)
        {
            var list = _listCacheRepository.Paginate(pageNumber, pageSize, express).MapToIEnumerable<T, Tdto>();

            var totalCount = await _listCacheRepository.CountAsync(express);
            return new PagingData<IEnumerable<Tdto>> { PageIndex = pageNumber, TotalCount = totalCount, List = list };
        }

        public virtual async Task<int> GetCountAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            var totalCount = await _listCacheRepository.CountAsync(express);
            return totalCount;
        }

        public virtual async Task<Tdto> GetSingleByIdAsync(IdQueryCommonInput input)
        {
            return (await _listCacheRepository.GetSingleAsync(x => x.Id == input.Id)).MapTo<T, Tdto>(); ;
        }

        public virtual async Task AddOrUpdateAsync(Tdto input)
        {
            var entity = input.MapTo<T>();
            if (entity.Id == default)
            {
                await _listCacheRepository.AddAsync(entity);
            }
            else
            {
                await _listCacheRepository.UpdateAsync(entity);
            }
        }

        public virtual async Task LogicDeleteAsync(IdQueryCommonInput input)
        {
            var entity = await _listCacheRepository.GetSingleAsync(x => x.Id == input.Id);
            entity.DeletedStatus = CommonConst.DeletedStatus_Deleted;
            await _listCacheRepository.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(IdQueryCommonInput input)
        {
            await _listCacheRepository.ExecuteDeleteAsync(x => x.Id == input.Id);
        }

        public virtual IEnumerable<Tdto> GetAllList(AllQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            return _listCacheRepository.Query(express).ToList().MapToIEnumerable<T, Tdto>(); ;
        }
    }
}
