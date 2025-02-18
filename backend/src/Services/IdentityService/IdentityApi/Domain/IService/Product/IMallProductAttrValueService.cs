using Domain.Entity.Product;
using Domain.IService.Base;
using Identity.Shared.Dto.Admin.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Product
{
    public interface IMallProductAttrValueService : IBaseListCacheService<MallProductAttrValue, MallProductAttrValueDto>
    {
        Task<List<MallProductAttrValue>> GetListAsync();
    }
}
