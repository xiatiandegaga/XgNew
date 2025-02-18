using Domain.Entity;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Order
{
    /// <summary>
    /// 
    ///</summary>
    public class MallOrderCashFlowMap : BaseEntityConfiguration<MallOrderCashFlow>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallOrderCashFlow> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_order_cash_flow");

            // ===================== 字段配置 =====================

            // --------------- 基础信息 ---------------
            builder.Property(t => t.Mid)
                .HasColumnName("mid")
                .HasMaxLength(200)
                .HasComment("商户号");

            builder.Property(t => t.Tid)
                .HasColumnName("tid")
                .HasMaxLength(200)
                .HasComment("终端号");

            builder.Property(t => t.InstMid)
                .HasColumnName("inst_mid")
                .HasMaxLength(200)
                .HasComment("机构商户号");

            // --------------- 支付信息 ---------------
            builder.Property(t => t.BankCardNo)
                .HasColumnName("bank_card_no")
                .HasMaxLength(200)
                .HasComment("支付银行信息");

            builder.Property(t => t.BankInfo)
                .HasColumnName("bank_info")
                .HasMaxLength(200)
                .HasComment("银行信息");

            // --------------- 金额信息 ---------------
            builder.Property(t => t.BillFunds)
                .HasColumnName("bill_funds")
                .HasMaxLength(200)
                .HasComment("资金渠道");

            builder.Property(t => t.BillFundsDesc)
                .HasColumnName("bill_funds_desc")
                .HasMaxLength(200)
                .HasComment("资金渠道说明");

            builder.Property(t => t.BuyerPayAmount)
                .HasColumnName("buyer_pay_amount")
                .HasMaxLength(200)
                .HasComment("实付金额");

            builder.Property(t => t.BuyerCashPayAmt)
                .HasColumnName("buyer_cash_pay_amt")
                .HasMaxLength(200)
                .HasComment("实付现金金额");

            builder.Property(t => t.CouponAmount)
                .HasColumnName("coupon_amount")
                .HasMaxLength(200)
                .HasComment("优惠金额");

            builder.Property(t => t.TotalAmount)
                .HasColumnName("total_amount")
                .HasMaxLength(200)
                .HasComment("订单金额（单位：分）");

            builder.Property(t => t.InvoiceAmount)
                .HasColumnName("invoice_amount")
                .HasMaxLength(200)
                .HasComment("开票金额");

            // --------------- 订单信息 ---------------
            builder.Property(t => t.MerOrderId)
                .HasColumnName("mer_order_id")
                .HasMaxLength(200)
                .HasComment("商户订单号");

            builder.Property(t => t.PayTime)
                .HasColumnName("pay_time")
                .HasMaxLength(200)
                .HasComment("支付时间");

            builder.Property(t => t.ReceiptAmount)
                .HasColumnName("receipt_amount")
                .HasMaxLength(200)
                .HasComment("实收金额");

            builder.Property(t => t.RefId)
                .HasColumnName("ref_id")
                .HasMaxLength(200)
                .HasComment("支付银行卡参考号");

            // --------------- 退款信息 ---------------
            builder.Property(t => t.RefundAmount)
                .HasColumnName("refund_amount")
                .HasMaxLength(200)
                .HasComment("退款金额");

            builder.Property(t => t.RefundDesc)
                .HasColumnName("refund_desc")
                .HasMaxLength(200)
                .HasComment("退款说明");

            builder.Property(t => t.SeqId)
                .HasColumnName("seq_id")
                .HasMaxLength(200)
                .HasComment("系统交易流水号");

            // --------------- 时间信息 ---------------
            builder.Property(t => t.SettleDate)
                .HasColumnName("settle_date")
                .HasMaxLength(200)
                .HasComment("结算日期");

            builder.Property(t => t.CreateTime)
                .HasColumnName("create_time")
                .HasMaxLength(200)
                .HasComment("订单创建时间");

            // --------------- 状态信息 ---------------
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasMaxLength(200)
                .HasComment("订单状态(TRADE_SUCCESS 支付成功  TRADE_REFUND退款成功)");

            // --------------- 关联信息 ---------------
            builder.Property(t => t.SubBuyerId)
                .HasColumnName("sub_buyer_id")
                .HasMaxLength(200)
                .HasComment("卖家子ID");

            builder.Property(t => t.TargetOrderId)
                .HasColumnName("target_order_id")
                .HasMaxLength(200)
                .HasComment("渠道订单号");

            builder.Property(t => t.TargetSys)
                .HasColumnName("target_sys")
                .HasMaxLength(200)
                .HasComment("支付渠道");

            // --------------- 优惠信息 ---------------
            builder.Property(t => t.CouponMerchantContribute)
                .HasColumnName("coupon_merchant_contribute")
                .HasMaxLength(200)
                .HasComment("商户出资优惠金额");

            builder.Property(t => t.CouponOtherContribute)
                .HasColumnName("coupon_other_contribute")
                .HasMaxLength(200)
                .HasComment("其他出资优惠金额");

            // --------------- 扩展信息 ---------------
            builder.Property(t => t.ActivityIds)
                .HasColumnName("activity_ids")
                .HasMaxLength(200)
                .HasComment("微信活动Id");

            builder.Property(t => t.RefundTargetOrderId)
                .HasColumnName("refund_target_order_id")
                .HasMaxLength(200)
                .HasComment("退货渠道订单号");

            builder.Property(t => t.RefundPayTime)
                .HasColumnName("refund_pay_time")
                .HasMaxLength(200)
                .HasComment("退货时间");

            builder.Property(t => t.RefundSettleDate)
                .HasColumnName("refund_settle_date")
                .HasMaxLength(200)
                .HasComment("退货结算日期");

            builder.Property(t => t.OrderDesc)
                .HasColumnName("order_desc")
                .HasMaxLength(200)
                .HasComment("订单详情");

            // --------------- 商户信息 ---------------
            builder.Property(t => t.MchntUuid)
                .HasColumnName("mchnt_uuid")
                .HasMaxLength(200)
                .HasComment("商户UUID");

            builder.Property(t => t.ConnectSys)
                .HasColumnName("connect_sys")
                .HasMaxLength(200)
                .HasComment("转接系统");

            builder.Property(t => t.SubInst)
                .HasColumnName("sub_inst")
                .HasMaxLength(200)
                .HasComment("商户所属分支机构代码");

            // --------------- 退货信息 ---------------
            builder.Property(t => t.RefundExtOrderId)
                .HasColumnName("refund_ext_order_id")
                .HasMaxLength(200)
                .HasComment("退货外部订单号");

            builder.Property(t => t.GoodsTradeNo)
                .HasColumnName("goods_trade_no")
                .HasMaxLength(200)
                .HasComment("商品交易单号");

            builder.Property(t => t.ExtOrderId)
                .HasColumnName("ext_order_id")
                .HasMaxLength(200)
                .HasComment("外部订单号");

            builder.Property(t => t.RefundOrderId)
                .HasColumnName("refund_order_id")
                .HasMaxLength(200)
                .HasComment("退货订单号");

            // --------------- 财务信息 ---------------
            builder.Property(t => t.RefundInvoiceAmount)
                .HasColumnName("refund_invoice_amount")
                .HasMaxLength(200)
                .HasComment("实退现金金额");

            // --------------- 分期信息 ---------------
            builder.Property(t => t.InstalTransInfo)
                .HasColumnName("instal_trans_info")
                .HasMaxLength(200)
                .HasComment("分期付款信息域");

            // --------------- 通知信息 ---------------
            builder.Property(t => t.NotifyDate)
                .HasColumnName("notify_date")
                .HasComment("实际通知回调时间");
        }
    }
}
