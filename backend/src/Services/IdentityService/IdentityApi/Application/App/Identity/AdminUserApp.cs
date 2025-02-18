using Cloud.Models;
using Cloud.Mvc;
using Domain.Filter;
using Domain.IService.Identity;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.Input;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Application.App.Identity
{
    public class AdminUserApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IAdminUserService _adminUserService;
        public AdminUserApp(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        /// <summary>
        /// GetPageListAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [XgAuthorize("adminUser:add")]
        public async Task<PagingData<IEnumerable<UserDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _adminUserService.PaginateAsync(input);
            return result;
        }

        /// <summary>
        /// GetSingleByIdAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUserCreateOrEditInput> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _adminUserService.FindSingleEditByIdAsync(input);
            return result;
        }


        /// <summary>
        /// AddOrUpdateAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] AdminUserCreateOrEditInput input)
        {
            await _adminUserService.AdminAddOrUpdateAsync(input);
        }

        /// <summary>
        /// LogicDeleteAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _adminUserService.LogicDeleteAsync(input);
        }

        /// <summary>
        /// ResetPasswordAsync
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task ResetPasswordAsync([FromBody] IdQueryCommonInput input)
        {
            await _adminUserService.ResetPasswordAsync(input);
        }

    }
}
