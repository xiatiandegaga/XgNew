import { request } from '@/utils/request';
import type { ListResult, ListModel } from '@/api/model/mall/mallProductAttrKeyModel';
import type { Params } from '@/api/model/common';

const Api = {
  GetPageList: '/MallProductAttrKey/GetPageList',
  GetAllList: '/MallProductAttrKey/GetAllList',
  AddOrUpdate: '/MallProductAttrKey/AddOrUpdate',
  FindSingleById: '/MallProductAttrKey/FindSingleDetailsById',
  GetAllDetails: '/MallProductAttrKey/GetAllDetailsListByCategoryId',
  LogicDelete: '/MallProductAttrKey/LogicDelete',
  GetList:'/MallProductAttrKey/GetList',
};

export function getPageList(data: Params) {
  return request.post<ListResult>({
    url: Api.GetPageList,
    data,
  });
}

export function getAttrAllList(data?: Params) {
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

export function getAllDetailsById(id: String) {
  return request.post({
    url: Api.GetAllDetails,
    data: { id },
  });
}

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
