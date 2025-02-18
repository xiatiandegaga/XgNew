using Cloud.Domain.Entities;
using Domain.Entity.Dictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;

namespace IdentityApi.Domain.EntityMap
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity<long>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // ---------------------------
            // 主键配置
            // ---------------------------
            builder.Property(t => t.Id)
                   .HasColumnName("id")
                   .IsRequired();

            // 删除标记字段
            builder.Property(t => t.DeletedStatus)
                   .HasColumnName("deleted_status")
                   .IsRequired()
                   .HasComment("是否删除 1正常 2停用");

            // 创建时间字段
            builder.Property(t => t.CreatedAt)
                   .HasColumnName("created_at")
                   .IsRequired()
                   .HasComment("创建时间");

            // 创建人字段
            builder.Property(t => t.CreatedBy)
                   .HasColumnName("created_by")
                   .IsRequired()
                   .HasComment("创建人");

            // 更新时间字段
            builder.Property(t => t.UpdatedAt)
                   .HasColumnName("updated_at")
                   .IsRequired()
                   .HasComment("更新时间");

            // 更新人字段
            builder.Property(t => t.UpdatedBy)
                   .HasColumnName("updated_by")
                   .IsRequired()
                   .HasComment("更新人");


            // ---------------------------
            // 全局查询过滤器
            // ---------------------------
            builder.HasQueryFilter(x =>
                x.DeletedStatus == CommonConst.DeletedStatus_Normal
            );

            ConfigureCustomFields(builder);
        }

        /// <summary>
        /// 子类扩展点（添加实体特有字段）
        /// </summary>
        public virtual void ConfigureCustomFields(EntityTypeBuilder<T> builder)
        {
            // 默认空实现，子类重写此方法添加特有字段
        }
    }
}
