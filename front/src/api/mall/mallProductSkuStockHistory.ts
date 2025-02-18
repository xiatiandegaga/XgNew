import { request } from '@/utils/request';
import type { ListResult } from '@/api/model/mall/mallProductSkuStockHistoryModel';
import type { Params } from '@/api/model/common';

const Api = {
  GetPageList: '/MallProductSkuStockHistory/GetPageList',
};

export function getPageList(data: Params) {
  return request.post<ListResult>({
    url: Api.GetPageList,
    data,
  });
}
