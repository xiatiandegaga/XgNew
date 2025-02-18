export interface ListResult {
 list: Array<ListModel>;
 totalCount: string
}
export interface ListModel {
 productName : string ,
 sortNo : number,
 productCode :  string ,
 productShortName : string ,
 brandId : string,
 ctgrId : string,
 productMainImg :  string ,
 productDetailImg :  string ,
 recommendStatus : number,
 unitName :  string ,
 minPrice : number,
 maxPrice : number,
 publishStatus : number,
 newStatus : number,
 remark :  string 
 [propName:string]:any
}
export interface SkuListModel {
 skuName: string ,
 sortNo: number,
 skuCode: string ,
 productId: string,
 skuPrice: number,
 skuImg: string ,
 skuStock: number,
 freezeStock: number,
 attrKeyValue:  string ,
 [propName:string]:any
}