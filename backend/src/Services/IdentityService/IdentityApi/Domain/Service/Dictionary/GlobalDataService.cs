using Cloud.Extensions;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Dictionary;
using Domain.IService.Dictionary;
using Domain.IService.Identity;
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
    public class GlobalDataService : BaseListCacheService<GlobalData, GlobalDataDto>, IGlobalDataService
    {
        private readonly IListCacheRepository<GlobalData> _listCacheGlobalDataRepository;
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IAuthenticationPrincipalService _authenticationService;
        public GlobalDataService(IListCacheRepository<GlobalData> listCacheGlobalDataRepository, ICloudUnitOfWork unitWork, IAuthenticationPrincipalService authenticationService) : base(listCacheGlobalDataRepository)
        {
            _listCacheGlobalDataRepository = listCacheGlobalDataRepository;
            _unitWork = unitWork;
            _authenticationService = authenticationService;
        }

        public async new  Task<PagingData<IEnumerable<GlobalDataDto>>> GetPageListAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<GlobalData>();
            return await  base.GetPageListAsync(input.PageNumber, input.PageSize, express, x => x.SortNo);
        }

        public override async Task AddOrUpdateAsync(GlobalDataDto input)
        {
            var entity = input.MapTo<GlobalData>();
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new MyException("名称不能为空！");
            }
            if (string.IsNullOrWhiteSpace(entity.Code))
            {
                throw new MyException("编号不能为空！");
            }
            if (entity.Id == 0)
            {
                if (await _listCacheGlobalDataRepository.ExistsAsync(x => x.Code == entity.Code))
                {
                    {
                        throw new MyException("编号已存在！");
                    }
                }
                if (await _listCacheGlobalDataRepository.ExistsAsync(x => x.Name == entity.Name))
                {
                    {
                        throw new MyException("名称已存在！");
                    }
                }
                await _listCacheGlobalDataRepository.AddAsync(entity);
            }
            else
            {
                if (await _listCacheGlobalDataRepository.ExistsAsync(x => x.Code == entity.Code && x.Id != entity.Id))
                {
                    {
                        throw new MyException("编号已存在！");
                    }
                }
                if (await _listCacheGlobalDataRepository.ExistsAsync(x => x.Name == entity.Name && x.Id != entity.Id))
                {
                    {
                        throw new MyException("名称已存在！");
                    }
                }
                var userId = await _authenticationService.GetAuthenticatedUserIdAsync();
                await _listCacheGlobalDataRepository.ExecuteUpdateAsync(x=>x.Id==entity.Id,y=>
                y.SetProperty(p=>p.Name,entity.Name)
                  .SetProperty(p => p.Code, entity.Code)
                  .SetProperty(p => p.SortNo, entity.SortNo)
                  .SetProperty(p => p.UpdatedAt, entity.UpdatedAt)
                  .SetProperty(p => p.UpdatedBy, userId)
                    );
            }
        }
    }
}
