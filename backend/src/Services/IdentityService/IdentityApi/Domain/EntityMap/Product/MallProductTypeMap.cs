using Domain.Entity.Product;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Product
{
    /// <summary>
    /// 产品类型表
    ///</summary>
    public class MallProductTypeMap : BaseEntityConfiguration<MallProductType>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProductType> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("mall_product_type"); // 统一 snake_case 格式

            // ===================== 字段配置 =====================
            // ------------------- 基础信息 -------------------
            builder.Property(t => t.TypeName)
                .HasColumnName("type_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("商品类型名称");

            builder.Property(t => t.TypeCode)
                .HasColumnName("type_code")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("商品类型编码（唯一标识）");

            // ------------------- 展示控制 -------------------
            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasDefaultValue(0)
                .HasComment("排序序号（数值越小越靠前）");

        }
    }
}
