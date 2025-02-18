using Domain.Entity.Product;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto.Admin.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Product
{
    public interface IMallProductAttrKeyService : IBaseListCacheService<MallProductAttrKey, MallProductAttrKeyDto>
    {
        Task<List<MallProductAttrKey>> GetListAsync();
        Task<MallProductAttrKeyInfoOutput> FindSingleDetailsByIdAsync(IdQueryCommonInput input);

        Task<IEnumerable<MallProductAttrKeyInfoOutput>> GetAllDetailsListByCategoryIdAsync(IdQueryCommonInput input);

        Task AddOrUpdateAsync(MallProductAttrKeyInfoOutput input);

        Task<string> GetAttrNamesAsync(string attrValues, List<MallProductAttrKey> list = default);

        Task<string> GetAttrStringAsync(string attrValues, List<MallProductAttrKey> list = default);
    }
}
