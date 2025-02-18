using Cloud.Extensions;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Dictionary;
using Domain.IService.Dictionary;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Service.Dictionary
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalDataDetailService : BaseListCacheService<GlobalDataDetail, GlobalDataDetailDto>, IGlobalDataDetailService
    {
        private readonly IListCacheRepository<GlobalDataDetail> _listCacheGlobalDataDetailRepository;
        private readonly ICloudUnitOfWork _unitWork;

        public GlobalDataDetailService(IListCacheRepository<GlobalDataDetail> listCacheGlobalDataDetailRepository, ICloudUnitOfWork unitWork) : base(listCacheGlobalDataDetailRepository)
        {
            _listCacheGlobalDataDetailRepository = listCacheGlobalDataDetailRepository;
            _unitWork = unitWork;
        }

        public async new Task<PagingData<IEnumerable<GlobalDataDetailDto>>> GetPageListAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<GlobalDataDetail>();
            var list = _listCacheGlobalDataDetailRepository.Paginate(input.PageNumber, input.PageSize, express,input.OrderBy,x => x.SortNo).MapToIEnumerable<GlobalDataDetail, GlobalDataDetailDto>();
            var totalCount = await _listCacheGlobalDataDetailRepository.CountAsync(express);
            return new PagingData<IEnumerable<GlobalDataDetailDto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }

        public override async Task AddOrUpdateAsync(GlobalDataDetailDto input)
        {
            var entity = input.MapTo<GlobalDataDetail>();
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new MyException("名称不能为空！");
            }
            if (string.IsNullOrWhiteSpace(entity.ConstKey))
            {
                throw new MyException("关键值不能为空！");
            }
            if (string.IsNullOrWhiteSpace(entity.Code))
            {
                throw new MyException("编码不能为空！");
            }
            if (entity.Id == 0)
            {
                await _listCacheGlobalDataDetailRepository.AddAsync(entity);
            }
            else
            {
                await _listCacheGlobalDataDetailRepository.UpdateAsync(entity);
            }
        }



    }
}
