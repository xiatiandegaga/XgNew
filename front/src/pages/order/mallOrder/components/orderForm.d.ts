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
    title: '产品名称',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productName',
  },
  {
    title: '产品属性',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productSkuAttrs',
  },
  {
    title: '购买数量',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productQuantity',
  },
  {
    title: '单价（元）',
    fixed: 'left',
    width: 120,
    ellipsis: true,
    align: 'left',
    colKey: 'productPriceAmount',
  },
  
  {
    title: '单据状态',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'statusName',
  },
//   {
//     title: '图片',
//     fixed: 'left',
//     width: 160,
//     ellipsis: true,
//     align: 'left',
//     colKey: 'productImgs',
//   },
 
];
const PAGINATION = {
  current: 1,
  pageSize: 10,
  total: 0,
};

export { COLUMNS, PAGINATION };
