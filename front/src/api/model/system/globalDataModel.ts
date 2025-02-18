export interface ListResult {
 data: Array<ListModel>;
}
export interface ListModel {
  code :  string ,
  name :  string ,
  sortNo : number
}

export interface DetailResult {
  list: Array<DetailModel>;
  totalCount: string
 }
 export interface DetailModel {
  code :  string ,
  name :  string ,
  remark :  string ,
  status : number,
  constKey :  string ,
  sortNo : number
 }
 
