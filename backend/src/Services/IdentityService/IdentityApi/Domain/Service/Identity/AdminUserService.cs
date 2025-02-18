
using Cloud.Mapster;
using Cloud.Extensions;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Utilities;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Domain.Service.Base;
using Identity.Shared;
using Identity.Shared.Dto;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xg.Cloud.Core;
using Identity.Shared.Dto.H5.Input;
using Identity.Shared.Dto.Admin.Input;

namespace Domain.Service.Identity
{
    public class AdminUserService : BaseCacheService<User, UserDto>, IAdminUserService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly ICacheRepository<User> _cacheUserRepository;
        private readonly IListCacheRepository<UserRole> _listCacheUserRoleRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService;
        public AdminUserService(ICloudUnitOfWork unitWork, ICacheRepository<User> cacheUserRepository, IListCacheRepository<UserRole> listCacheUserRoleRepository, IConfiguration configuration, IAuthenticationPrincipalService authenticationPrincipalService) : base(cacheUserRepository)
        {
            _unitWork = unitWork;
            _cacheUserRepository = cacheUserRepository;
            _listCacheUserRoleRepository = listCacheUserRoleRepository;
            _configuration = configuration;
            _authenticationPrincipalService = authenticationPrincipalService;
        }


        public async Task<AdminUserCreateOrEditInput> FindSingleEditByIdAsync(IdQueryCommonInput input)
        {
            var result = (await _cacheUserRepository.GetSingleByIdAsync(input.Id)).MapTo<User, AdminUserCreateOrEditInput>();
            result.UserRoleIds = (await _listCacheUserRoleRepository.QueryAsync(x => x.UserId == result.Id)).Select(x => x.RoleId).ToList();
            return result;
        }


        public async Task AdminAddOrUpdateAsync(AdminUserCreateOrEditInput dto)
        {
            var entity = dto.MapTo<User>();
            if (string.IsNullOrWhiteSpace(entity.Account))
            {
                throw new MyException($"账号{CommonConst.Tips_DataNull}");
            }
            if (entity.Id == default)
            {
                entity.UserCategory = IdentityCommonConst.UserCategory_Backend;
                 _unitWork.Add(entity);
            }
            else
            {
                 _unitWork.Update(entity);
                _unitWork.ExecuteDelete<UserRole>(x => x.UserId == entity.Id);
            }
            if (dto.UserRoleIds != default && dto.UserRoleIds.Count > 0)
            {
                var userRoles = new List<UserRole>();
                dto.UserRoleIds.ForEach(x =>
                {
                    userRoles.Add(new UserRole { UserId = entity.Id, RoleId = x });
                });
                _unitWork.AddRange(userRoles);
            }
            await _unitWork.CommitAsync();
            await _unitWork.UpdateSingleCacheAsync(entity);
            await _unitWork.RemoveListCacheAsync<UserRole>();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public async Task UpdatePasswordAsync(AdminUserChangePwdInput input)
        {
            if (input == null) throw new MyException("参数不能为空！", CommonConst.No);
            if (string.IsNullOrWhiteSpace(input.OldPassword)) throw new MyException("原密码不能为空！", CommonConst.No);
            if (string.IsNullOrWhiteSpace(input.NewPassword)) throw new MyException("新密码不能为空！", CommonConst.No);
            if (string.IsNullOrWhiteSpace(input.ConfirmPassword)) throw new MyException("确认密码不能为空！", CommonConst.No);

            input.ConfirmPassword = input.ConfirmPassword.Trim();
            input.NewPassword = input.NewPassword.Trim();
            if (input.ConfirmPassword != input.NewPassword) throw new MyException("新密码和确认密码不一致", 0);

            var loginUser = await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            if (loginUser.Password != EncryptionUtility.MD5(loginUser.Account.StrReverse() + input.OldPassword)) throw new MyException("原密码不正确！", 0);

            loginUser.Password = EncryptionUtility.MD5(loginUser.Account.StrReverse() + input.NewPassword);
            _unitWork.Update<User>(loginUser);
            await _unitWork.CommitAsync();
            await _unitWork.UpdateSingleCacheAsync<User>(loginUser);
        }

        public async Task ResetPasswordAsync(IdQueryCommonInput input)
        {
            if (input.Id == default) throw new MyException("用户id不能为空！");
            var user = await _unitWork.GetSingleAsync<User>(x => x.Id == input.Id);
            user.Password = EncryptionUtility.MD5($"{user.Account.StrReverse()}{EncryptionUtility.MD5(_configuration["DefaultSetting:Password"])}");
            _unitWork.Update(user);
            await _unitWork.CommitAsync();
            await _unitWork.UpdateSingleCacheAsync(user);
        }

        public override async Task LogicDeleteAsync(IdQueryCommonInput input)
        {
            var entity = await _unitWork.GetSingleAsync<User>(x => x.Id == input.Id);
            #region 系统管理员 不能删
            if (entity.IsSystem == CommonConst.Yes)
            {
                throw new MyException("您无权删除该用户！");
            }
            #endregion
            entity.DeletedStatus = CommonConst.DeletedStatus_Deleted;
            _unitWork.Update(entity);
            _unitWork.ExecuteDelete<UserRole>(x => x.UserId == input.Id);
            await _unitWork.CommitAsync();
            await _unitWork.RemoveSingleCacheAsync(entity);
            await _unitWork.RemoveListCacheAsync<UserRole>();
        }
    }

}
