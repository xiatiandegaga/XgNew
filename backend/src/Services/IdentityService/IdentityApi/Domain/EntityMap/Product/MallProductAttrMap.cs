using Domain.Entity.Product;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Product
{
    /// <summary>
    /// 产品属性表key
    ///</summary>
    public class MallProductAttrMap : BaseEntityConfiguration<MallProductAttr>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProductAttr> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_product_attr"); // 统一 snake_case

            // ===================== 字段配置 =====================
            // ------------------- 核心关联信息 -------------------
            builder.Property(t => t.MallProductId)
                .HasColumnName("mall_product_id")
                .IsRequired()
                .HasComment("关联商品ID");

            builder.Property(t => t.MallProductAttrKeyId)
                .HasColumnName("mall_product_attr_key_id")
                .IsRequired()
                .HasComment("关联属性键ID");

            builder.Property(t => t.MallProductAttrValueId)
                .HasColumnName("mall_product_attr_value_id")
                .IsRequired()
                .HasComment("关联属性值ID");
        }
    }
}
