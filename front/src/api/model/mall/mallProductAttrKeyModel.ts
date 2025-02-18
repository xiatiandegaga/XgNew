export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 sortNo: number,
 typeId: string,
 attrKeyName:string,
 [propName:string]:any
}