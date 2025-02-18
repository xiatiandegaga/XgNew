import { request } from '@/utils/request';
import type { ListResult,ListModel } from '@/api/model/system/userModel';
import type { Params } from '@/api/model/common';

const Api = {
 GetPageList: '/user/GetPageList',
 update: '/user/update',
 FindSingleById: '/user/FindSingleById',
 LogicDelete: '/user/LogicDelete',
 GetRemoteSerch:'/user/GetRemoteSerch',
};


export function getPageList(data: Params) {
 return request.post<ListResult>({
   url: Api.GetPageList,
   data 
 });
}

export function update(data: ListModel) {
 return request.post({
   url: Api.update,
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

export function getRemoteSerch(key:String) {
  return request.get({
    url: Api.GetRemoteSerch,
    params: { key }
  });
 }