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
    /// 产品类型表
    /// </summary>
    public class MallProductTypeService : BaseListCacheService<MallProductType, MallProductTypeDto>, IMallProductTypeService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IListCacheRepository<MallProductType> _listCacheMallProductType;

        public MallProductTypeService(IListCacheRepository<MallProductType> listCacheMallProductType, ICloudUnitOfWork unitWork) : base(listCacheMallProductType)
        {
            _unitWork = unitWork;
            _listCacheMallProductType = listCacheMallProductType;
        }

        public override async Task AddOrUpdateAsync(MallProductTypeDto input)
        {
            var entity = input.MapTo<MallProductType>();
            if (string.IsNullOrWhiteSpace(entity.TypeCode))
            {
                throw new MyException("类型编号不能为空");
            }
            if (string.IsNullOrWhiteSpace(entity.TypeName))
            {
                throw new MyException("类型名称不能为空");
            }
            if (entity.Id > 0)
            {
                if (await _unitWork.ExistsAsync<MallProductType>(x => x.Id != entity.Id && x.TypeCode == entity.TypeCode))
                {
                    throw new MyException($"类型编号【{entity.TypeCode}】已存在！");
                }
                if (await _unitWork.ExistsAsync<MallProductType>(x => x.Id != entity.Id && x.TypeName == entity.TypeName))
                {
                    throw new MyException($"类型名称【{entity.TypeName}】已存在！");
                }
                await _listCacheMallProductType.UpdateAsync(entity);
            }
            else
            {
                if (await _unitWork.ExistsAsync<MallProductType>(x => x.TypeCode == entity.TypeCode))
                {
                    throw new MyException($"类型编号【{entity.TypeCode}】已存在！");
                }
                if (await _unitWork.ExistsAsync<MallProductType>(x => x.TypeName == entity.TypeName))
                {
                    throw new MyException($"类型名称【{entity.TypeName}】已存在！");
                }
                await _listCacheMallProductType.AddAsync(entity);
            }
        }


        public new async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            if (await _unitWork.ExistsAsync<MallProduct>(x => x.MallProductTypeId == input.Id))
            {
                throw new MyException("该商品类型下已经存在商品信息，无法删除！");
            }
            await _listCacheMallProductType.ExecuteDeleteAsync(x => x.Id == input.Id);
        }

    }
}
