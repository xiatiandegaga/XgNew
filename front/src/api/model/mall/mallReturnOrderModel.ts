export interface ListResult {
    list: Array<ListModel>;
    totalCount: string
   }
   export interface ListModel {
    orderNo :  string ,
    refOrderNo: string,
    createDate: string,
    status: number,
    detailStatus:number,
    productName:string,
    productQuantity:number,
    productSkuAttrs:string,
    mallOrderDetailId:string,
    checkDate:string,
    responseResult:string,
    remark:string,
    userName:string,
    userMobile:string,
    checkUserName:string,
    productAmount:number,
    totalProductAmount:number,
    detailStatusName:string,
    [propName:string]:any
   }