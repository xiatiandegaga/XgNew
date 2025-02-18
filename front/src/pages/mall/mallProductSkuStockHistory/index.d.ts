import { status } from 'nprogress';
import { PrimaryTableCol, TableRowData } from 'tdesign-vue-next';

const COLUMNS: PrimaryTableCol<TableRowData>[] = [
  {
    title: '单号',
    fixed: 'left',
    width: 220,
    ellipsis: true,
    align: 'left',
    colKey: 'stockNo',
  },
  {
    title: '出入库类型',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'stockType',
  },
  {
    title: '产品名称',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'mallProductName',
  },
  {
    title: '产品属性',

    width: 220,
    ellipsis: true,
    align: 'left',
    colKey: 'goodsAttrs',
  },
  {
    title: '数量',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'num',
  },
  {
    title: '日期',

    width: 200,
    ellipsis: true,
    align: 'left',
    colKey: 'createDate',
  },
  {
    title: '细分类型',

    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'stockDetailTypeName',
  }
];
const PAGINATION = {
  current: 1,
  pageSize: 10,
  total: 0,
};
const SEARCHFORM = {
  stockNo: '',
  productId: null,
  createDate: [],
  stockType: null,
  stockDetailType: null,
};
export { COLUMNS, PAGINATION, SEARCHFORM };
