using Domain.Entity.Dictionary;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using System.Threading.Tasks;

namespace Domain.IService.Dictionary
{
    public interface IGlobalDataDetailService : IBaseListCacheService<GlobalDataDetail, GlobalDataDetailDto>
    {
        Task DeleteAsync(IdQueryCommonInput input);
    }
}
