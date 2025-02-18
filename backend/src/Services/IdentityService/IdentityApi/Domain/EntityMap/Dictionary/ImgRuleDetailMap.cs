using Domain.Entity.Dictionary;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;

namespace Domain.EntityMap.Dictionary
{
    public class ImgRuleDetailMap : BaseEntityConfiguration<ImgRuleDetail>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<ImgRuleDetail> builder)
        {
            // ===================== 表配置 =====================
            builder.ToTable("dic_img_rule_detail");

            // ===================== 字段配置 =====================
            builder.Property(t => t.ImgRuleDetailCode)
                   .HasColumnName("img_rule_detail_code")
                   .HasMaxLength(100)
                   .IsRequired()
                   .HasComment("明细唯一编码");

            builder.Property(t => t.ImgRuleDetailName)
                   .HasColumnName("img_rule_detail_name")
                   .HasMaxLength(100)
                   .IsRequired()
                   .HasComment("明细显示名称");

            builder.Property(t => t.ImgRuleId)
                   .HasColumnName("img_rule_id")
                   .IsRequired()
                   .HasComment("关联规则主表ID");

            builder.Property(t => t.MainImg)
                   .HasColumnName("main_img")
                   .HasMaxLength(1000)
                   .HasComment("主图URL");

            builder.Property(t => t.DetailImage)
                   .HasColumnName("detail_image")
                   .HasMaxLength(1000)
                   .HasComment("详情图URL集合");

            builder.Property(t => t.LinkType)
                   .HasColumnName("link_type")
                   .HasMaxLength(200)
                   .HasComment("链接类型");

            builder.Property(t => t.LinkKey)
                   .HasColumnName("link_key")
                   .HasMaxLength(300)
                   .HasComment("链接关联键");

            builder.Property(t => t.LinkAddress)
                   .HasColumnName("link_address")
                   .HasMaxLength(1000)
                   .HasComment("直接跳转URL");

            builder.Property(t => t.StartTime)
                   .HasColumnName("start_time")
                   .IsRequired()
                   .HasComment("规则生效时间");

            builder.Property(t => t.EndTime)
                   .HasColumnName("end_time")
                   .HasComment("规则失效时间");

            builder.Property(t => t.SortNo)
                   .HasColumnName("sort_no")
                   .HasDefaultValue(0)
                   .HasComment("排序序号（数值越小越靠前）");
        }
    }
}
