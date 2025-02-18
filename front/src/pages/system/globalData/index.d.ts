import { PrimaryTableCol, TableRowData, } from 'tdesign-vue-next';
const COLUMNS: PrimaryTableCol<TableRowData>[] = [
 {
  title: '编码',
  fixed: 'left',
  width: 160,
  ellipsis: true,
  align: 'left',
  colKey: 'code',
 },
 {
  title: 'key值',
  width: 160,
  ellipsis: true,
  colKey: 'constKey',
 },
 {
  title: '名称',
  width: 160,
  ellipsis: true,
  colKey: 'name',
 },
 {
  title: '备注',
  width: 160,
  ellipsis: true,
  colKey: 'remark',
 },
 {
  title: '状态',
  width: 160,
  ellipsis: true,
  colKey: 'status',
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
 realName:'',
 nickName: '',
 mobile: '',
 status: 1
};
export {
 COLUMNS,
 PAGINATION,
 SEARCHFORM
}