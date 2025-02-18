using Domain.Entity.Dictionary;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;

namespace Domain.EntityMap.Dictionary
{
    /// <summary>
    /// 
    ///</summary>
    public class ImgRuleMap : BaseEntityConfiguration<ImgRule>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<ImgRule> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("dic_img_rule"); 

            // ===================== 字段配置 =====================
            // 图片规则编码
            builder.Property(t => t.ImgRuleCode)
                   .HasColumnName("img_rule_code")  
                   .HasMaxLength(100)
                   .IsRequired()
                   .HasComment("图片规则编码");

            // 规则名称
            builder.Property(t => t.ImgRuleName)
                   .HasColumnName("img_rule_name")
                   .HasMaxLength(100)
                   .IsRequired()
                   .HasComment("规则显示名称（示例：商品主图规则）");

            // 备注说明
            builder.Property(t => t.Remark)
                   .HasColumnName("remark")
                   .HasMaxLength(500)
                   .HasComment("规则详细描述");

            // 排序序号
            builder.Property(t => t.SortNo)
                   .HasColumnName("sort_no")
                   .HasDefaultValue(0) 
                   .HasComment("排序序号（数值越小越靠前）");

        }
    }
}
