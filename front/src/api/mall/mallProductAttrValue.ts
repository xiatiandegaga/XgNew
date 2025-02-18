import { request } from '@/utils/request';
import type { ListResult, ListModel } from '@/api/model/mall/mallProductAttrValueModel';
import type { Params } from '@/api/model/common';

const Api = {
  LogicDelete: '/MallProductAttrValue/LogicDelete',
  GetList:'/MallProductAttrValue/GetList',
};



export function logicDelete(id: String) {
  return request.post({
    url: Api.LogicDelete,
    data: { id },
  });
}
export function getList() {
  return request.get({
    url: Api.GetList,
  });
}
