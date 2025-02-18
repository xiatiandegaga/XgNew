export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 sortNo: number,
 typeCode: string,
 typeName:string,
 [propName:string]:any
}