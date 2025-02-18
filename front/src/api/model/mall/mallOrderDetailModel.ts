export interface mallOrderDetailModel {
    orderNo :  string ,
    productSkuId:string,
    productName:string,
    productQuantity: number,
    status: number,
    productSkuAttrs:string,
    productImgs:string,
    mallOrderId:string,
    statusName:string,
    productPriceAmount:number
    [propName:string]:any
   }