using Cloud.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Order
{
    /// <summary>
    /// 
    ///</summary>
    public class MallOrderDetail : BaseEntity<long>
    {
        /// <summary>
        /// 订单号 
        ///</summary>
        [MaxLength(100)]
        public string OrderNo { get; set; }
        /// <summary>
        /// 产品id 
        ///</summary>
        public long ProductSkuId { get; set; }
        /// <summary>
        /// 产品名称 
        ///</summary>
        [MaxLength(100)]
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
        [MaxLength(1000)]
        public string ProductSkuAttrs { get; set; }
        /// <summary>
        /// 产品图片 
        ///</summary>
        [MaxLength(1000)]
        public string ProductImgs { get; set; }
        /// <summary>
        /// 订单id 
        ///</summary>
        public long MallOrderId { get; set; }

        /// <summary>
        /// 订单详细状态(0待付款 1待发货 2待收货 3已完成 4退款申请中 5退款中 6已退款 7退货申请中 8退货中 9已退货 10已评价  11已取消)
        ///</summary>
        public int Status { get; set; }

        /// <summary>
        /// 传给第三方的退单订单号（比如给银联，每次支付要更新订单号，太bt了）
        /// </summary>
        [MaxLength(100)]
        public string ThirdReturnOrderNo { get; set; }

    }
}
