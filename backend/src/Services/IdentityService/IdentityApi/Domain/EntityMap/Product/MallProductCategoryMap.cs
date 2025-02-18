using Domain.Entity.Product;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Product
{
    /// <summary>
    /// 产品目录表
    ///</summary>
    public class MallProductCategoryMap : BaseEntityConfiguration<MallProductCategory>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProductCategory> builder)
        {
            // ===================== 表结构配置 =====================
            builder.ToTable("mall_product_category"); // snake_case 规范

            // ===================== 字段配置 =====================
            // ------------------- 分类基础信息 -------------------
            builder.Property(t => t.CategoryName)
                .HasColumnName("category_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("分类名称");

            builder.Property(t => t.CategoryCode)
                .HasColumnName("category_code")
                .HasMaxLength(100)
                .HasComment("分类编码");

            // ------------------- 层级关系 -------------------
            builder.Property(t => t.Pid)
                .HasColumnName("pid")
                .HasComment("父级分类ID（0表示根分类）");

            // ------------------- 展示控制 -------------------
            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasDefaultValue(0)
                .HasComment("排序序号");

            builder.Property(t => t.ImageUrl)
                .HasColumnName("image_url")
                .HasMaxLength(1000)
                .HasComment("分类展示图URL");

            // ------------------- 类型关联 -------------------
            builder.Property(t => t.MallProductTypeId)
                .HasColumnName("mall_product_type_id")
                .IsRequired()
                .HasComment("关联商品类型ID");

        }
    }
}
