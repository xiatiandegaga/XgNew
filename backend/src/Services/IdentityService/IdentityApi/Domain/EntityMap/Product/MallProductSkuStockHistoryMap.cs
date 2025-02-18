using Domain.Entity;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;

namespace Domain.EntityMap.Product
{
    public class MallProductSkuStockHistoryMap : BaseEntityConfiguration<MallProductSkuStockHistory>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProductSkuStockHistory> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_product_sku_stock_history"); // 统一 snake_case 格式

            // ===================== 字段配置 =====================
            // ------------------- 基础信息 -------------------
            builder.Property(t => t.StockNo)
                .HasColumnName("stock_no")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("库存操作流水号");

            // ------------------- 商品关联 -------------------
            builder.Property(t => t.ProductId)
                .HasColumnName("product_id")
                .IsRequired()
                .HasComment("关联商品ID");

            builder.Property(t => t.SkuId)
                .HasColumnName("sku_id")
                .IsRequired()
                .HasComment("关联SKU ID");

            // ------------------- 库存操作信息 -------------------
            builder.Property(t => t.Num)
                .HasColumnName("num")
                .IsRequired()
                .HasComment("操作数量（正数为增加，负数为减少）");

            builder.Property(t => t.StockType)
                .HasColumnName("stock_type")
                .IsRequired()
                .HasComment("库存操作类型（1-入库，2-出库，3-调整）");

            builder.Property(t => t.StockDetailType)
                .HasColumnName("stock_detail_type")
                .HasMaxLength(200)
                .HasComment("库存操作明细类型（如：采购入库、销售出库）");

            // ------------------- 其他信息 -------------------
            builder.Property(t => t.Remark)
                .HasColumnName("remark")
                .HasMaxLength(500)
                .HasComment("操作备注");

            builder.Property(t => t.GoodsAttrs)
                .HasMaxLength(1000)
                .HasColumnName("goods_attrs")
                .HasComment("商品属性信息（JSON格式）");
        }
    }
}
