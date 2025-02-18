export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 imgRuleCode :  string ,
 imgRuleName: string,
 remark: number,
 status : number,
 [propName:string]:any
 imgRuleDetailDtos: Array<ImgRuleDetail>
}

export interface ImgRuleDetail  {
  id?: string;
  remark : string,
  sortNo : number,
  status : number,
  mainImg: string,
  detailImage?: string,
  linkType: string,
  linkKey?: string,
  linkAddress?: string,
  startTime: Date,
  endTime: Date,
}
