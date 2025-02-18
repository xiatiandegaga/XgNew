using Cloud.Caching;
using Domain.Entity.Identity;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.IService.Identity
{
    /// <summary>
    /// 用于身份认证的接口
    /// </summary>
    /// <remarks>实例的生命周期为每HttpRequest</remarks>
    public interface IAuthenticationPrincipalService : ICloudService
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录的用户</param>
        string SignIn(User user);

        /// <summary>
        /// 注销
        /// </summary>
        Task SignOutAsync();


        Task<long> GetAuthenticatedUserIdAsync();
        /// <summary>
        /// 获取当前登录的用户异步方法
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        Task<User> GetAuthenticatedUserAsync();

        /// <summary>
        /// 检查验证码是否匹配
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> CheckVerifyCodeAsync(string code);

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="cachingExpireType"></param>
        /// <returns></returns>
        Task<string> GetMobileVerifyCodeAsync(string mobile, CachingExpireType cachingExpireType = CachingExpireType.MobileCodeVerify);

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> CheckMobileVerifyCodeAsync(string mobile, string code);
    }
}
