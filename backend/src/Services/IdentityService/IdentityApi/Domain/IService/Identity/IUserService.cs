using Domain.Entity.Identity;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.H5.Input;
using Identity.Shared.Dto.WeChat.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Identity
{
    public interface IUserService : IBaseCacheService<User, UserDto>
    {
        Task<(User, int)> UserLoginAsync(WeChatCodeInput userInfo, string thirdpartId);

        Task UpdateAsync(AdminUserCreateOrEditInput dto);

        /// <summary>
        /// 远程搜索(用户名或手机号)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        List<UserDto> GetRemoteSerch(string key);

        /// <summary>
        /// 江苏银行h5登陆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<User> H5UserLoginAsync(H5UserSignMobileInput input);
    }
}
