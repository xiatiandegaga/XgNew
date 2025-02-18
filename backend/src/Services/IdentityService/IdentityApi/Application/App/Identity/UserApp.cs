using Cloud.Models;
using Cloud.Mvc;
using Domain.IService.Identity;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.Input;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Identity
{
    public class UserApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IUserService _userService;
        public UserApp(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// GetPageList
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagingData<IEnumerable<UserDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _userService.PaginateAsync(input);
            return result;
        }

        /// <summary>
        /// GetSingleByIdAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _userService.GetSingleByIdAsync(input);
            return result;
        }

        /// <summary>
        /// AddOrUpdate
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Update([FromBody] AdminUserCreateOrEditInput input)
        {
            await _userService.UpdateAsync(input);
        }

        /// <summary>
        /// LogicDeleteAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _userService.LogicDeleteAsync(input);
        }

        /// <summary>
        /// 远程搜索(用户名或手机号)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]

        public List<UserDto> GetRemoteSerch(string key)
        {
            return _userService.GetRemoteSerch(key);
        }
    }
}
