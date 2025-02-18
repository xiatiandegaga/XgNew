using Cloud.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class MallOrderCashFlow : BaseEntity<long>
    {
        /// <summary>
        /// 商户号
        /// </summary>
        [MaxLength(200)]
        public string Mid { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        [MaxLength(200)]
        public string Tid { get; set; }

        /// <summary>
        /// 机构商户机构商户号
        /// </summary>
        [MaxLength(200)]
        public string InstMid { get; set; }

        /// <summary>
        /// 支付银行支付银行信息
        /// </summary>
        [MaxLength(200)]
        public string BankCardNo { get; set; }

        /// <summary>
        /// 银行信息
        /// </summary>
        [MaxLength(200)]
        public string BankInfo { get; set; }

        /// <summary>
        /// 资金渠道
        /// </summary>
        [MaxLength(200)]
        public string BillFunds { get; set; }

        /// <summary>
        /// 资金渠道说明
        /// </summary>
        [MaxLength(200)]
        public string BillFundsDesc { get; set; }

        /// <summary>
        /// 卖家ID
        /// </summary>
        [MaxLength(200)]
        public string BuyerId { get; set; }

        /// <summary>
        /// 买家用户名
        /// </summary>
        [MaxLength(200)]
        public string BuyerUsername { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        [MaxLength(200)]
        public string BuyerPayAmount { get; set; }

        /// <summary>
        /// 实付现金金额
        /// </summary>
        [MaxLength(200)]
        public string BuyerCashPayAmt { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        [MaxLength(200)]
        public string CouponAmount { get; set; }

        /// <summary>
        /// 订单金额（单位：分）
        /// </summary>
        [MaxLength(200)]
        public string TotalAmount { get; set; }

        /// <summary>
        /// 开票金额
        /// </summary>
        [MaxLength(200)]
        public string InvoiceAmount { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        [MaxLength(200)]
        public string MerOrderId { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [MaxLength(200)]
        public string PayTime { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        [MaxLength(200)]
        public string ReceiptAmount { get; set; }

        /// <summary>
        /// 支付银行卡参考号
        /// </summary>
        [MaxLength(200)]
        public string RefId { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [MaxLength(200)]
        public string RefundAmount { get; set; }

        /// <summary>
        /// 退款说明
        /// </summary>
        [MaxLength(200)]
        public string RefundDesc { get; set; }

        /// <summary>
        /// 系统交易流水号
        /// </summary>
        [MaxLength(200)]
        public string SeqId { get; set; }

        /// <summary>
        /// 结算日期
        /// </summary>
        [MaxLength(200)]
        public string SettleDate { get; set; }

        /// <summary>
        /// 订单状态(TRADE_SUCCESS 支付成功  TRADE_REFUND退款成功) 
        /// </summary>
        [MaxLength(200)]
        public string Status { get; set; }

        /// <summary>
        /// 卖家子ID
        /// </summary>
        [MaxLength(200)]
        public string SubBuyerId { get; set; }

        /// <summary>
        /// 渠道订单号
        /// </summary>
        [MaxLength(200)]
        public string TargetOrderId { get; set; }

        /// <summary>
        /// 支付渠道
        /// </summary>
        [MaxLength(200)]
        public string TargetSys { get; set; }

        /// <summary>
        /// 商户出资优惠金额
        /// </summary>
        [MaxLength(200)]
        public string CouponMerchantContribute { get; set; }

        /// <summary>
        /// 其他出资优惠金额
        /// </summary>
        [MaxLength(200)]
        public string CouponOtherContribute { get; set; }
        /// <summary>
        /// 微信活动Id
        /// </summary>
        [MaxLength(200)]
        public string ActivityIds { get; set; }

        /// <summary>
        /// 退货渠道订单号
        /// </summary>
        [MaxLength(200)]
        public string RefundTargetOrderId { get; set; }

        /// <summary>
        /// 退货时间
        /// </summary>
        [MaxLength(200)]
        public string RefundPayTime { get; set; }

        /// <summary>
        /// 结算日期
        /// </summary>
        [MaxLength(200)]
        public string RefundSettleDate { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        [MaxLength(200)]
        public string OrderDesc { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        [MaxLength(200)]
        public string CreateTime { get; set; }

        /// <summary>
        /// 商户UUID
        /// </summary>
        [MaxLength(200)]
        public string MchntUuid { get; set; }

        /// <summary>
        /// 转接系统
        /// </summary>
        [MaxLength(200)]
        public string ConnectSys { get; set; }

        /// <summary>
        /// 商户所属分支机构代码
        /// </summary>
        [MaxLength(200)]
        public string SubInst { get; set; }

        /// <summary>
        /// 退货外部订单号
        /// </summary>
        [MaxLength(200)]
        public string RefundExtOrderId { get; set; }

        /// <summary>
        /// 商品交易单号
        /// </summary>
        [MaxLength(200)]
        public string GoodsTradeNo { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        [MaxLength(200)]
        public string ExtOrderId { get; set; }

        /// <summary>
        /// 退货订单号
        /// </summary>
        [MaxLength(200)]
        public string RefundOrderId { get; set; }

        /// <summary>
        /// 实退现金金额
        /// </summary>
        [MaxLength(200)]
        public string RefundInvoiceAmount { get; set; }

        /// <summary>
        /// 分期付款信息域
        /// </summary>
        [MaxLength(200)]
        public string InstalTransInfo { get; set; }

        /// <summary>
        /// 实际通知回调时间
        /// </summary>
        [MaxLength(200)]
        public DateTime NotifyDate { get; set; }

    }
}
