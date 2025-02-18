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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;
namespace Domain.Service.Dictionary
{
    /// <summary>
    /// 
    /// </summary>
    public class ImgRuleService : BaseListCacheService<ImgRule, ImgRuleDto>, IImgRuleService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IListCacheRepository<ImgRule> _listCacheImgRuleRepository;
        private readonly IListCacheRepository<ImgRuleDetail> _listCacheImgRuleDetailRepository;

        public ImgRuleService(IListCacheRepository<ImgRule> listCacheImgRuleRepository, ICloudUnitOfWork unitWork, IListCacheRepository<ImgRuleDetail> listCacheImgRuleDetailRepository) : base(listCacheImgRuleRepository)
        {
            _listCacheImgRuleRepository = listCacheImgRuleRepository;
            _unitWork = unitWork;
            _listCacheImgRuleDetailRepository = listCacheImgRuleDetailRepository;
        }

        public async new Task<PagingData<IEnumerable<ImgRuleDto>>> GetPageListAsync(PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<ImgRule>();
            var list = _listCacheImgRuleRepository.Paginate(input.PageNumber, input.PageSize, express, "SortNo ASC").MapToIEnumerable<ImgRule, ImgRuleDto>();
            var totalCount = await _listCacheImgRuleRepository.CountAsync(express);
            return new PagingData<IEnumerable<ImgRuleDto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }

        public async Task<ImgRuleInfoDto> FindSingleDetailsByIdAsync(IdQueryCommonInput input)
        {
            var output = (await _listCacheImgRuleRepository.GetSingleAsync(x => x.Id == input.Id)).MapTo<ImgRule, ImgRuleInfoDto>();
            var imgRuleDetailsDto = (await _listCacheImgRuleDetailRepository.QueryAsync(x => x.ImgRuleId == input.Id)).MapToList<ImgRuleDetail, ImgRuleDetailDto>();
            output.ImgRuleDetailDtos= imgRuleDetailsDto;
            return output;
        }

        public async Task AddOrUpdateAsync(ImgRuleInfoDto input)
        {
            var imgRuleEntity = input.MapTo<ImgRule>();
            var imgRuleDetailsEntity = input.ImgRuleDetailDtos.MapToList<ImgRuleDetailDto, ImgRuleDetail>();
            if (string.IsNullOrWhiteSpace(imgRuleEntity.ImgRuleName))
            {
                throw new MyException("名称不能为空！");
            }
            if (string.IsNullOrWhiteSpace(imgRuleEntity.ImgRuleCode))
            {
                throw new MyException("编码不能为空！");
            }
            if (imgRuleEntity.Id == 0)
            {
                if (await _unitWork.ExistsAsync<ImgRule>(x => x.ImgRuleCode == imgRuleEntity.ImgRuleCode))
                {
                    {
                        throw new MyException("编号已存在！");
                    }
                }
                _unitWork.Add(imgRuleEntity);
                imgRuleDetailsEntity.ForEach(x =>
                {
                    x.Id = 0;
                    x.ImgRuleId = imgRuleEntity.Id;
                });
                _unitWork.AddRange(imgRuleDetailsEntity);
            }
            else
            {
                if (await _unitWork.ExistsAsync<ImgRule>(x => x.ImgRuleCode == imgRuleEntity.ImgRuleCode && x.Id != imgRuleEntity.Id))
                {
                    {
                        throw new MyException("编号已存在！");
                    }
                }
                 _unitWork.Update(imgRuleEntity);
                _unitWork.ExecuteDelete<ImgRuleDetail>(x => x.ImgRuleId == imgRuleEntity.Id);
                imgRuleDetailsEntity.ForEach(x =>
                {
                    x.Id = 0;
                    x.ImgRuleId = imgRuleEntity.Id;
                });
                _unitWork.AddRange(imgRuleDetailsEntity);
            }
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<ImgRule>();
            await _unitWork.RemoveListCacheAsync<ImgRuleDetail>();
        }

        public override async Task LogicDeleteAsync(IdQueryCommonInput input)
        {
            var imgRuleEntity = await _unitWork.GetSingleAsync<ImgRule>(x => x.Id == input.Id);
            imgRuleEntity.DeletedStatus = CommonConst.DeletedStatus_Deleted;
             _unitWork.Update(imgRuleEntity);

            var imgRuleDetailEntity = await _unitWork.GetSingleAsync<ImgRuleDetail>(x => x.Id == input.Id);
            imgRuleDetailEntity.DeletedStatus = CommonConst.DeletedStatus_Deleted;
            _unitWork.Update(imgRuleDetailEntity);

            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<ImgRule>();
            await _unitWork.RemoveListCacheAsync<ImgRuleDetail>();
        }

        public async Task<IEnumerable<ImgRuleInfoDto>> GetAllDetailsList()
        {
            var output = new List<ImgRuleInfoDto>();
            var nowTime = DateTime.Now;
            output= (await _listCacheImgRuleRepository.GetListAsync()).MapToList<ImgRule, ImgRuleInfoDto>();
            if (output == default || output.Count == 0)
            {
                return null;
            }
            output.ForEach(async x =>
            {
                var dtoImgRuleDetails = (await _listCacheImgRuleDetailRepository.QueryAsync(d => d.ImgRuleId == x.Id && nowTime >= d.StartTime && nowTime <= d.EndTime)).OrderBy(d => d.SortNo).MapToList<ImgRuleDetail, ImgRuleDetailDto>();
                x.ImgRuleDetailDtos = dtoImgRuleDetails;
            });
            return output;
        }

        public async Task<List<ImgRuleInfoDto>> GetListByCodesAsync([FromBody] CodesQueryCommonInput input)
        {
            if (input == default)
            {
                throw new MyException("入参input不能为空");
            }
            var output = new List<ImgRuleInfoDto>();
            var codeList = input.Codes.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var nowTime = DateTime.Now;
            output =(await _listCacheImgRuleRepository.QueryAsync(x => codeList.Contains(x.ImgRuleCode))).OrderBy(x => x.SortNo).MapToList<ImgRule, ImgRuleInfoDto>();
            if (output == default || output.Count == 0)
            {
                return null;
            }
            output.ForEach(async x =>
            {
                var dtoImgRuleDetails = (await _listCacheImgRuleDetailRepository.QueryAsync(d => d.ImgRuleId == x.Id && nowTime >= d.StartTime && nowTime <= d.EndTime)).OrderBy(d => d.SortNo).MapToList<ImgRuleDetail, ImgRuleDetailDto>();
                x.ImgRuleDetailDtos=dtoImgRuleDetails;
            });
            return output;
        }

    }
}
