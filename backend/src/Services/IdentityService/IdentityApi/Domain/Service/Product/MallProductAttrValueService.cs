using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Product;
using Domain.IService.Product;
using Domain.Service.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Service.Product
{
    /// <summary>
    /// 产品属性表值value
    /// </summary>
    public class MallProductAttrValueService : BaseListCacheService<MallProductAttrValue,MallProductAttrValueDto>, IMallProductAttrValueService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IListCacheRepository<MallProductAttrValue> _listCacheMallProductAttrValueRepository;
        public MallProductAttrValueService(IListCacheRepository<MallProductAttrValue> listCacheMallProductAttrValueRepository, ICloudUnitOfWork unitWork) : base(listCacheMallProductAttrValueRepository)
        {
            _unitWork = unitWork;
            _listCacheMallProductAttrValueRepository= listCacheMallProductAttrValueRepository;
        }
        public async Task<List<MallProductAttrValue>> GetListAsync()
        {
            return await  _listCacheMallProductAttrValueRepository.GetListAsync();
        }
        public override async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            if (await _unitWork.ExistsAsync<MallProductAttr>(x => x.MallProductAttrValueId == input.Id))
            {
                throw new MyException("已存在该属性值的商品，无法删除！");
            }
            _unitWork.ExecuteDelete<MallProductAttrValue>(x => x.Id == input.Id);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<MallProductAttrValue>();
        }
    }
}
