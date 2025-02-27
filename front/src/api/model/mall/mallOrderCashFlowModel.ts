export interface ListResult {
  list: Array<ListModel>;
  totalCount: string
}
export interface ListModel {
  mid :  string ,
  bankCardNo :  string ,
  bankInfo :  string ,
  billFunds :  string ,
  billFundsDesc :  string ,
  buyerId :  string ,
  buyerUsername :  string ,
  buyerPayAmount :  string ,
  buyerCashPayAmt :  string ,
  couponAmount :  string ,
  totalAmount :  string ,
  invoiceAmount :  string ,
  merOrderId :  string ,
  payTime :  string ,
  receiptAmount :  string ,
  refId :  string ,
  refundAmount :  string ,
  refundDesc :  string ,
  seqId :  string ,
  settleDate :  string ,
  status :  string ,
  subBuyerId :  string ,
  targetOrderId :  string ,
  targetSys :  string ,
  couponMerchantContribute :  string ,
  couponOtherContribute :  string ,
  activityIds :  string ,
  refundTargetOrderId :  string ,
  refundPayTime :  string ,
  refundSettleDate :  string ,
  orderDesc:  string ,
  createTime:  string ,
  mchntUuid:  string ,
  connectSys:  string ,
  subInst:  string ,
  refundExtOrderId:  string ,
  goodsTradeNo:  string ,
  extOrderId:  string ,
  refundOrderId:  string ,
  refundInvoiceAmount:  string ,
  instalTransInfo:  string ,
  notifyDate: string,
  [propName:string]:any
}