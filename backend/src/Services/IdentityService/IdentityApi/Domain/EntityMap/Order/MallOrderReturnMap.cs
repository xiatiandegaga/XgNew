using Domain.Entity.Order;
using IdentityApi.Domain.EntityMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xg.Cloud.Core;
namespace Domain.EntityMap.Order
{
    /// <summary>
    /// 
    ///</summary>
    public class MallOrderReturnMap : BaseEntityConfiguration<MallOrderReturn>
    {
        public override void ConfigureCustomFields(EntityTypeBuilder<MallOrderReturn> builder)
        {
            // ===================== 表结构配置 =====================
            builder.ToTable("mall_order_return"); // 统一 snake_case

            // ===================== 字段配置 =====================
            // ------------------- 订单关联信息 -------------------
            builder.Property(t => t.OrderNo)
                .HasColumnName("order_no")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("原始订单编号（唯一标识）");

            builder.Property(t => t.RefOrderNo)
                .HasColumnName("ref_order_no")
                .HasMaxLength(100)
                .HasComment("关联退货单号（外部系统）");

            builder.Property(t => t.RefMallOrderNo)
                .HasColumnName("ref_mall_order_no")
                .HasMaxLength(100)
                .HasComment("商城系统退货单号");

            // ------------------- 商品信息快照 -------------------
            builder.Property(t => t.ProductSkuId)
                .HasColumnName("product_sku_id")
                .IsRequired()
                .HasComment("商品SKU唯一标识");

            builder.Property(t => t.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("商品名称（退货时快照）");

            builder.Property(t => t.ProductQuantity)
                .HasColumnName("product_quantity")
                .HasDefaultValue(1)
                .HasComment("退货数量");

            builder.Property(t => t.ProductPrice)
                .HasColumnName("product_price")
                .HasComment("产品价格（单价：分） ");

            builder.Property(t => t.ProductSkuAttrs)
                .HasColumnName("product_sku_attrs")
                .HasMaxLength(1000)
                .HasComment("SKU属性（JSON格式）");

            builder.Property(t => t.ProductImgs)
                .HasColumnName("product_imgs")
                .HasMaxLength(1000)
                .HasComment("商品图片（分号分隔URL集合）");

            // ------------------- 流程控制 -------------------
            builder.Property(t => t.MallOrderDetailId)
                .HasColumnName("mall_order_detail_id")
                .IsRequired()
                .HasComment("关联订单明细ID");

            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired()
                .HasComment("退货状态：APPLIED-已申请 AUDITING-审核中 COMPLETED-已完成");

            // ------------------- 申请信息 -------------------
            builder.Property(t => t.ApplicationReason)
                .HasColumnName("application_reason")
                .HasMaxLength(200)
                .HasComment("申请原因分类（质量问题/七天无理由等）");

            builder.Property(t => t.ApplicationDescription)
                .HasColumnName("application_description")
                .HasMaxLength(500)
                .HasComment("问题详细描述");

            builder.Property(t => t.ApplicationImgs)
                .HasColumnName("application_imgs")
                .HasMaxLength(2000)
                .HasComment("凭证图片（分号分隔URL集合）");

            // ------------------- 审核信息 -------------------
            builder.Property(t => t.CheckUserId)
                .HasColumnName("check_user_id")
                .HasComment("审核人ID");

            builder.Property(t => t.CheckDate)
                .HasColumnName("check_date")
                .HasComment("审核时间");

            builder.Property(t => t.ResponseResult)
                .HasColumnName("response_result")
                .HasMaxLength(500)
                .HasComment("审核结果说明");

            // ------------------- 物流信息 -------------------
            builder.Property(t => t.ReturnLogisticsNo)
                .HasColumnName("return_logistics_no")
                .HasMaxLength(100)
                .HasComment("退货物流单号");

            // ------------------- 用户信息 -------------------
            builder.Property(t => t.UserId)
                .HasColumnName("user_id")
                .IsRequired()
                .HasComment("申请人ID");

            // ------------------- 时间信息 -------------------
            builder.Property(t => t.CreateDate)
                .HasColumnName("create_date")
                .IsRequired()
                .HasComment("申请提交时间");

            // ------------------- 附加信息 -------------------
            builder.Property(t => t.Remark)
                .HasColumnName("remark")
                .HasMaxLength(500)
                .HasComment("系统操作备注");

        }
    }
}
