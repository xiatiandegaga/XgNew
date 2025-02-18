import type { mallOrderDetailModel} from '@/api/model/mall/mallOrderDetailModel';

export interface ListResult {
    list: Array<ListModel>;
    totalCount: string
   }
   export interface ListModel {
    orderNo :  string ,
    status: number,
    totalAmount: number,
    payAmount: number,
    adminDiscountAmount:number,
    logisticsCompany:string,
    logisticsNo:string,
    outTradeNo:string,
    remark:string,
    paymentTime:string,
    createTime:string,
    deliveryTime:string,
    receiveTime:string,
    commentTime:string,
    numberOfInstallments:number,
    userName:string,
    userMobile:string,
    statusName:string,
    logisticsCompanyName:string,
    receiveInfo:string,
    mallOrderDetails:mallOrderDetailModel[]
    [propName:string]:any
   }