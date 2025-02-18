using Cloud.Domain.Entities;
using System;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// Dto
    ///</summary>
    public class MallOrderDetailDto : BaseEntity<long>
    {
        /// <summary>
        /// 订单号 
        ///</summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 产品id 
        ///</summary>
        public long ProductSkuId { get; set; }
        /// <summary>
        /// 产品名称 
        ///</summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品购买数量 
        ///</summary>
        public int ProductQuantity { get; set; }
        /// <summary>
        /// 产品价格（单价：分） 
        ///</summary>
        public int ProductPrice { get; set; }
        /// <summary>
        /// sku属性 
        ///</summary>
        public string ProductSkuAttrs { get; set; }
        /// <summary>
        /// 产品图片 
        ///</summary>
        public string ProductImgs { get; set; }
        /// <summary>
        /// 订单id 
        ///</summary>
        public long MallOrderId { get; set; }
        /// <summary>
        /// 订单详细状态(0待付款 1待发货 2待收货 3已完成 4退款申请中 5退款中 6已退款 7退货申请中 8退货中 9以退货 10已评价  11已取消)
        ///</summary>
        public int Status { get; set; }

        /// <summary>
        /// 订单详细状态(0待付款 1待发货 2待收货 3已完成 4退款申请中 5退款中 6已退款 7退货申请中 8退货中 9以退货 10已评价  11已取消)
        ///</summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 产品价格（单价：元） 
        ///</summary>
        public decimal ProductPriceAmount { get; set; }

    }
}
