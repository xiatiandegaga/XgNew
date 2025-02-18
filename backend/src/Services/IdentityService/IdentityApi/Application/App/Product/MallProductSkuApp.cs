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
    /// 商城产品sku信息
    /// </summary>
    public class MallProductSkuApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallProductSkuService _mallProductSkuService;
        public MallProductSkuApp(IMallProductSkuService mallProductSkuService)
        {
            _mallProductSkuService = mallProductSkuService;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<MallProductSkuDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallProductSkuService.GetPageListAsync(input);
            return result;
        }

        [HttpPost]
        public async Task<MallProductSkuDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductSkuService.GetSingleByIdAsync(input);
            return result;
        }

        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] List<MallProductSkuDto> input)
        {
            await _mallProductSkuService.AddOrUpdateAsync(input);
        }

        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductSkuService.LogicDeleteAsync(input);
        }

        [HttpPost]
        public IEnumerable<MallProductSkuDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _mallProductSkuService.GetAllList(input);
            return result;
        }
    }
}