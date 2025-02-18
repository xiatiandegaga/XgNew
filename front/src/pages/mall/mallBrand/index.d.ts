import { PrimaryTableCol, TableRowData, } from 'tdesign-vue-next';
const COLUMNS: PrimaryTableCol<TableRowData>[] = [
 {
  title: '品牌编号',
  fixed: 'left',
  width: 160,
  ellipsis: true,
  align: 'left',
  colKey: 'brandCode',
 },
 {
  title: '品牌名称',
  fixed: 'left',
  width: 160,
  ellipsis: true,
  align: 'left',
  colKey: 'brandName',
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
 brandName:'',
};
export {
 COLUMNS,
 PAGINATION,
 SEARCHFORM
}