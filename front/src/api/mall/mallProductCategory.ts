import { request } from '@/utils/request';
import type { ListResult,ListModel } from '@/api/model/mall/mallProductCategoryModel';
import type { Params } from '@/api/model/common';

const Api = {
 GetPageList: '/MallProductCategory/GetPageList',
 GetAllList: '/MallProductCategory/GetAllList',
 AddOrUpdate: '/MallProductCategory/AddOrUpdate',
 FindSingleById: '/MallProductCategory/FindSingleById',
 LogicDelete: '/MallProductCategory/LogicDelete',
 GetListByTypeId:'/MallProductCategory/GetListByTypeId'
};


export function getPageList(data: Params) {
 return request.post<ListResult>({
   url: Api.GetPageList,
   data
 });
}

export function getCategoryAllList(data?: Params) {
  return request.post<Array<ListModel>>({
    url: Api.GetAllList,
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
export function getListByTypeId(mallProductTypeId:String) {
  return request.get({
    url: Api.GetListByTypeId,
    params: { mallProductTypeId }
  });
 }

