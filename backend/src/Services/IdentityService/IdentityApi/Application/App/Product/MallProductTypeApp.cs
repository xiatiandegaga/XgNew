using Cloud.Models;
using Cloud.Mvc;
using Domain.IService.Product;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Product
{
    /// <summary>
    /// 产品类型表
    /// </summary>
    public class MallProductTypeApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallProductTypeService _mallProductTypeService;
        public MallProductTypeApp(IMallProductTypeService mallProductTypeService)
        {
            _mallProductTypeService = mallProductTypeService;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<MallProductTypeDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallProductTypeService.GetPageListAsync(input);
            return result;
        }

        [HttpPost]
        public async Task<MallProductTypeDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductTypeService.GetSingleByIdAsync(input);
            return result;
        }

        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] MallProductTypeDto input)
        {
            await _mallProductTypeService.AddOrUpdateAsync(input);
        }

        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductTypeService.LogicDeleteAsync(input);
        }

        [HttpPost]
        public IEnumerable<MallProductTypeDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _mallProductTypeService.GetAllList(input).OrderBy(x=>x.SortNo);
            return result;
        }
    }
}