using Cloud.Models;
using Cloud.Mvc;
using Cloud.Repositories;
using Domain.Entity.Dictionary;
using Domain.IService.Dictionary;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;


namespace Application.App.Dictionary
{
    /// <summary>
	/// GlobalData应用层
	/// </summary>
    public class GlobalDataApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IGlobalDataService _globalDataService;
        private readonly IListCacheRepository<GlobalData> _listCacheGlobalDataRepository;

        public GlobalDataApp(IGlobalDataService globalDataService, IListCacheRepository<GlobalData> listCacheGlobalDataRepository)
        {
            _globalDataService = globalDataService;
            _listCacheGlobalDataRepository = listCacheGlobalDataRepository;
        }

        /// <summary>
        /// GetPageListAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagingData<IEnumerable<GlobalDataDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _globalDataService.GetPageListAsync(input);
            return result;
        }

        /// <summary>
        /// GetSingleByIdAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GlobalDataDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _globalDataService.GetSingleByIdAsync(input);
            return result;
        }


        /// <summary>
        /// AddOrUpdateAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] GlobalDataDto input)
        {
            await _globalDataService.AddOrUpdateAsync(input);
        }

        /// <summary>
        /// LogicDeleteAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _globalDataService.LogicDeleteAsync(input);
        }


        /// <summary>
        /// GetAllList
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<GlobalDataDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _globalDataService.GetAllList(input).OrderBy(x => x.SortNo);
            return result;
        }
    }
}