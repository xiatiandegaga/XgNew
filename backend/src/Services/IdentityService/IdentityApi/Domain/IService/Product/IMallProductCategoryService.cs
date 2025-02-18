using Domain.Entity.Product;
using Domain.IService.Base;
using Identity.Shared.Dto.Admin.General;

namespace Domain.IService.Product
{
    public interface IMallProductCategoryService : IBaseListCacheService<MallProductCategory, MallProductCategoryDto>
    {

    }
}
