import { PrimaryTableCol, TableRowData } from 'tdesign-vue-next';

const COLUMNS: PrimaryTableCol<TableRowData>[] = [
  {
    title: '属性名',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'attrKeyName',
  },
  {
    title: '商品类型',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'typeName',
  },
  {
    title: '商品目录',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'categoryName',
  },
  {
    align: 'left',
    fixed: 'right',
    width: 160,
    colKey: 'op',
    title: '操作',
  },
];
const COLUMNS_FORM: PrimaryTableCol<TableRowData>[] = [
  // {
  //   colKey: 'row-select',
  //   fixed: 'left',
  //   type: 'multiple',
  //   width: 50,
  // },
  {
    title: '属性名',
    width: 160,
    ellipsis: true,
    colKey: 'attrValueName',
  },
  {
    title: '排序码',
    width: 160,
    ellipsis: true,
    colKey: 'sortNo',
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
  attrKeyName: '',
  mallProductCategoryId: '',
};
export { COLUMNS, COLUMNS_FORM, PAGINATION, SEARCHFORM };
