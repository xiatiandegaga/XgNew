import { PrimaryTableCol, TableRowData, } from 'tdesign-vue-next';
const COLUMNS: PrimaryTableCol<TableRowData>[] = [
 {
  title: '用户昵称',
  fixed: 'left',
  width: 160,
  ellipsis: true,
  align: 'left',
  colKey: 'nickName',
 },
 {
  title: '用户手机',
  width: 160,
  ellipsis: true,
  colKey: 'mobile',
 },
 {
  title: '状态',
  width: 160,
  ellipsis: true,
  colKey: 'status',
 },
 {
  title: '创建时间',
  width: 160,
  ellipsis: true,
  colKey: 'createTime',
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