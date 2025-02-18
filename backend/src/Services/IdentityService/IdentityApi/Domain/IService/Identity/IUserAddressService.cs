using Cloud.Models;
using Domain.Entity.Identity;
using Domain.IService.Base;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.General;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IService.Identity
{
    public interface IUserAddressService : IBaseService<UserAddress, UserAddressDto>
    {
        /// <summary>
        /// 获取登录人的收获地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagingData<IEnumerable<UserAddressDto>>> GetMyPageListAsync([FromBody] PageQueryCommonInput input);
        /// <summary>
        /// 新增或修改收获地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        Task<UserAddress> AddOrUpdateAsync(UserAddress entity);
        
        /// <summary>
        /// 设为默认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        Task SetDefaultAsync(long id);
    }
}
