import { PrimaryTableCol, TableRowData, } from 'tdesign-vue-next';
const COLUMNS: PrimaryTableCol<TableRowData>[] = [
 {
  title: '用户账号',
  fixed: 'left',
  width: 160,
  ellipsis: true,
  align: 'left',
  colKey: 'account',
 },
 {
    title: '账号名称',
    fixed: 'left',
    width: 160,
    ellipsis: true,
    align: 'left',
    colKey: 'realName',
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
 showJumper: true,
};
const SEARCHFORM = {
 account:'',
};
export {
 COLUMNS,
 PAGINATION,
 SEARCHFORM
}