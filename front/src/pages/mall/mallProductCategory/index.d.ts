import { PrimaryTableCol, TableRowData } from 'tdesign-vue-next';

const COLUMNS: PrimaryTableCol<TableRowData>[] = [
  {
    title: '目录名称',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'categoryName',
  },
  {
    title: '目录编号',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'categoryCode',
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
    align: 'left',
    fixed: 'right',
    width: 160,
    colKey: 'op',
    title: '操作',
  },
];
const SEARCHFORM = {
  categoryName: '',
  mallProductTypeId:0
};
export { COLUMNS, SEARCHFORM };
