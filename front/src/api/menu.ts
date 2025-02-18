import { request } from '@/utils/request';
import type { CardListResult, ListResult } from '@/api/model/listModel';

const Api = {
  GetPageList: '/menu/GetPageList',
  AddOrUpdate: '/menu/AddOrUpdate',
  FindSingleById: '/menu/FindSingleById',
  LogicDelete: '/menu/LogicDelete',
};

export function getPageList(data) {
  return request.post({
    url: Api.GetPageList,
    data: data 
  });
}

export function addOrUpdate(data) {
  return request.post({
    url: Api.AddOrUpdate,
    data: data 
  });
}


export function findSingleById(data) {
    return request.post({
      url: Api.FindSingleById,
      data: data 
    });
  }

  export function logicDelete(data) {
    return request.post({
      url: Api.LogicDelete,
      data: data 
    });
  }
