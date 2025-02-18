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
    public class MallBrandMap : BaseEntityConfiguration<MallBrand>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallBrand> builder)
        {
            // ===================== 表结构配置 =====================
            builder.ToTable("mall_brand"); // 统一 snake_case 格式

            // ===================== 字段配置 =====================
            // ------------------- 品牌基础信息 -------------------
            builder.Property(t => t.BrandName)
                .HasColumnName("brand_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("品牌名称");

            builder.Property(t => t.BrandCode)
                .HasColumnName("brand_code")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("品牌唯一编码");

            // ------------------- 图片信息 -------------------
            builder.Property(t => t.BrandImg)
                .HasColumnName("brand_img")
                .HasMaxLength(500)
                .HasComment("品牌展示图（完整URL）");

            builder.Property(t => t.BrandLogo)
                .HasColumnName("brand_logo")
                .HasMaxLength(100)
                .HasComment("品牌Logo标识（资源路径）");

            // ------------------- 展示配置 -------------------
            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasDefaultValue(0)
                .HasComment("排序序号（数值越小越靠前）");

        }
    }
}
