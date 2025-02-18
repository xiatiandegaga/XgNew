using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Snowflake;
using Cloud.Utilities;
using Cloud.Utilities.Json;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Domain.Service.Base;
using Identity.Shared;
using Identity.Shared.Dto;
using Identity.Shared.Dto.Admin.Input;
using Identity.Shared.Dto.H5.Input;
using Identity.Shared.Dto.WeChat.Input;
using Mapster;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Service.Identity
{
    public class UserService(ICloudUnitOfWork unitWork, ICacheRepository<User> cacheUserRepository, IConfiguration configuration, ISnowflakeIdWorker snowflakeIdWorker) : BaseCacheService<User, UserDto>(cacheUserRepository), IUserService
    {
        private readonly ICloudUnitOfWork _unitWork = unitWork;
        private readonly IConfiguration _configuration = configuration;
        private readonly ISnowflakeIdWorker _snowflakeIdWorker = snowflakeIdWorker;

        public async Task<(User, int)> UserLoginAsync(WeChatCodeInput userInfo, string thirdpartId)
        {
            int isFirstLogin = 0;
            var user = await _unitWork.GetSingleAsync<User>(x => x.ThirdpartId == thirdpartId);
            if (user == default)
            {
                var entity = new User
                {
                    Id = _snowflakeIdWorker.NextId(),
                    UserCategory = IdentityCommonConst.UserCategory_Front,
                    NickName = "微信用户",
                    ThirdpartId= thirdpartId,
                };
                _unitWork.Add(entity);
                await _unitWork.CommitAsync();
                await _unitWork.UpdateSingleCacheAsync(entity);
                isFirstLogin = 1;
                return (entity, isFirstLogin);
            }
            else
            {
                if (user.ReferId == default && userInfo.Pid != default && userInfo.Pid != user.Id)
                {
                    var pUser = await _unitWork.GetSingleAsync<User>(x => x.Id == userInfo.Pid);
                    if (pUser == default || string.IsNullOrWhiteSpace(pUser.Mobile))
                    {
                        return (user, isFirstLogin);
                    }
                    else
                    {
                        user.ReferId = userInfo.Pid;
                        if (user.Pid == default)
                        {
                            user.Pid = userInfo.Pid;
                        }
                        _unitWork.Update(user);
                        await _unitWork.CommitAsync();
                        await _unitWork.UpdateSingleCacheAsync(user);
                    }
                }
                return (user, isFirstLogin);
            }
        }

        public async Task<User> H5UserLoginAsync(H5UserSignMobileInput input)
        {
            var  userPrivateKeyHex = _configuration["RemoteServices:JSBankMallH5:UserPrivateKey"];
            var customerJson=SMSignUtility.Sm2DecryptString(userPrivateKeyHex, input.SignMobile);
            var jsBankUserModel = JsonUtility.Deserialize<H5UserJsBankUserInput>(customerJson);
            if(jsBankUserModel==default || string.IsNullOrWhiteSpace(jsBankUserModel.MobileNo))
            {
                throw new MyException("用户加密信息不正确");
            }
            var user = await _unitWork.GetSingleAsync<User>(x => x.Mobile == jsBankUserModel.MobileNo);
            if (user == default)
            {
                var entity = new User
                {
                    Id = _snowflakeIdWorker.NextId(),
                    UserCategory = IdentityCommonConst.UserCategory_Front,
                    NickName = "江苏银行用户",
                    Mobile = jsBankUserModel.MobileNo,
                };
                _unitWork.Add(entity);
                await _unitWork.CommitAsync();
                await _unitWork.UpdateSingleCacheAsync(entity);
                user = entity;

            }
            return user ;

        }

        public async Task UpdateAsync(AdminUserCreateOrEditInput dto)
        {
            var entity = dto.MapTo<User>();
             _unitWork.Update(entity);
            await _unitWork.CommitAsync();
            await _unitWork.UpdateSingleCacheAsync(entity);
        }
        /// <summary>
        /// 远程搜索(用户名或手机号)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public List<UserDto> GetRemoteSerch(string key)
        {
           return  _unitWork.Paginate<User>(1, 20, x => x.RealName.Contains(key) || x.Mobile.Contains(key)).ProjectToType<UserDto>().ToList();
        }
    }

}
