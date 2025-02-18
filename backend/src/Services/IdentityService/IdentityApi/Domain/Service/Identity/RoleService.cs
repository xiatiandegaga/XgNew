using Cloud.Mapster;
using Cloud.Models;
using Cloud.Repositories;
using Cloud.Repositories.EntityFrameworkCore;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Domain.Service.Base;
using Identity.Shared.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Domain.Service.Identity
{
    public class RoleService : BaseListCacheService<Role, RoleDto>, IRoleService
    {
        private readonly ICloudUnitOfWork _unitWork;
        private readonly IListCacheRepository<RoleMenu> _listCacheRoleMenuRepository;

        public RoleService(IListCacheRepository<Role> repository, ICloudUnitOfWork unitWork, IListCacheRepository<RoleMenu> listCacheRoleMenuRepository) : base(repository)
        {
            _unitWork = unitWork;
            _listCacheRoleMenuRepository = listCacheRoleMenuRepository;
        }


        public override async Task<RoleDto> GetSingleByIdAsync(IdQueryCommonInput input)
        {
            var result = await base.GetSingleByIdAsync(input);
            result.RoleMenuIds = (await _listCacheRoleMenuRepository.QueryAsync(x => x.RoleId == input.Id)).Select(x => x.MenuId).ToList();
            return result;
        }


        public override async Task AddOrUpdateAsync(RoleDto input)
        {
            var entity = input.MapTo<Role>();
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new MyException("角色名称不能为空！");
            }
            if (entity.Id == default)
            {
                if (await _unitWork.ExistsAsync<Role>(x => x.Name == input.Name))
                    throw new MyException("角色名称已存在！");
                _unitWork.Add(entity);
            }
            else
            {
                if (await _unitWork.ExistsAsync<Role>(x => x.Name == input.Name && x.Id != entity.Id))
                    throw new MyException("角色名称已存在！");
                 _unitWork.Update(entity);
                _unitWork.ExecuteDelete<RoleMenu>(x => x.RoleId == entity.Id);
            }
            if (input.RoleMenuIds != default && input.RoleMenuIds.Count > 0)
            {
                var roleMenus = new List<RoleMenu>();
                input.RoleMenuIds.ForEach(x =>
                {
                    roleMenus.Add(new RoleMenu { RoleId = entity.Id, MenuId = x });
                });
                _unitWork.AddRange(roleMenus);
            }
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<Role>();
            await _unitWork.RemoveListCacheAsync<RoleMenu>();
        }

    }
}
