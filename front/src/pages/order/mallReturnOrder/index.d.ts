import { status } from 'nprogress';
import { PrimaryTableCol, TableRowData } from 'tdesign-vue-next';

const COLUMNS: PrimaryTableCol<TableRowData>[] = [
  {
    title: '订单号',
    fixed: 'left',
    width: 220,
    ellipsis: true,
    align: 'left',
    colKey: 'orderNo',
  },
  {
    title: '关联订单号',
    width: 220,
    ellipsis: true,
    align: 'left',
    colKey: 'refMallOrderNo',
  },
  {
    title: '用户',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'userName',
  },
  {
    title: '用户手机',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'userMobile',
  },
  {
    title: '产品名称',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productName',
  },
  {
    title: '产品属性',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productSkuAttrs',
  },
  {
    title: '产品数量',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productQuantity',
  },
  {
    title: '单价金额（元）',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productAmount',
  },
  {
    title: '总金额（元）',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'totalProductAmount',
  },
  {
    title: '订单状态',

    width: 120,
    ellipsis: true,
    align: 'left',
    colKey: 'status',
  },
  {
    title: '详细状态',
    width: 120,
    ellipsis: true,
    align: 'left',
    colKey: 'detailStatusName',
  },
  {
    title: '申请时间',
    width: 200,
    ellipsis: true,
    align: 'left',
    colKey: 'createDate',
  },

  {
    align: 'left',
    fixed: 'right',
    width: 160,
    colKey: 'op',
    title: '操作',
  },
];
const PAGINATION = {
  current: 1,
  pageSize: 10,
  total: 0,
};
const SEARCHFORM = {
  orderNo: '',
  userId: null,
  createTime: [],
  status: null,
  detailStatus: null,
};
export { COLUMNS, PAGINATION, SEARCHFORM };
