import { request } from '@/utils/request';
import type { ListModel } from '@/api/model/system/globalDataModel';
import type { Params } from '@/api/model/common';

const Api = {
 GetAllList: '/GlobalData/GetAllList',
 AddOrUpdate: '/GlobalData/AddOrUpdate',
 FindSingleById: '/GlobalData/FindSingleById',
};


export function getAllList(data?: Params) {
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

