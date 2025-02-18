namespace Xg.Cloud.BankMallH5.Cloud.Model
{
    public class OrderReturnModel: BaseReturnModel
    {

        /// <summary>
        /// 消息ID
        /// </summary>
        public string? msgId { get; set; }

        /// <summary>
        /// 请求系统预留字段
        /// </summary>
        public string? srcReserve { get; set; }

        /// <summary>
        /// 报文响应时间 格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string? responseTimestamp { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string? mid { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        public string? tid { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? merOrderId { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string? merName { get; set; }

        /// <summary>
        /// 平台流水号
        /// </summary>
        public string? seqId { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string? status { get; set; }

        /// <summary>
        /// 支付渠道商户号
        /// </summary>
        public string? targetMid { get; set; }

        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string? targetOrderId { get; set; }

        /// <summary>
        /// 目标平台的状态
        /// </summary>
        public string? targetStatus { get; set; }

        /// <summary>
        /// 目标平台代码
        /// </summary>
        public string? targetSys { get; set; }

        /// <summary>
        /// 支付总金额
        /// </summary>
        public string? totalAmount { get; set; }

        /// <summary>
        /// 总退款金额
        /// </summary>
        public string? refundAmount { get; set; }

        /// <summary>
        /// 退款渠道列表
        /// </summary>
        public string? refundFunds { get; set; }

        /// <summary>
        /// 退款渠道描述
        /// </summary>
        public string? refundFundsDesc { get; set; }

        /// <summary>
        /// 实付部分退款金额
        /// </summary>
        public string? refundInvoiceAmount { get; set; }

        /// <summary>
        /// 退货订单号
        /// </summary>
        public string? refundOrderId { get; set; }

        /// <summary>
        /// 目标系统退货订单号
        /// </summary>
        public string? refundTargetOrderId { get; set; }

        /// <summary>
        /// 商户出资优惠金额
        /// </summary>
        public string? refundMerchantContribute { get; set; }

        /// <summary>
        /// 其他出资优惠金额
        /// </summary>
        public string? refundOtherContribute { get; set; }

        /// <summary>
        /// 优惠退货金额（合计）
        /// </summary>
        public string? totalRefundPromotionAmt { get; set; }
  
        /// <summary>
        /// 优惠状态  0：订单无优惠 1：订单有优惠但未找到 2：订单有优惠且信息完整
        /// </summary>
        public string? orderPromotionStatus { get; set; }

        /// <summary>
        /// 活动列表数组-JSON  单品营销优惠活动列表
        /// </summary>
        public string? eventList { get; set; }

        /// <summary>
        /// 渠道方  最大长度50位 ACP：银联  UMS：银联商务  ALIPAY：支付宝   WXPAY：微信
        /// </summary>
        public string? promotionProviderCode { get; set; }

        /// <summary>
        /// 流水ID 最大长度50位  ACP：couponInfo[0].id   UMS：无   ALIPAY：无  WXPAY：promotion_id
        /// </summary>
        public string? flowId { get; set; }

        /// <summary>
        /// 活动  编号  最大长度50位 ACP： couponInfo[0].id  UMS：无  ALIPAY： ALI_EVENT_NO  WXPAY：无
        /// </summary>
        public string? eventNo { get; set; }

        /// <summary>
        /// 活动名称  最大长度64位   ACP：couponInfo[0].desc UMS：无 ALIPAY：无 WXPAY：无
        /// </summary>
        public string? eventName { get; set; }

        /// <summary>
        /// 优惠范围 最大长度32位  promotion_provider= ACP场景： goods_list不存在时GLOBAL，goods_list存在时SINGLE promotion_provider= ALIPAY场景：GLOBAL
        /// </summary>
        public string? promotionRange { get; set; }

        /// <summary>
        /// 优惠类型  最大长度32位  promotion_provider= ACP场景： couponInfo[0].type DD01随机立减   CP01事后赠予券  CP02事前领取券 promotion_provider= ALIPAY场景： DISCOUNT
        /// </summary> 
        public string? promotionType { get; set; }

        /// <summary>
        /// 退货优惠金额  最大长度15位 CP：couponInfo[*].offstAmt累加结果  UMS：无  ALIPAY：fund_channel为COUPON, DISCOUNT, MCOUPON, MDISCOUNT金额累加    WXPAY：refund_amount
        /// </summary>
        public string? refundPromotionAmt { get; set; }
 
        /// <summary>
        /// 平台金额 最大长度15位 ouponInfo[].offstAmt累加结果（对应的couponInfo[].spnsrId应为 00010000）  UMS：无  ALIPAY：fund_channel为COUPON, DISCOUNT金额累加 WXPAY：无
        /// </summary>
        public string? platPromotionAmt { get; set; }

        /// <summary>
        /// 商户优惠金额  最大长度15位 ACP： couponInfo[].offstAmt累加结果（对应的couponInfo[].spnsrId内容长度为15）  UMS：无  ALIPAY：fund_channel为  MCOUPON, MDISCOUNT金额累加 WXPAY：无
        /// </summary>
        public string? mchntPromotionAmt { get; set; }

        /// <summary>
        /// 第三方优惠金额  最大长度15位 ACP：couponInfo[].offstAmt累加结果（对应的couponInfo[].spnsrId内容长度为8，且不为00010000）  UMS：无   ALIPAY：无   WXPAY：无
        /// </summary>
        public string? thirdPartyPromotionAmt { get; set; }

        /// <summary>
        /// 第三方出资详情  base64编码 promotion_provider= ACP场景：couponInfo
        /// </summary>
        public string? thirdPartyPromotionDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? oriInfo { get;set; }
    }
}
