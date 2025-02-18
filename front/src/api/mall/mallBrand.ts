import { request } from '@/utils/request';
import type { ListResult,ListModel } from '@/api/model/mall/mallBrandModel';
import type { Params } from '@/api/model/common';

const Api = {
 GetPageList: '/MallBrand/GetPageList',
 GetBrandAllList: '/MallBrand/GetAllList',
 AddOrUpdate: '/MallBrand/AddOrUpdate',
 FindSingleById: '/MallBrand/FindSingleById',
 LogicDelete: '/MallBrand/LogicDelete',
};


export function getPageList(data: Params) {
 return request.post<ListResult>({
   url: Api.GetPageList,
   data 
 });
}

export function getBrandAllList(data?: Params) {
  return request.post<Array<ListModel>>({
    url: Api.GetBrandAllList,
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


