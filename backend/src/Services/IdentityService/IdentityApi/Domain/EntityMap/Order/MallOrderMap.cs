using Domain.Entity.Order;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Order
{
    /// <summary>
    /// 
    ///</summary>
    public class MallOrderMap : BaseEntityConfiguration<MallOrder>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallOrder> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_order");  // 统一 snake_case 格式

            // ===================== 字段配置 =====================
            // ------------------- 订单基础信息 -------------------
            builder.Property(t => t.OrderNo)
                .HasColumnName("order_no")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("订单编号（唯一业务标识）");

            builder.Property(t => t.UserId)
                .HasColumnName("user_id")
                .IsRequired()
                .HasComment("关联用户ID");

            // ------------------- 金额信息 -------------------
            builder.Property(t => t.TotalPrice)
                .HasColumnName("total_price")
                .HasComment("订单总金额");

            builder.Property(t => t.PayPrice)
                .HasColumnName("pay_price")
                .HasComment("实际支付金额");

            builder.Property(t => t.AdminDiscountPrice)
                .HasColumnName("admin_discount_price")
                .HasDefaultValue(0m)
                .HasComment("管理员调整金额");

            // ------------------- 支付信息 -------------------
            builder.Property(t => t.PayType)
                .HasColumnName("pay_type")
                .HasMaxLength(100)
                .HasComment("支付方式");

            builder.Property(t => t.OutTradeNo)
                .HasColumnName("out_trade_no")
                .HasMaxLength(100)
                .HasComment("第三方支付交易号");

            builder.Property(t => t.ThirdOrderNo)
                .HasColumnName("third_order_no")
                .HasMaxLength(100)
                .HasComment("第三方系统订单号");

            // ------------------- 物流信息 -------------------
            builder.Property(t => t.LogisticsCompany)
                .HasColumnName("logistics_company")
                .HasMaxLength(100)
                .HasComment("物流公司");

            builder.Property(t => t.LogisticsNo)
                .HasColumnName("logistics_no")
                .HasMaxLength(100)
                .HasComment("物流单号");

            // ------------------- 时间信息 -------------------
            builder.Property(t => t.PaymentTime)
                .HasColumnName("payment_time")
                .HasComment("支付时间");

            builder.Property(t => t.DeliveryTime)
                .HasColumnName("delivery_time")
                .HasComment("发货时间");

            builder.Property(t => t.ReceiveTime)
                .HasColumnName("receive_time")
                .HasComment("收货时间");

            builder.Property(t => t.CommentTime)
                .HasColumnName("comment_time")
                .HasComment("评价时间");

            // ------------------- 其他信息 -------------------
            builder.Property(t => t.ReceiveInfo)
                .HasColumnName("receive_info")
                .HasMaxLength(2000)
                .HasComment("收货信息（JSON格式）");

            builder.Property(t => t.Remark)
                .HasColumnName("remark")
                .HasMaxLength(500)
                .HasComment("订单备注");

            builder.Property(t => t.NumberOfInstallments)
                .HasColumnName("number_of_installments")
                .HasComment("分期付款期数");

            // ------------------- 状态管理 -------------------
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasComment("订单状态（0待付款 1已付款待发货 2待收货 3已完成 4售后 ）");
        }
    }
}
