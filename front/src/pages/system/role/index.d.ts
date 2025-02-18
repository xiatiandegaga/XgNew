import { PrimaryTableCol, TableRowData, } from 'tdesign-vue-next';
const COLUMNS: PrimaryTableCol<TableRowData>[] = [
 {
  title: '角色名称',
  fixed: 'left',
  width: 100,
  ellipsis: true,
  align: 'left',
  colKey: 'name',
 },
 {
  title: '角色描述',
  width: 160,
  ellipsis: true,
  colKey: 'description',
 },
 {
  title: '级别',
  width: 160,
  colKey: 'level',
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
 name: '',
 status: 1
};
export {
 COLUMNS,
 PAGINATION,
 SEARCHFORM
}