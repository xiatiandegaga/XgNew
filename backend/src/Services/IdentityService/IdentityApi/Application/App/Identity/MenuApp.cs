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
	/// menu应用层
	/// </summary>
    public class MenuApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IMenuService _menuService;

        public MenuApp(IMenuService userMenuServic)
        {
            _menuService = userMenuServic;
        }

        /// <summary>
        /// GetPageList
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagingData<IEnumerable<MenuDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _menuService.GetPageListAsync(input);
            return result;
        }


        [HttpPost]
        public async Task<MenuDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _menuService.GetSingleByIdAsync(input);
            return result;
        }

        /// <summary>
        /// GetUserMenus
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<MenuDto>> GetUserMenusAsync()
        {
            var result =await  _menuService.GetUserMenusAsync();
            return result;
        }


        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] MenuDto input)
        {
            await _menuService.AddOrUpdateAsync(input);
        }
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _menuService.LogicDeleteAsync(input);
        }

        [HttpPost]
        public IEnumerable<MenuDto> GetAllList([FromBody] AllQueryCommonInput input)
        {
            var result = _menuService.GetAllList(input);
            return result;
        }
    }
}