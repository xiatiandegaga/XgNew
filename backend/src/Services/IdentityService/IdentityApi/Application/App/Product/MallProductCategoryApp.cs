using Cloud.Models;
using Cloud.Mvc;
using Cloud.Repositories;
using Domain.Entity.Product;
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
    /// 产品目录表
    /// </summary>
    public class MallProductCategoryApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMallProductCategoryService _mallProductCategoryService;
        private readonly IListCacheRepository<MallProductCategory> _listCacheMallProductCategoryRepository;
        public MallProductCategoryApp(IMallProductCategoryService mallProductCategoryService, IListCacheRepository<MallProductCategory> listCacheMallProductCategoryRepository)
        {
            _mallProductCategoryService = mallProductCategoryService;
            _listCacheMallProductCategoryRepository= listCacheMallProductCategoryRepository;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<MallProductCategoryDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _mallProductCategoryService.GetPageListAsync(input);
            return result;
        }

        [HttpPost]
        public async Task<MallProductCategoryDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _mallProductCategoryService.GetSingleByIdAsync(input);
            return result;
        }
        /// <summary>
        /// 新增或编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] MallProductCategoryDto input)
        {
            await _mallProductCategoryService.AddOrUpdateAsync(input);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _mallProductCategoryService.LogicDeleteAsync(input);
        }

        /// <summary>
        /// 获取所有目录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<MallProductCategoryDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _mallProductCategoryService.GetAllList(input).OrderBy(x=>x.Id);
            return result;
        }

        /// <summary>
        /// 通过商品类型id获取目录列表
        /// </summary>
        /// <param name="mallProductTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<MallProductCategory> GetListByTypeId(long mallProductTypeId)
        {
            var result = _listCacheMallProductCategoryRepository.Query(x=>x.MallProductTypeId==mallProductTypeId).OrderBy(x=>x.SortNo).ToList();
            return result;
        }
    }
}