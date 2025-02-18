export interface ListResult {
    list: Array<mallProductSkuStockHistoryModel>;
    totalCount: string
   }
   export interface mallProductSkuStockHistoryModel {
    stockNo: string,
    createDate: string,
    stockType:number,
    mallProductName:string,
    goodsAttrs:string,
    num:string,
    stockDetailTypeName:string,
    remark:string,
    [propName:string]:any
   }