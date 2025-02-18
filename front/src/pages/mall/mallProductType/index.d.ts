import { PrimaryTableCol, TableRowData } from 'tdesign-vue-next';

const COLUMNS: PrimaryTableCol<TableRowData>[] = [
  {
    title: '类型名称',
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
const PAGINATION = {
  current: 1,
  pageSize: 10,
  total: 0,
};
const SEARCHFORM = {
  typeName: '',
};
export { COLUMNS, PAGINATION, SEARCHFORM };
