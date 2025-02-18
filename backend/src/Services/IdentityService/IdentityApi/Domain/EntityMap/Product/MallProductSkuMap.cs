using Domain.Entity.Product;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Product
{
    /// <summary>
    /// 
    ///</summary>
    public class MallProductSkuMap : BaseEntityConfiguration<MallProductSku>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProductSku> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_product_sku"); // 统一 snake_case 格式

            // ===================== 字段配置 =====================
            // ------------------- 基础信息 -------------------
            builder.Property(t => t.SkuName)
                .HasColumnName("sku_name")
                .HasMaxLength(100)
                .HasComment("SKU名称");

            builder.Property(t => t.SkuCode)
                .HasColumnName("sku_code")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("SKU唯一编码");

            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasComment("排序序号（数值越小越靠前）");

            // ------------------- 价格信息 -------------------
            builder.Property(t => t.SkuPrice)
                .HasColumnName("sku_price")
                .HasComment("SKU销售价格");

            builder.Property(t => t.SkuInnerPrice)
                .HasColumnName("sku_inner_price")
                .IsRequired()
                .HasComment("SKU内部价格");

            // ------------------- 库存信息 -------------------
            builder.Property(t => t.SkuStock)
                .HasColumnName("sku_stock")
                .HasComment("SKU库存数量");

            builder.Property(t => t.FreezeStock)
                .HasColumnName("freeze_stock")
                .HasComment("SKU冻结库存数量");

            // ------------------- 图片信息 -------------------
            builder.Property(t => t.SkuImg)
                .HasColumnName("sku_img")
                .HasMaxLength(1000)
                .HasComment("SKU图片URL");

            // ------------------- 规格信息 -------------------
            builder.Property(t => t.AttrKeyValue)
                .HasColumnName("attr_key_value")
                .HasMaxLength(2000)
                .HasComment("SKU属性键值对（JSON格式）");

            builder.Property(t => t.SpecParam)
                .HasColumnName("spec_param")
                .HasMaxLength(2000)
                .HasComment("SKU规格参数（JSON格式）");

            // ------------------- 分期信息 -------------------
            builder.Property(t => t.NumberOfInstallments)
                .HasColumnName("number_of_installments")
                .HasMaxLength(100)
                .HasComment("SKU支持的分期期数");

            builder.Property(t => t.InnerNumberOfInstallments)
                .HasColumnName("inner_number_of_installments")
                .HasMaxLength(100)
                .HasComment("SKU内部支持的分期期数");

            // ------------------- 状态信息 -------------------
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasComment("SKU状态（0-下架，1-上架）");


        }
    }
}
