export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 sortNo: number,
 ctgrCode: string,
 ctgrName:string,
 pid: string,
 imageUrl: string,
 [propName:string]:any
}
export interface AttrDetail  {
 id?: string,
 attrValueName : string,
 sortNo : number,
}