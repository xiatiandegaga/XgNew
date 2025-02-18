import { request } from '@/utils/request';
import type { ListResult,ListModel } from '@/api/model/system/userModel';
import type { Params } from '@/api/model/common';

const Api = {
 GetPageList: '/Role/GetPageList',
 GetAllList: '/Role/GetAllList',
 AddOrUpdate: '/Role/AddOrUpdate',
 FindSingleById: '/Role/FindSingleById',
 LogicDelete: '/Role/LogicDelete',
};


export function getPageList(data: Params) {
 return request.post<ListResult>({
   url: Api.GetPageList,
   data 
 });
}

export function getAllList() {
  return request.post<Array<ListModel>>({
    url: Api.GetAllList,
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
