import { request } from '@/utils/request';
import type { DetailResult,DetailModel } from '@/api/model/system/globalDataModel';
import type { Params } from '@/api/model/common';

const Api = {
 GetPageList: '/GlobalDataDetail/GetPageList',
 GetAllList: '/GlobalDataDetail/GetAllList',
 AddOrUpdate: '/GlobalDataDetail/AddOrUpdate',
 FindSingleById: '/GlobalDataDetail/FindSingleById',
 Delete: '/GlobalDataDetail/Delete',
 GetListByCodes:'/GlobalDataDetail/GetListByCodes',
};


export function getPageList(data: Params) {
 return request.post<DetailResult>({
   url: Api.GetPageList,
   data 
 });
}
export function getAllList(data?: Params) {
  return request.post<Array<DetailModel>>({
    url: Api.GetAllList,
    data 
  });
 }

export function addOrUpdate(data: DetailModel) {
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

export function deleteById(id:String) {
 return request.post({
   url: Api.Delete,
   data: { id }
 });
}
export function getListByCodes(codes:String) {
  return request.post({
    url: Api.GetListByCodes,
    data: { codes }
  });
 }