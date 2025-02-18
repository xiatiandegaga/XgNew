using Cloud.Models;
using Cloud.Mvc;
using Cloud.Repositories;
using Domain.Entity.Dictionary;
using Domain.IService.Dictionary;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Dictionary
{
    /// <summary>
	/// GlobalData应用层
	/// </summary>
    public class GlobalDataDetailApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IGlobalDataDetailService _globalDataDetailService;
        private readonly IListCacheRepository<GlobalDataDetail> _listCacheGlobalDataDetailRepository;

        public GlobalDataDetailApp(IGlobalDataDetailService globalDataDetailService, IListCacheRepository<GlobalDataDetail> listCacheGlobalDataDetailRepository)
        {
            _globalDataDetailService = globalDataDetailService;
            _listCacheGlobalDataDetailRepository = listCacheGlobalDataDetailRepository;
        }

        /// <summary>
        /// GetPageListAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagingData<IEnumerable<GlobalDataDetailDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _globalDataDetailService.GetPageListAsync(input);
            return result;
        }

        /// <summary>
        /// GetSingleByIdAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GlobalDataDetailDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _globalDataDetailService.GetSingleByIdAsync(input);
            return result;
        }


        /// <summary>
        /// AddOrUpdateAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] GlobalDataDetailDto input)
        {
            await _globalDataDetailService.AddOrUpdateAsync(input);
        }

        /// <summary>
        /// LogicDeleteAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _globalDataDetailService.LogicDeleteAsync(input);
        }

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _globalDataDetailService.DeleteAsync(input);
        }

        /// <summary>
        /// GetAllList
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<GlobalDataDetailDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _globalDataDetailService.GetAllList(input).OrderBy(x => x.SortNo);
            return result;
        }

        /// <summary>
        /// 根据编号获取列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<GlobalDataDetail>> GetListByCodes([FromBody] CodesQueryCommonInput input)
        {
            string[] codeList= input.Codes.Split(',',StringSplitOptions.RemoveEmptyEntries);
            var result =(await _listCacheGlobalDataDetailRepository.QueryAsync(x=>codeList.Contains(x.Code))).OrderBy(x => x.SortNo).ToList();
            return result;
        }
    }
}