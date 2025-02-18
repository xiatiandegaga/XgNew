using Domain.Entity.Identity;
using Domain.IService.Base;
using Identity.Shared.Dto;

namespace Domain.IService.Identity
{
    public interface IRoleService : IBaseListCacheService<Role, RoleDto>
    {

    }
}
