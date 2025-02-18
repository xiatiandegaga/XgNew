import { PrimaryTableCol, TableRowData } from 'tdesign-vue-next';

const COLUMNS: PrimaryTableCol<TableRowData>[] = [
  {
    title: '商品名称',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productName',
  },
  {
    title: '商品简称',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'productShortName',
  },
  {
    title: '商品类型',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'mallProductTypeId',
  },
  {
    title: '商品目录',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'mallProductCategoryId',
  },
  
  {
    title: '商品状态',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'status',
  },
  {
    align: 'left',
    fixed: 'right',
    width: 180,
    colKey: 'op',
    title: '操作',
  },
];

const COLUMNS_FORM: PrimaryTableCol<TableRowData>[] = [
  {
    colKey: 'row-select',
    fixed: 'left',
    type: 'multiple',
    width: 20,
  },
  {
    title: '属性名',
    width: 120,
    ellipsis: true,
    fixed: 'left',
    colKey: 'attrKeyValue',
  },
  {
    title: '分期数',
    width: 120,
    ellipsis: true,
    colKey: 'numberOfInstallments',
  },
  {
    title: '参数',
    ellipsis: true,
    colKey: 'specParam',
  },
  {
    title: '价格',
    width: 100,
    ellipsis: true,
    colKey: 'skuPriceAmount',
  },
  {
    title: '状态',
    width: 100,
    ellipsis: true,
    colKey: 'status',
  },
  {
    title: '库存',
    width: 100,
    ellipsis: true,
    colKey: 'skuStock',
  },
  {
    title: '冻结库存',
    width: 100,
    ellipsis: true,
    colKey: 'freezeStock',
  },
 
  {
    title: '排序码',
    width: 100,
    ellipsis: true,
    colKey: 'sortNo',
  },
  {
    align: 'left',
    fixed: 'right',
    width: 100,
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
  productName: '',
  status:null
};
export { COLUMNS, COLUMNS_FORM, PAGINATION, SEARCHFORM };
