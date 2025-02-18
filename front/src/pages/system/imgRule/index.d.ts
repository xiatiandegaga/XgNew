import { PrimaryTableCol, TableRowData, } from 'tdesign-vue-next';
const COLUMNS: PrimaryTableCol<TableRowData>[] = [
 {
  title: '编号',
  fixed: 'left',
  width: 160,
  ellipsis: true,
  align: 'left',
  colKey: 'imgRuleCode',
 },
 {
  title: '名称',
  width: 160,
  ellipsis: true,
  colKey: 'imgRuleName',
 },
 {
  title: '说明',
  width: 160,
  ellipsis: true,
  colKey: 'remark',
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
 {
  colKey: 'row-select',
  fixed: 'left',
  type: 'multiple',
  width: 50,
 },
 {
  title: '主图',
  fixed: 'left',
  width: 120,
  ellipsis: true,
  align: 'left',
  colKey: 'mainImg',
 },
 {
  title: '链接类型',
  fixed: 'left',
  width: 120,
  ellipsis: true,
  colKey: 'linkType',
 },
 {
  title: '链接对象',
  width: 160,
  ellipsis: true,
  colKey: 'linkKey',
 },
 {
  title: '链接地址',
  width: 200,
  ellipsis: true,
  colKey: 'linkAddress',
 },
 {
  title: '开始日期',
  width: 260,
  ellipsis: true,
  colKey: 'startTime',
 },
 {
  title: '结束日期',
  width: 260,
  ellipsis: true,
  colKey: 'endTime',
 },
 {
  title: '链接图片',
  width: 160,
  ellipsis: true,
  colKey: 'detailImage',
 },
 {
  title: '排序码',
  width: 160,
  ellipsis: true,
  colKey: 'sortNo',
 },
 {
  title: '备注',
  width: 160,
  ellipsis: true,
  colKey: 'remark',
 },
];
const PAGINATION ={
 current: 1,
 pageSize: 10,
 total: 0,
 showJumper: true,
};
const SEARCHFORM = {
 imgRuleCode:'',
 imgRuleName: '',
 status: 1
};
export {
 COLUMNS,
 COLUMNS_FORM,
 PAGINATION,
 SEARCHFORM
}