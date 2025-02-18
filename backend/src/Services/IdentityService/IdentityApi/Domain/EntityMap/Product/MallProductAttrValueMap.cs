using Domain.Entity.Product;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Product
{
    /// <summary>
    /// 产品属性表值value
    ///</summary>
    public class MallProductAttrValueMap : BaseEntityConfiguration<MallProductAttrValue>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProductAttrValue> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_product_attr_value"); // 统一 snake_case

            // ===================== 字段配置 =====================
            // ------------------- 属性值信息 -------------------
            builder.Property(t => t.AttrValueName)
                .HasColumnName("attr_value_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("属性值名称（如：红色、XL码）");

            // ------------------- 关联信息 -------------------
            builder.Property(t => t.MallProductAttrKeyId)
                .HasColumnName("mall_product_attr_key_id")
                .IsRequired()
                .HasComment("关联属性键ID");

            // ------------------- 展示控制 -------------------
            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasDefaultValue(0)
                .HasComment("排序序号（数值越小越靠前）");

        }
    }
}
