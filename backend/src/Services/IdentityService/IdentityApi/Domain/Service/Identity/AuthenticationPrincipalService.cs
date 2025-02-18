using Cloud.Caching;
using Cloud.Models;
using Cloud.Repositories;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Domain.Service.Identity
{
    public class AuthenticationPrincipalService : IAuthenticationPrincipalService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ICacheRepository<User> _cacheUserRepository;
        private User _signedInUser;
        private ICache _cache;
        private readonly ILogger<AuthenticationPrincipalService> _logger;
        public AuthenticationPrincipalService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ICacheRepository<User> cacheUserRepository, ICache cache, ILogger<AuthenticationPrincipalService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _cacheUserRepository = cacheUserRepository;
            _cache = cache;
            _logger= logger;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录的用户</param>
        public string SignIn(User user)
        {
            //添加Claims信息
            var claims = new[] {
                                 new Claim( ClaimTypes.NameIdentifier,user.Id.ToString()) };
            var rsa = RSA.Create();
            rsa.ImportPrivateKey(RSAKeyType.Pkcs8, _configuration["Jwt:priKey"]);
            var key = new RsaSecurityKey(rsa);
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,//添加claims
              notBefore: DateTime.Now,
              expires: DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:expires_hours"])),
              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 注销
        /// </summary>
        public async Task SignOutAsync() =>await  _httpContextAccessor.HttpContext.SignOutAsync(_configuration["AuthenticationScheme"]);

        /// <summary>
        /// 获取当前认证的用户
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        public async Task<User> GetAuthenticatedUserAsync()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || !httpContext.User.Identity.IsAuthenticated)
            {
                throw new MyException("登录信息错误，请重新登录！", 0);
                //return null;
            }
            if (_signedInUser != null)
                return _signedInUser;
            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;
            long id = Convert.ToInt64(claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _signedInUser = await _cacheUserRepository.GetSingleByIdAsync(id);
            if (_signedInUser == null) throw new MyException("登录信息错误，请重新登录！", 0);
            return _signedInUser;
        }

        public async Task<long> GetAuthenticatedUserIdAsync()
        {
            return (await GetAuthenticatedUserAsync()).Id;
        }

        /// <summary>
        /// 获取授权token,用于扫描二维码等
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cachingExpireType"></param>
        /// <returns></returns>
        public async Task<string> GetTokenAsync(string data, CachingExpireType cachingExpireType = CachingExpireType.ObjectCollection)
        {
            var token = Guid.NewGuid().ToString("N");
            await _cache.SetAsync(token, data, cachingExpireType);
            return token;
        }

        //}
        /// <summary>
        /// 检查验证码是否匹配
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> CheckVerifyCodeAsync(string code)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            request.Cookies.TryGetValue(_configuration["Cookie:VerifyCode"], out string verifyCodeKey);
            if (string.IsNullOrEmpty(verifyCodeKey))
                return false;
            if (!await _cache.ExistsAsync(verifyCodeKey))
                return false;
            var correctVerifyCode = await _cache.GetAsync<string>(verifyCodeKey);
            if (string.IsNullOrEmpty(correctVerifyCode))
                return false;
            if (correctVerifyCode == code)
                return true;
            return false;
        }


        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="cachingExpireType"></param>
        /// <returns></returns>
        public async Task<string> GetMobileVerifyCodeAsync(string mobile, CachingExpireType cachingExpireType = CachingExpireType.MobileCodeVerify)
        {
            var token = $"MobileVerifyCode:{mobile}";
            int pre = RandomNumberGenerator.GetInt32(0, 10);
            int num = RandomNumberGenerator.GetInt32(100, 1000);
            var code = pre.ToString() + num.ToString();
            await _cache.SetAsync(token, code, cachingExpireType);
            return code;
        }
        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> CheckMobileVerifyCodeAsync(string mobile, string code)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                throw new MyException("手机号不能为空", 0);
            if (string.IsNullOrWhiteSpace(code))
                throw new MyException("验证码不能为空", 0);
            var token = $"MobileVerifyCode:{mobile}";
            var value = await _cache.GetAsync<string>(token);
            if (string.IsNullOrWhiteSpace(value)) throw new MyException("验证码已失效！", 0);
            return code == value;
        }
    }
}
