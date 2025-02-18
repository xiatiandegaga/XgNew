using Cloud.Models;
using Cloud.Mvc;
using Domain.Entity.Product;
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
    /// 产品属性表值value
    /// </summary>
    public class MallProductAttrValueApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallProductAttrValueService _mallProductAttrValueService;
        public MallProductAttrValueApp(IMallProductAttrValueService mallProductAttrValueService)
        {
            _mallProductAttrValueService = mallProductAttrValueService;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<MallProductAttrValueDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallProductAttrValueService.GetPageListAsync(input);
            return result;
        }

        [HttpPost]
        public async Task<MallProductAttrValueDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductAttrValueService.GetSingleByIdAsync(input);
            return result;
        }

        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] MallProductAttrValueDto input)
        {
            await _mallProductAttrValueService.AddOrUpdateAsync(input);
        }

        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductAttrValueService.LogicDeleteAsync(input);
        }

        [HttpPost]
        public IEnumerable<MallProductAttrValueDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _mallProductAttrValueService.GetAllList(input);
            return result;
        }
        [HttpGet]
        public async Task<List<MallProductAttrValue>> GetListAsync()
        {
            var result =await _mallProductAttrValueService.GetListAsync();
            return result;
        }
    }
}