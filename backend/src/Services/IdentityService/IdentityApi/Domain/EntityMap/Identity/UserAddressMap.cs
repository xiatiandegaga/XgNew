using Domain.Entity.Identity;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;

namespace Domain.EntityMap.Identity
{
    public class UserAddressMap : BaseEntityConfiguration<UserAddress>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<UserAddress> builder)
        {
            // ========== 表配置 ==========
            builder.ToTable("sys_user_address");

            // ========== 字段配置 ==========
            builder.Property(t => t.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.Property(t => t.ReceiverProvinceName)
                   .HasColumnName("receiver_province_name")
                   .HasMaxLength(50);

            builder.Property(t => t.ReceiverCityName)
                   .HasColumnName("receiver_city_name")
                   .HasMaxLength(50);

            builder.Property(t => t.ReceiverCountyName)
                   .HasColumnName("receiver_county_name")
                   .HasMaxLength(50);

            builder.Property(t => t.ReceiverDetailInfo)
                   .HasColumnName("receiver_detail_info")
                   .HasMaxLength(500);

            builder.Property(t => t.ReceiverPostCode)
                   .HasColumnName("receiver_post_code");

            builder.Property(t => t.ReceiverMobile)
                   .HasColumnName("receiver_mobile")
                   .HasMaxLength(50);

            builder.Property(t => t.ReceiverName)
                   .HasColumnName("receiver_name")
                   .HasMaxLength(100);

            builder.Property(t => t.IsDefault)
                   .HasColumnName("is_default")
                   .IsRequired();
        }
    }
}
