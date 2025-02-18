import { request } from '@/utils/request';
import type { ListResult, ListModel } from '@/api/model/mall/mallOrderCashFlowModel';
import type { Params } from '@/api/model/common';

const Api = {
  GetPageList: '/MallOrderCashFlow/GetPageList',
  FindSingleById: '/MallOrderCashFlow/FindSingleById',
};

export function getOrderCashFlowPageList(data: Params) {
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

