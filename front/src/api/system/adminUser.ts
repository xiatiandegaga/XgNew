import { request } from '@/utils/request';
import type { ListResult,ListModel } from '@/api/model/system/adminUserModel';
import type { Params } from '@/api/model/common';

const Api = {
 GetPageList: '/AdminUser/GetPageList',
 AddOrUpdate: '/AdminUser/AddOrUpdate',
 FindSingleById: '/AdminUser/FindSingleById',
 LogicDelete: '/AdminUser/LogicDelete',
 RestPassword: '/AdminUser/ResetPassword'
};


export function getPageList(data: Params) {
 return request.post<ListResult>({
   url: Api.GetPageList,
   data 
 });
}

export function addOrUpdate(data: ListModel) {
 return request.post({
   url: Api.AddOrUpdate,
   data 
 });
}

export function findSingleById(id:String) {
 return request.post({
   url: Api.FindSingleById,
   data: { id }
 });
}

export function logicDelete(id:String) {
 return request.post({
   url: Api.LogicDelete,
   data: { id }
 });
}

export function restPassword(id:String) {
  return request.post({
    url: Api.RestPassword,
    data: { id }
  });
 }

