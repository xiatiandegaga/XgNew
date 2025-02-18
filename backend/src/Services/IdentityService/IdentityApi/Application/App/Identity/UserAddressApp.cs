using Cloud.Models;
using Cloud.Mvc;
using Cloud.Repositories;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xg.Cloud.Core;
namespace Application.App.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAddressApp : ICloudApp, ICloudDynamicWebApi
    {
        private readonly IUserAddressService _userAddressService;
        private readonly IRepository<UserAddress> _userAddressRepository;
        public UserAddressApp(IUserAddressService userAddressService, IRepository<UserAddress> userAddressRepository)
        {
            _userAddressService = userAddressService;
            _userAddressRepository = userAddressRepository;
        }

        [HttpPost]
        public async Task<PagingData<IEnumerable<UserAddressDto>>> GetPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var result = await _userAddressService.GetPageListAsync(input);
            return result;
        }
        /// <summary>
        /// 获取登录人的收获地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagingData<IEnumerable<UserAddressDto>>> GetMyPageList([FromBody] PageQueryCommonInput input)
        {
            return await _userAddressService.GetMyPageListAsync(input);
        }
        [HttpPost]
        public async Task<UserAddressDto> GetSingleByIdAsync([FromBody] IdQueryCommonInput input)
        {
            var result = await _userAddressService.GetSingleByIdAsync(input);
            return result;
        }
        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddOrUpdateAsync([FromBody] UserAddress input)
        {
            await _userAddressService.AddOrUpdateAsync(input);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task LogicDeleteAsync([FromBody] IdQueryCommonInput input)
        {
            await _userAddressService.LogicDeleteAsync(input);
        }


        /// <summary>
        /// 设为默认
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        [HttpPost]
        public async Task SetDefaultAsync([FromBody] IdQueryCommonInput input)
        {
            await _userAddressService.SetDefaultAsync(input.Id);
        }
    }
}