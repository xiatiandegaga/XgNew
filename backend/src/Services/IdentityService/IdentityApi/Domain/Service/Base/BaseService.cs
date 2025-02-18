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
    public class BaseService<T, Tdto> : IBaseService<T, Tdto> where T : BaseEntity<long>
    {
        private readonly IRepository<T> _repository;


        public BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<PagingData<IEnumerable<Tdto>>> GetPageListAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            var list = _repository.Paginate(input.PageNumber, input.PageSize, express,null,x=>x.Id).ProjectToType<Tdto>();
            var totalCount = await _repository.CountAsync(express);
            return new PagingData<IEnumerable<Tdto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }

        public async Task<PagingData<IEnumerable<Tdto>>> GetPageListWithDetailsAsync(int pageNumber = 1, int pageSize = 10, Expression<Func<T, bool>> express = null, string orderBy = "Id DESC", params Expression<Func<T, object>>[] includeProperties)
        {
            var list = _repository.Paginate(pageNumber, pageSize, express, orderBy,includeProperties).ProjectToType<Tdto>();
            var totalCount = await _repository.CountAsync(express);
            return new PagingData<IEnumerable<Tdto>> { PageIndex = pageNumber, TotalCount = totalCount, List = list };
        }

        public virtual async Task<int> GetCountAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            var totalCount = await _repository.CountAsync(express);
            return totalCount;
        }

        public virtual async Task<Tdto> GetSingleByIdAsync(IdQueryCommonInput input)
        {
            return  await _repository.Query(x => x.Id == input.Id).ProjectToType<Tdto>().FirstOrDefaultAsync();
        }

        public virtual async Task AddOrUpdateAsync(Tdto input)
        {
            var entity = input.MapTo<T>();
            if (entity.Id == default)
            {
                await _repository.AddAsync(entity);
            }
            else
            {
                await _repository.UpdateAsync(entity);
            }
        }

        public virtual async Task LogicDeleteAsync(IdQueryCommonInput input)
        {
            var entity = await _repository.GetSingleAsync(x => x.Id == input.Id);
            entity.DeletedStatus = CommonConst.DeletedStatus_Deleted;
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(IdQueryCommonInput input)
        {
            await _repository.ExecuteDeleteAsync(x => x.Id == input.Id);
        }

        public virtual IEnumerable<Tdto> GetAllList(AllQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<T>();
            return _repository.Query(express).ProjectToType<Tdto>(); ;
        }
    }
}
