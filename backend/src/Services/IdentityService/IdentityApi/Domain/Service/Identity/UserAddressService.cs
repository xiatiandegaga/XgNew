using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Domain.Service.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;
using Cloud.Extensions;
using Mapster;
using Identity.Shared.Dto.Admin.General;
using Identity.Shared.Dto;

namespace Domain.Service.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAddressService : BaseService<UserAddress, UserAddressDto>, IUserAddressService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService;

        public UserAddressService(ICacheRepository<UserAddress> repository, ICloudUnitOfWork unitWork, IAuthenticationPrincipalService authenticationPrincipalService) : base(repository)
        {
            _unitWork = unitWork;
            _authenticationPrincipalService = authenticationPrincipalService;
        }

        /// <summary>
        /// 获取登录人的收获地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagingData<IEnumerable<UserAddressDto>>> GetMyPageListAsync([FromBody] PageQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<UserAddress>();
            var loginUser=await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            if (express == default) {
                express = LinqExtensions.True<UserAddress>();
            }

            express = express.CloudAnd(x=>x.UserId==loginUser.Id);
            var list = _unitWork.Paginate<UserAddress>(input.PageNumber, input.PageSize, express, "IsDefault desc,Id desc").ProjectToType<UserAddressDto>().ToList();
            var totalCount = await  _unitWork.CountAsync<UserAddress>(express);
            return new PagingData<IEnumerable<UserAddressDto>> { PageIndex = input.PageNumber, TotalCount = totalCount, List = list };
        }
        /// <summary>
        /// 新增或修改收获地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public async Task<UserAddress> AddOrUpdateAsync(UserAddress entity)
        {
            if (entity == null) throw new MyException("对象不能为空！", 0);
            if (string.IsNullOrWhiteSpace(entity.ReceiverDetailInfo)) throw new MyException("详细地址不能为空！", CommonConst.No);
            if (string.IsNullOrWhiteSpace(entity.ReceiverName)) throw new MyException("联系人不能为空！", CommonConst.No);
            if (string.IsNullOrWhiteSpace(entity.ReceiverMobile)) throw new MyException("联系方式不能为空！", CommonConst.No);
            var user = await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            if (entity.IsDefault == 1)
            {
                var list = _unitWork.Query<UserAddress>(x => x.UserId == user.Id&&x.Id!=entity.Id).ToList();
                if (list != null && list.Count > 0)
                {
                    list.ForEach(item =>
                    {
                        
                            item.IsDefault = 0;
                            _unitWork.Update<UserAddress>(item);
                        

                    });
                }
            }
            if (entity.Id == 0)
            {
                entity.UserId = user.Id;
                _unitWork.Add<UserAddress>(entity);
            }
            else
            {
               _unitWork.Update<UserAddress>(entity);
            }
            await _unitWork.CommitAsync();
            return entity;
        }
        /// <summary>
        /// 设为默认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public async Task SetDefaultAsync(long id)
        {
            var user = await _authenticationPrincipalService.GetAuthenticatedUserAsync();

            var list = _unitWork.Query<UserAddress>(x => x.UserId == user.Id).ToList();
            if (list == null || list.Count == 0) throw new MyException("未找到有效的收货地址！", CommonConst.No);
            list.ForEach(item =>
            {
                if (item.Id == id) item.IsDefault = 1;
                else item.IsDefault = 0;
                _unitWork.Update<UserAddress>(item);
            });
            await _unitWork.CommitAsync();
        }
    }
}
