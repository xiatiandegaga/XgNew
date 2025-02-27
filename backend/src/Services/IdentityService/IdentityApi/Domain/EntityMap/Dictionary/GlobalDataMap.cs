// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using Domain.Entity.Dictionary;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;

namespace Domain.EntityMap.Dictionary
{
    public class GlobalDataMap
           : BaseEntityConfiguration<GlobalData>

    {
        public override void ConfigureCustomFields(EntityTypeBuilder<GlobalData> builder)
        {
            // ---------------------------
            // 表配置
            // ---------------------------
            builder.ToTable("dic_global_data");



            // ---------------------------
            // 字段配置
            // ---------------------------

            // 编码字段
            builder.Property(t => t.Code)
                   .HasColumnName("code")
                   .HasMaxLength(100)
                   .IsRequired()
                   .HasComment("编码");

            // 名称字段
            builder.Property(t => t.Name)
                   .HasColumnName("name")
                   .HasMaxLength(100)
                   .IsRequired()
                   .HasComment("名称");

            // 排序字段
            builder.Property(t => t.SortNo)
                   .HasColumnName("sort_no")
                   .HasDefaultValue(0)
                   .HasComment("排序序号（数值越小越靠前）");

         
        }
    }
}
