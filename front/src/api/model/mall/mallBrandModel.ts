export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 brandName :  string ,
 sortNo: number,
 brandCode: string,
 brandImg: string,
 brandLogo:string,
 [propName:string]:any
}