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
    public class MallProductMap : BaseEntityConfiguration<MallProduct>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallProduct> builder)
        {
            // ===================== 表结构配置 =====================
            builder.ToTable("mall_product"); // 统一 snake_case 格式

            // ===================== 字段配置 =====================
            // ------------------- 基础信息 -------------------
            builder.Property(t => t.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("商品名称（唯一标识）");

            builder.Property(t => t.ProductCode)
                .HasColumnName("product_code")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("商品唯一编码（业务标识）");

            builder.Property(t => t.ProductShortName)
                .HasColumnName("product_short_name")
                .HasMaxLength(100)
                .HasComment("商品简称（展示用短名称）");

            // ------------------- 分类关联 -------------------
            builder.Property(t => t.BrandId)
                .HasColumnName("brand_id")
                .HasComment("关联品牌ID");

            builder.Property(t => t.MallProductCategoryId)
                .HasColumnName("mall_product_category_id")
                .IsRequired()
                .HasComment("关联商品分类ID");

            builder.Property(t => t.MallProductTypeId)
                .HasColumnName("mall_product_type_id")
                .IsRequired()
                .HasComment("关联商品类型ID");

            // ------------------- 图片资源 -------------------
            builder.Property(t => t.ProductMainImg)
                .HasColumnName("product_main_img")
                .HasMaxLength(1000)
                .HasComment("商品主图URL（首屏展示图）");

            builder.Property(t => t.ProductDetailImg)
                .HasColumnName("product_detail_img")
                .HasMaxLength(1000)
                .HasComment("商品详情图URL（分号分隔多图）");

            // ------------------- 展示控制 -------------------
            builder.Property(t => t.SortNo)
                .HasColumnName("sort_no")
                .HasDefaultValue(0)
                .HasComment("排序序号（数值越小越靠前）");

            // ------------------- 状态控制 -------------------
            builder.Property(t => t.RecommendStatus)
                .HasColumnName("recommend_status")
                .HasDefaultValue((short)0)
                .HasComment("推荐状态（0-未推荐，1-推荐）");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasDefaultValue((short)0)
                .HasComment("商品状态（0-下架，1-上架）");

            builder.Property(t => t.NewStatus)
                .HasColumnName("new_status")
                .HasDefaultValue((short)0)
                .HasComment("新品状态（0-非新品，1-新品）");

            // ------------------- 其他信息 -------------------
            builder.Property(t => t.UnitName)
                .HasColumnName("unit_name")
                .HasMaxLength(100)
                .HasComment("商品单位（如：件、套、箱）");

            builder.Property(t => t.Remark)
                .HasColumnName("remark")
                .HasMaxLength(500)
                .HasComment("商品备注（内部使用）");

            builder.Property(t => t.Desc)
                .HasColumnName("desc")
                .HasMaxLength(500)
                .HasComment("商品卖点描述（前端展示）");
  
        }
    }
}
