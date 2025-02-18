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
    public class MallProductAttrKeyMap : BaseEntityConfiguration<MallProductAttrKey>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProductAttrKey> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_product_attr_key"); // 统一 snake_case

            // ===================== 字段配置 =====================
            // ------------------- 属性键信息 -------------------
            builder.Property(t => t.AttrKeyName)
                .HasColumnName("attr_key_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("属性键名称（如：颜色、尺寸）");

            // ------------------- 分类关联 -------------------
            builder.Property(t => t.MallProductTypeId)
                .HasColumnName("mall_product_type_id")
                .IsRequired()
                .HasComment("关联商品类型ID");

            builder.Property(t => t.MallProductCategoryId)
                .HasColumnName("mall_product_category_id")
                .HasComment("关联商品分类ID（可为空）");

            // ------------------- 展示控制 -------------------
            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasDefaultValue(0)
                .HasComment("排序序号（数值越小越靠前）");

        }
    }
}
