import { request } from '@/utils/request';
import type { ListResult, ListModel } from '@/api/model/mall/mallProductTypeModel';
import type { Params } from '@/api/model/common';

const Api = {
  GetPageList: '/MallProduct/GetPageList',
  GetAllList: '/MallProduct/GetAllList',
  AddOrUpdate: '/MallProduct/AddOrUpdate',
  FindSingleById: '/MallProduct/FindSingleById',
  FindSingleDetailById: '/MallProduct/FindSingleDetailsById',
  LogicDelete: '/MallProduct/LogicDelete',
  TakeOn: '/MallProduct/TakeOn',
  TakeOff: '/MallProduct/TakeOff',
  StockInv:'/MallProduct/StockInv',
  StockRel:'/MallProduct/StockRel',
};

export function getPageList(data: Params) {
  return request.post<ListResult>({
    url: Api.GetPageList,
    data,
  });
}
export function getAllList() {
  return request.get({
    url: Api.GetAllList,
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

export function findSingleDetailById(id: String) {
  return request.post({
    url: Api.FindSingleDetailById,
    data: { id },
  });
}

export function logicDelete(id: String) {
  return request.post({
    url: Api.LogicDelete,
    data: { id },
  });
}

export function takeOn(id: String) {
  return request.post({
    url: Api.TakeOn,
    data: { id },
  });
}

export function takeOff(id: String) {
  return request.post({
    url: Api.TakeOff,
    data: { id },
  });
}
export function stockInv(data: ListModel) {
  return request.post({
    url: Api.StockInv,
    data,
  });
}
export function stockRel(data: ListModel) {
  return request.post({
    url: Api.StockRel,
    data,
  });
}
