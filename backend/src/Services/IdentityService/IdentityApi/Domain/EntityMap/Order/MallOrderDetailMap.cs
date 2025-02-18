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
    public class MallOrderDetailMap : BaseEntityConfiguration<MallOrderDetail>
    {
        public void Configure(EntityTypeBuilder<MallOrderDetail> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_order_detail"); // 统一 snake_case 格式


            // ===================== 字段配置 =====================

            // ------------------- 订单信息 -------------------
            builder.Property(t => t.OrderNo)
                .HasColumnName("order_no")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("业务订单号（唯一标识）");

            builder.Property(t => t.MallOrderId)
                .HasColumnName("mall_order_id")
                .IsRequired()
                .HasComment("关联主订单ID");

            // ------------------- 商品信息 -------------------
            builder.Property(t => t.ProductSkuId)
                .HasColumnName("product_sku_id")
                .IsRequired()
                .HasComment("商品SKU唯一标识");

            builder.Property(t => t.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("商品名称（快照）");

            builder.Property(t => t.ProductQuantity)
                .HasColumnName("product_quantity")
                .HasDefaultValue(1)
                .HasComment("购买数量");

            builder.Property(t => t.ProductPrice)
                .HasColumnName("product_price")
                .HasColumnType("int") // 单位：分
                .IsRequired()
                .HasComment("商品单价（单位：分）");

            // ------------------- SKU信息 -------------------
            builder.Property(t => t.ProductSkuAttrs)
                .HasColumnName("product_sku_attrs")
                .HasMaxLength(1000)
                .HasComment("SKU属性（JSON格式）");

            builder.Property(t => t.ProductImgs)
                .HasColumnName("product_imgs")
                .HasMaxLength(1000)
                .HasComment("商品图片URL集合（分号分隔）");

            // ------------------- 状态管理 -------------------
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasComment("订单状态：0待付款 1待发货 2待收货 3已完成 4退款申请中 5退款中 6已退款 7退货申请中 8退货中 9已退货 10已评价 11已取消");

            builder.Property(t => t.ThirdReturnOrderNo)
                .HasColumnName("third_return_order_no")
                .HasMaxLength(100)
                .HasComment("第三方退单号（支付系统要求）");

        }
    }
}
