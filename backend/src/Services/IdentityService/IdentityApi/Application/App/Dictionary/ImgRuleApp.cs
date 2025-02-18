using Cloud.Models;
using Cloud.Mvc;
using Cloud.Repositories;
using Domain.Entity.Dictionary;
using Domain.IService.Dictionary;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Dictionary
{
    /// <summary>
	/// ImgRule应用层
	/// </summary>
    public class ImgRuleApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IImgRuleService _imgRuleService;
        private readonly IListCacheRepository<ImgRule> _listCacheImgRuleRepository;
        private readonly IListCacheRepository<ImgRuleDetail> _listCacheImgRuleDetailRepository;


        public ImgRuleApp(IImgRuleService imgRuleService, IListCacheRepository<ImgRule> listCacheImgRuleRepository, IListCacheRepository<ImgRuleDetail> listCacheImgRuleDetailRepository)
        {
            _imgRuleService = imgRuleService;
            _listCacheImgRuleRepository = listCacheImgRuleRepository;
            _listCacheImgRuleDetailRepository = listCacheImgRuleDetailRepository;
        }

        /// <summary>
        /// GetPageListAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagingData<IEnumerable<ImgRuleDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _imgRuleService.GetPageListAsync(input);
            return result;
        }

        /// <summary>
        /// GetSingleByIdAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImgRuleInfoDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _imgRuleService.FindSingleDetailsByIdAsync(input);
            return result;
        }


        /// <summary>
        /// AddOrUpdateAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] ImgRuleInfoDto input)
        {
            await _imgRuleService.AddOrUpdateAsync(input);
        }

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _imgRuleService.LogicDeleteAsync(input);
        }

        [HttpPost]
        public async Task<IEnumerable<ImgRuleInfoDto>> GetAllList()
        {
            var result = await _imgRuleService.GetAllDetailsList();
            return result;
        }

        /// <summary>
        /// 根据编号获取列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<ImgRuleInfoDto>> GetListByCodesAsync([FromBody] CodesQueryCommonInput input)
        {
            var result =await  _imgRuleService.GetListByCodesAsync(input);
            return result;
        }

    }
}