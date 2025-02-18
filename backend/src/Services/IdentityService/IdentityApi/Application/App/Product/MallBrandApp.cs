using Cloud.Models;
using Cloud.Mvc;
using Domain.IService.Product;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Product
{
    /// <summary>
    /// 商城产品品牌信息
    /// </summary>
    public class MallBrandApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallBrandService _mallBrandService;
        public MallBrandApp(IMallBrandService mallBrandService)
        {
            _mallBrandService = mallBrandService;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<MallBrandDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallBrandService.GetPageListAsync(input);
            return result;
        }

        [HttpPost]
        public async Task<MallBrandDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallBrandService.GetSingleByIdAsync(input);
            return result;
        }

        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] MallBrandDto input)
        {
            await _mallBrandService.AddOrUpdateAsync(input);
        }

        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallBrandService.LogicDeleteAsync(input);
        }

        [HttpPost]
        public IEnumerable<MallBrandDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _mallBrandService.GetAllList(input);
            return result;
        }
    }
}