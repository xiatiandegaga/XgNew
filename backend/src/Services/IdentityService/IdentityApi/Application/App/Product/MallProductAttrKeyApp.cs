using Cloud.Models;
using Cloud.Mvc;
using Domain.Entity.Product;
using Domain.IService.Product;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Output;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Product
{
    /// <summary>
    /// 产品属性表key
    /// </summary>
    public class MallProductAttrKeyApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallProductAttrKeyService _mallProductAttrKeyService;
        public MallProductAttrKeyApp(IMallProductAttrKeyService mallProductAttrKeyService)
        {
            _mallProductAttrKeyService = mallProductAttrKeyService;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<MallProductAttrKeyDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallProductAttrKeyService.GetPageListAsync(input);
            return result;
        }

        [HttpPost]
        public async Task<MallProductAttrKeyDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductAttrKeyService.GetSingleByIdAsync(input);
            return result;
        }
        /// <summary>
        /// 新增或编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] MallProductAttrKeyInfoOutput input)
        {
            await _mallProductAttrKeyService.AddOrUpdateAsync(input);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductAttrKeyService.LogicDeleteAsync(input);
        }

        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<MallProductAttrKeyDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _mallProductAttrKeyService.GetAllList(input);
            return result;
        }
        /// <summary>
        /// 获取所有列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<MallProductAttrKey>> GetListAsync()
        {
            var result =await _mallProductAttrKeyService.GetListAsync();
            return result;
        }
        /// <summary>
        /// 根据产品属性表key的id查找属性相关信息
        /// </summary>
        /// <param name="input">产品属性表key的id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MallProductAttrKeyInfoOutput> FindSingleDetailsByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductAttrKeyService.FindSingleDetailsByIdAsync(input);
            return result;
        }

        /// <summary>
        /// 根据产品目录的id查找属性相关信息
        /// </summary>
        /// <param name="input">产品类型id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MallProductAttrKeyInfoOutput>> GetAllDetailsListByCategoryIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductAttrKeyService.GetAllDetailsListByCategoryIdAsync(input);
            return result;
        }

    }
}