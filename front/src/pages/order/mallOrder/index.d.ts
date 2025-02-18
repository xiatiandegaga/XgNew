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
    title: '支付总金额（元）',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'payAmount',
  },
  {
    title: '下单时间',

    width: 200,
    ellipsis: true,
    align: 'left',
    colKey: 'createTime',
  },
  {
    title: '支付时间',

    width: 200,
    ellipsis: true,
    align: 'left',
    colKey: 'paymentTime',
  },
  {
    title: '发货时间',

    width: 200,
    ellipsis: true,
    align: 'left',
    colKey: 'deliveryTime',
  },
  {
    title: '签收时间',

    width: 200,
    ellipsis: true,
    align: 'left',
    colKey: 'receiveTime',
  },
  {
    title: '物流公司',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'logisticsCompanyName',
  },
  {
    title: '物流单号',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'logisticsNo',
  },
  {
    title: '分期',

    width: 120,
    ellipsis: true,
    align: 'left',
    colKey: 'numberOfInstallments',
  },
  {
    title: '订单状态',

    width: 120,
    ellipsis: true,
    align: 'left',
    colKey: 'statusName',
  },
  {
    title: '收获地址',

    width: 200,
    ellipsis: true,
    align: 'left',
    colKey: 'receiveInfo',
  },
  {
    title: '备注',
    width: 220,
    ellipsis: true,
    align: 'left',
    colKey: 'remark',
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
};
export { COLUMNS, PAGINATION, SEARCHFORM };
