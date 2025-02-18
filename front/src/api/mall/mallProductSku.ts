import { request } from '@/utils/request';
import type { SkuListModel } from '@/api/model/mall/mallProductModel';

const Api = {
 AddOrUpdate: '/MallProductSku/AddOrUpdate',
 FindSingleById: '/MallProductSku/FindSingleDetailsById',
};


export function addOrUpdate(data: Array<SkuListModel>) {
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


