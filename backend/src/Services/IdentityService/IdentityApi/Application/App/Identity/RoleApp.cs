using Cloud.Models;
using Cloud.Mvc;
using Domain.IService.Identity;
using Identity.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;
namespace Application.App.Identity
{
    /// <summary>
	/// Role应用层
	/// </summary>
    public class RoleApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IRoleService _roleService;
        public RoleApp(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<RoleDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _roleService.GetPageListAsync(input);
            return result;
        }

        [HttpPost]
        public async Task<RoleDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _roleService.GetSingleByIdAsync(input);
            return result;
        }

        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] RoleDto input)
        {
            await _roleService.AddOrUpdateAsync(input);
        }

        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _roleService.LogicDeleteAsync(input);
        }

        [HttpPost]
        public IEnumerable<RoleDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _roleService.GetAllList(input);
            return result;
        }
    }
}