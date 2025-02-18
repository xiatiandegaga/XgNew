using Cloud.Extensions;
using Cloud.Mapster;
using Cloud.Models;
using Cloud.Mvc;
using Cloud.Mvc.Filters;
using Cloud.Repositories;
using Cloud.Utilities;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.Admin.Output;
using Identity.Shared.Dto.H5.Input;
using Identity.Shared.Dto.H5.Output;
using Identity.Shared.Dto.WeChat.Input;
using Identity.Shared.Dto.WeChat.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Identity
{
    public class LoginApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly ICacheRepository<User> _cacheUserRepository;
        private readonly IAuthenticationPrincipalService _authenticationService;
        private readonly IUserService _userService;
        private readonly IWechatLoginService _wechatLoginService;
        public LoginApp(ICacheRepository<User> cacheUserRepository,
                                    IAuthenticationPrincipalService authenticationService,
                                    IUserService userService,
                                    IWechatLoginService wechatLoginService)
        {
            _cacheUserRepository = cacheUserRepository;
            _authenticationService = authenticationService;
            _userService = userService;
            _wechatLoginService = wechatLoginService;
        }

        /// <summary>
        /// WeChatLogin
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        [HttpPost, AllowAnonymous]
        public async Task<WeChatUserLoginTokenOutput> WeChatLogin([FromBody] WeChatCodeInput input)
        {
            var jsCode2SessionResponse = await _wechatLoginService.WeChatLogin(input.Code);
            if (string.IsNullOrEmpty(jsCode2SessionResponse.Openid))
            {
                throw new MyException("获取Openid失败！");
            }
            var data = await _userService.UserLoginAsync(input, jsCode2SessionResponse.Openid);
            var token = _authenticationService.SignIn(data.Item1);
            return new WeChatUserLoginTokenOutput { IsFirstLogin = data.Item2, Token = token };
        }

        /// <summary>
        /// H5Login
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        [HttpPost, AllowAnonymous]
        public async Task<H5UserLoginTokenOutput> H5Login([FromBody] H5UserSignMobileInput input)
        {
            var user = await _userService.H5UserLoginAsync(input);
            var token = _authenticationService.SignIn(user);
            var userDto= user.MapTo<User, UserDto>();
            return new H5UserLoginTokenOutput { Token = token ,userDto=userDto};
        }

        /// <summary>
        ///  AccountLogin
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<AdminResponseLoginUserOutput> AccountLogin([FromBody] AdminRequestLoginUserInput input)
        {
            var account = input.Account.Trim();
            var password = input.Password.Trim();
            var rsaPassword = EncryptionUtility.MD5(account.StrReverse() + password);
            var user = await _cacheUserRepository.GetSingleAsync(u => (u.Account == account || u.Mobile == account) && u.Password == rsaPassword);
            if (user == default)
                throw new MyException("账号或密码错误");
            var token = _authenticationService.SignIn(user);
            var userDto = new AdminResponseLoginUserOutput
            {
                Token = token,
                NickName = user.NickName,
                RealName = user.RealName,
                Avatar = user.Img
            };
            return userDto;
        }

        /// <summary>
        /// GetAuthenticatedUser
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserDto> GetAuthenticatedUserAsync()
        {
            var user = (await _authenticationService.GetAuthenticatedUserAsync()).MapTo<User, UserDto>();
            if (user == default)
                throw new MyException("用户未知");
            return user;
        }


        /// <summary>
        /// download excel
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, NoWrapper]
        public FileContentResult DownloadTestFileAsync()
        {
            var lst = new List<User>
            {
               new User{NickName="1" },
               new User{NickName="2" }
            };
            FileContentResult fileContent = new(ExcelUtility.DumpExcel(lst.CopyToDataTable()), "application/ms-excel")
            {
                FileDownloadName = "test.xlsx"
            };
            return fileContent;
        }

    }
}
