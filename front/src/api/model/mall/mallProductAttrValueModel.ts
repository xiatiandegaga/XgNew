export interface ListResult {
    list: Array<ListModel>;
    totalCount: string
   }
   export interface ListModel {
    sortNo: number,
    mallProductAttrKeyId: string,
    attrValueName:string,
    status:number,
    id:string
    [propName:string]:any
   }