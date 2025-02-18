using Cloud.Models;
using Cloud.Mvc;
using Domain.Entity.Product;
using Domain.IService.Product;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Input;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Product
{
    /// <summary>
    /// 商城产品信息
    /// </summary>
    public class MallProductApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallProductService _mallProductService;

        public MallProductApp(IMallProductService mallProductService)
        {
            _mallProductService = mallProductService;
        }
        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagingData<IEnumerable<MallProductDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallProductService.GetPageListAsync(input);
            return result;
        }

        /// <summary>
        /// 通过id获取表单信息（后台管理用）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MallProductDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductService.GetSingleByIdAsync(input);
            return result;
        }
        /// <summary>
        /// 新增或编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] MallProductDto input)
        {
            await _mallProductService.AddOrUpdateAsync(input);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductService.LogicDeleteAsync(input);
        }


        /// <summary>
        /// 通过类型id获取列表信息
        /// </summary>
        /// <param name="typeId">商品类型id</param>
        /// <returns></returns>
        [HttpGet]
        public List<MallProduct> GetAllListByTypeId(long typeId)
        {
            var result = _mallProductService.GetAllListByTypeId(typeId);
            return result;
        }
        [HttpGet]
        public async Task<List<MallProduct>> GetAllListAsync()
        {
            var result =await  _mallProductService.GetAllListAsync();
            return result;
        }
        /// <summary>
        /// 通过产品id获取产品信息  (仅H5页面使用)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MallProductDto> FindSingleByIdForH5Async([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductService.FindSingleByIdForH5Async(input);
            return result;
        }
        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        [HttpPost]
        public async Task TakeOnAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductService.TakeOnAsync(input.Id);
        }
        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        [HttpPost]
        public async Task TakeOffAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductService.TakeOffAsync(input.Id);
        }
        /// <summary>
        /// 批量启用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task BatchTakeOnAsync([FromBody] ListIdQueryCommonInput input)
        {
            await _mallProductService.BatchTakeOnAsync(input.ListId);
        }

        /// <summary>
        /// 批量禁用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task BatchTakeOffAsync([FromBody] ListIdQueryCommonInput input)
        {
            await _mallProductService.BatchTakeOffAsync(input.ListId);
        }

        /// <summary>
        /// 通过类型编号获取所有上架商品并分类分组（仅H5页面使用）
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<dynamic> GetProductsGroupbyCategoryByTypeCode(string typeCode)
        {
            return await _mallProductService.GetProductsGroupbyCategoryByTypeCode(typeCode);
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        [HttpPost]
        public void StockInv([FromBody] AdminMallSkuStockChangeInput input)
        {
            _mallProductService.StockInv(input);
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="MyException"></exception>
        [HttpPost]
        public void StockRel([FromBody] AdminMallSkuStockChangeInput input)
        {
            _mallProductService.StockRel(input);
        }
    }
}