import { request } from '@/utils/request';
import type { ListResult, ListModel } from '@/api/model/mall/mallReturnOrderModel';
import type { Params } from '@/api/model/common';

const Api = {
  GetPageList: '/MallOrderReturn/GetPageList',
  FindSingleById: '/MallOrderReturn/FindSingleById',
  CheckOrder:'/MallOrderReturn/CheckOrder',
  OrderReturnInv:'/MallOrderReturn/OrderReturnInv',
  OrderRefund:'/MallOrderReturn/OrderRefund',
  OrderReturned:'/MallOrderReturn/orderDetailReturned '
};

export function getReturnOrderPageList(data: Params) {
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

export function checkOrder(data) {
  return request.post({
    url: Api.CheckOrder,
    data
  });
}

export function orderReturnInv(id) {
  return request.post({
    url: Api.OrderReturnInv,
    data:{id}
  });
}

export function orderRefund(id) {
  return request.post({
    url: Api.OrderRefund,
    data:{id}
  });
}

export function orderReturned(id) {
  return request.post({
    url: Api.OrderReturned,
    data:{id}
  });
}

