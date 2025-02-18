using Cloud.Mapster;
using Cloud.Extensions;
using Cloud.Models;
using Cloud.Repositories;
using Domain.Entity.Identity;
using Domain.IService.Identity;
using Domain.Service.Base;
using Identity.Shared.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cloud.Repositories.EntityFrameworkCore;
using Xg.Cloud.Core;
namespace Domain.Service.Identity
{
    public class MenuService : BaseListCacheService<Menu, MenuDto>, IMenuService
    {
        private readonly IListCacheRepository<Menu> _listCacheMenuRepository;
        private readonly IListCacheRepository<RoleMenu> _listCacheRoleMenuRepository;
        private readonly IListCacheRepository<UserRole> _listCacheUserRoleRepository;
        private readonly IAuthenticationPrincipalService _authenticationPrincipalService;
        private readonly ICloudUnitOfWork _unitWork;

        public MenuService(IListCacheRepository<Menu> listCacheMenuRepository, IListCacheRepository<RoleMenu> listCacheRoleMenuRepository, IListCacheRepository<UserRole> listCacheUserRoleRepository, IAuthenticationPrincipalService authenticationPrincipalService, ICloudUnitOfWork unitWork) : base(listCacheMenuRepository)
        {
            _listCacheMenuRepository = listCacheMenuRepository;
            _listCacheRoleMenuRepository = listCacheRoleMenuRepository;
            _listCacheUserRoleRepository = listCacheUserRoleRepository;
            _authenticationPrincipalService = authenticationPrincipalService;
            _unitWork = unitWork;
        }


        public override async Task AddOrUpdateAsync(MenuDto input)
        {
            var entity = input.MapTo<Menu>();
            string menuTypeName = "菜单";
            if (input.Category == CommonConst.MenuCategory_Button)
            {
                menuTypeName = "按钮";
            }

            if (string.IsNullOrWhiteSpace(entity.MetaTitle))
            {
                throw new MyException($"{menuTypeName}名称不能为空！");
            }
            if (input.Category == CommonConst.MenuCategory_Menu && string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new MyException("路由名称不能为空！");
            }

            if (entity.Id == 0)
            {
                if (await _listCacheMenuRepository.ExistsAsync(x => x.Pid == entity.Pid && x.MetaTitle == entity.MetaTitle))
                {
                    throw new MyException($"当前目录下{menuTypeName}名称【{entity.MetaTitle}】已存在！");
                }
                if (input.Category == CommonConst.MenuCategory_Menu &&await  _listCacheMenuRepository.ExistsAsync(x => x.Name == entity.Name))
                {
                    throw new MyException($"路由名称【{entity.Name}】已存在！");
                }

                await _listCacheMenuRepository.AddAsync(entity);
            }
            else
            {
                if (await _listCacheMenuRepository.ExistsAsync(x => x.Pid == entity.Pid && x.MetaTitle == entity.MetaTitle && x.Id != entity.Id))
                {
                    throw new MyException($"当前目录下{menuTypeName}名称【{entity.MetaTitle}】已存在！");
                }
                if (input.Category == CommonConst.MenuCategory_Menu &&await  _listCacheMenuRepository.ExistsAsync(x => x.Name == entity.Name && x.Id != entity.Id))
                {
                    throw new MyException($"路由名称【{entity.Name}】已存在！");
                }
                await _listCacheMenuRepository.UpdateAsync(entity);
            }
        }
        public override async Task DeleteAsync([FromBody] IdQueryCommonInput input)
        {
            if (await _unitWork.ExistsAsync<Menu>(x => x.Pid == input.Id && x.Category == CommonConst.MenuCategory_Menu))
            {
                throw new MyException("该菜单下有子菜单，不能删除！");
            }
            var menuButtonList = _unitWork.Query<Menu>(x => x.Pid == input.Id && x.Category == CommonConst.MenuCategory_Button).Select(x => x.Id).ToList();
              _unitWork.ExecuteDelete<Menu>(x => x.Id == input.Id || (x.Pid == input.Id && x.Category == CommonConst.MenuCategory_Button));
             _unitWork.ExecuteDelete<RoleMenu>(x => x.MenuId == input.Id || menuButtonList.Contains(x.MenuId));
            await _unitWork.CommitAsync();
            await _unitWork.RemoveListCacheAsync<Menu>();
            await _unitWork.RemoveListCacheAsync<RoleMenu>();
        }

        public override IEnumerable<MenuDto> GetAllList(AllQueryCommonInput input)
        {
            var express = input.Filter.GetCloudDynamicExpress<Menu>();
            return  _listCacheMenuRepository.Query(express).OrderBy(x => x.SortNo).ToList().MapToIEnumerable<Menu, MenuDto>();
        }

        /// <summary>
        /// 菜单
        /// </summary>
        public async Task<IEnumerable<MenuDto>> GetUserMenusAsync()
        {
            var _userRoleIds = new List<long>();
            var _user = await _authenticationPrincipalService.GetAuthenticatedUserAsync();
            if (_user != default)
            {
                _userRoleIds = _listCacheUserRoleRepository.Query(x => x.UserId == _user.Id).Select(u => u.RoleId).ToList();
            }
            var menuRoleList = _listCacheRoleMenuRepository.Query(x => _userRoleIds.Contains(x.RoleId));
            var menuIds = menuRoleList.Select(x => x.MenuId).ToList();
            var menuList = _listCacheMenuRepository.Query(u => menuIds.Contains(u.Id)).OrderBy(u => u.SortNo).ToList().MapToIEnumerable<Menu, MenuDto>(); ;
            return menuList;
        }
    }
}
