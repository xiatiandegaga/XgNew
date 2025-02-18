using Cloud.Caching;
using Cloud.Extensions;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Utilities;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Identity.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Identity
{
    public class AuthenticationApp : ICloudApp
    {
        private readonly ICacheRepository<User> _cacheUserRepository;
        private readonly IAuthenticationPrincipalService _authenticationService;
        public AuthenticationApp(ICacheRepository<User> cacheUserRepository,
       IAuthenticationPrincipalService authenticationService)
        {
            _cacheUserRepository = cacheUserRepository;
            _authenticationService = authenticationService;
        }

        public async Task<User> Login(string account, string password)
        {
            return await _cacheUserRepository.GetSingleAsync(u => (u.Account == account || u.Mobile == account) && u.Password == password);
        }

        public async Task<User> AccountLoginAsync(string account, string password)
        {
            var rsaPassword = EncryptionUtility.MD5(account.StrReverse() + password);
            return await _cacheUserRepository.GetSingleAsync(u => (u.Account == account || u.Mobile == account) && u.Password == rsaPassword);
        }

        public async Task<bool> CheckVerifyCodeAsync(string code)
        {
            return await _authenticationService.CheckVerifyCodeAsync(code);
        }


        /// <summary>
        /// GetMobileVerifyCodeAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="cachingExpireType"></param>
        /// <returns></returns>
        public async Task<string> GetMobileVerifyCodeAsync(string mobile, CachingExpireType cachingExpireType = CachingExpireType.MobileCodeVerify)
        {
            if (!await _cacheUserRepository.ExistsAsync(x => x.Account == mobile && x.Password != "")) throw new MyException("该手机号尚未开通登录权限！", 0);
            return await _authenticationService.GetMobileVerifyCodeAsync(mobile, cachingExpireType);
        }

        /// <summary>
        /// CheckMobileVerifyCodeAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> CheckMobileVerifyCodeAsync(string mobile, string code)
            => await _authenticationService.CheckMobileVerifyCodeAsync(mobile, code);


        public async Task<IEnumerable<User>> GetUserByListId(List<long> listId)
        {
            return await _cacheUserRepository.GetListAsync(x => listId.Contains(x.Id));
        }

        /// <summary>
        ///FindSingleByAccount
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task<User> FindSingleByAccount(string mobile)
        {
            return await _cacheUserRepository.GetSingleAsync(x => x.Account == mobile);
        }

        public string SignIn(User user)
        {
            return _authenticationService.SignIn(user);
        }

        public async Task<User> GetAuthenticatedUserAsync() => await _authenticationService.GetAuthenticatedUserAsync();

        public async Task<UserDto> GetAuthUserDtoAsync()
        {
            var entity = await _authenticationService.GetAuthenticatedUserAsync();
            var user = entity.MapTo<UserDto>();
            if (user == null) throw new MyException("用户不能为空", 0);
            return user;
        }
    }
}
