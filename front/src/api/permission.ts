import { request } from '@/utils/request';
import type { RouteItem } from '@/api/model/permissionModel';
import type { Params } from '@/api/model/common';

const Api = {
  AccountLogin: '/login/AccountLogin',
  MenuList: '/menu/GetUserMenus',
  AllList:'/menu/GetAllList'
};

export function accountLogin(userInfo: Record<string, unknown>) {
  return request.post({
    url: Api.AccountLogin,
    data: userInfo
  });
}


export function getMenuList() {
  return request.post({
    url: Api.MenuList,
  });
}

export function getAllList(data?: Params) {
  return request.post({
    url: Api.AllList,
    data
  });
}

