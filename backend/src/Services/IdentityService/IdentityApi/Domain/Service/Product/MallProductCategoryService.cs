using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Product;
using Domain.IService.Product;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Domain.Service.Product
{
    /// <summary>
    /// 产品目录表
    /// </summary>
    public class MallProductCategoryService : BaseListCacheService<MallProductCategory,MallProductCategoryDto>, IMallProductCategoryService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IListCacheRepository<MallProductCategory> _listCacheMallProductCategoryRepository;
        public MallProductCategoryService(IListCacheRepository<MallProductCategory> listCacheMallProductCategoryRepository, ICloudUnitOfWork unitWork) : base(listCacheMallProductCategoryRepository)
        {
            _unitWork = unitWork;
            _listCacheMallProductCategoryRepository= listCacheMallProductCategoryRepository;
        }


        public override async Task AddOrUpdateAsync([FromBody] MallProductCategoryDto input)
        {
            var entity = input.MapTo<MallProductCategory>();
            if (entity.MallProductTypeId==default)
            {
                throw new MyException("商品类型不能为空");
            }
            if (string.IsNullOrWhiteSpace(entity.CategoryName))
            {
                throw new MyException("目录名称不能为空");
            }
            if (entity.Id > 0)
            {
                if (await _unitWork.ExistsAsync<MallProductCategory>(x => x.Id != entity.Id &&x.MallProductTypeId==entity.MallProductTypeId && x.CategoryName == entity.CategoryName))
                {
                    throw new MyException($"当前商品类型下目录名称【{entity.CategoryName}】已存在！");
                }
      
                await _listCacheMallProductCategoryRepository.UpdateAsync(entity);
            }
            else
            {
                if (await _unitWork.ExistsAsync<MallProductCategory>(x => x.MallProductTypeId == entity.MallProductTypeId && x.CategoryName == entity.CategoryName))
                {
                    throw new MyException($"当前商品类型下目录名称【{entity.CategoryName}】已存在！");
                }
                await _listCacheMallProductCategoryRepository.AddAsync(entity);
            }
        }

     
        public override async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            if (await _unitWork.ExistsAsync<MallProductCategory>(x => x.MallProductTypeId == input.Id))
            {
                throw new MyException("该商品目录下已经存在商品信息，无法删除！");
            }
            if(await _unitWork.ExistsAsync<MallProductAttrKey>(x=>x.MallProductCategoryId==input.Id))
            {
                throw new MyException("该商品目录下已经存在商品属性信息，无法删除！");
            }
            await _listCacheMallProductCategoryRepository.ExecuteDeleteAsync(x => x.Id == input.Id);
        }

    }
}
