import { request } from '@/utils/request';
import type { ListResult, ListModel } from '@/api/model/mall/mallOrderModel';
import type { Params } from '@/api/model/common';

const Api = {
  GetPageList: '/MallOrder/GetPageList',
  FindSingleById: '/MallOrder/FindSingleById',
  SendOut: '/MallOrder/SendOut'
};

export function getOrderPageList(data: Params) {
  return request.post<ListResult>({
    url: Api.GetPageList,
    data,
  });
}

export function findSingleById(id: String) {
  return request.post({
    url: Api.FindSingleById,
    data: { id },
  });
}

export function sendOut(data) {
    return request.post({
      url: Api.SendOut,
      data,
    });
  }
