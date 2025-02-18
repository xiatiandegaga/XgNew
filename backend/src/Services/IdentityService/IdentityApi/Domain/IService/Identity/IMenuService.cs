using Domain.Entity.Identity;
using Domain.IService.Base;
using Identity.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Identity
{
    public interface IMenuService : IBaseListCacheService<Menu, MenuDto>
    {
       Task<IEnumerable<MenuDto>> GetUserMenusAsync();

    }
}
