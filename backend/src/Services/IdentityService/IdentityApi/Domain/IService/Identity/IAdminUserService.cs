using Domain.Entity.Identity;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.Input;
using System.Threading.Tasks;

namespace Domain.IService.Identity
{
    public interface IAdminUserService : IBaseCacheService<User, UserDto>
    {
        Task<AdminUserCreateOrEditInput> FindSingleEditByIdAsync(IdQueryCommonInput input);

        Task AdminAddOrUpdateAsync(AdminUserCreateOrEditInput dto);

        Task ResetPasswordAsync(IdQueryCommonInput input);
    }
}
