import { request } from '@/utils/request';
import type { ListResult, ListModel } from '@/api/model/mall/mallProductTypeModel';
import type { Params } from '@/api/model/common';

const Api = {
  GetPageList: '/MallProductType/GetPageList',
  GetAllList: '/MallProductType/GetAllList',
  AddOrUpdate: '/MallProductType/AddOrUpdate',
  FindSingleById: '/MallProductType/FindSingleById',
  LogicDelete: '/MallProductType/LogicDelete',
};

export function getTypePageList(data: Params) {
  return request.post<ListResult>({
    url: Api.GetPageList,
    data,
  });
}
export function getTypeAllList(data?: Params) {
  return request.post<Array<ListModel>>({
    url: Api.GetAllList,
    data,
  });
}
export function addOrUpdate(data: ListModel) {
  return request.post({
    url: Api.AddOrUpdate,
    data,
  });
}

export function findSingleById(id: String) {
  return request.post({
    url: Api.FindSingleById,
    data: { id },
  });
}

export function logicDelete(id: String) {
  return request.post({
    url: Api.LogicDelete,
    data: { id },
  });
}
