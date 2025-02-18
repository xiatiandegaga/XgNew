using Cloud.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity.Order
{
    public class MallOrderReturn : BaseEntity<long>
    {

        /// <summary>
        /// 订单号 
        ///</summary>
        [MaxLength(100)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 退单子单订单号 
        ///</summary>
        [MaxLength(100)]
        public string RefOrderNo { get; set; }

        /// <summary>
        /// 退单主单订单号 
        ///</summary>
        [MaxLength(100)]
        public string RefMallOrderNo { get; set; }

        
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 状态 （0待审核 1已通过 2已拒绝）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 详细状态
        /// </summary>
        //public int DetailStatus { get; set; }
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
        /// 产品数量 
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
        /// 订单明细id 
        ///</summary>
        public long MallOrderDetailId { get; set; }
        /// <summary>
        /// 审核用户id
        /// </summary>
        public long CheckUserId { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 处理意见
        /// </summary>
        [MaxLength(500)]
        public string ResponseResult { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 退货收货地址信息：Json格式
        /// </summary>
        //[MaxLength(1000)]
        //public string ReceiveInfo { get; set; }

        /// <summary>
        /// 申请原因
        /// </summary>
        [MaxLength(100)]
        public string ApplicationReason { get; set; }

        /// <summary>
        /// 申请说明
        /// </summary>
        [MaxLength(2000)]
        public string ApplicationDescription { get; set; }

        /// <summary>
        /// 申请图片 
        ///</summary>
        [MaxLength(1000)]
        public string ApplicationImgs { get; set; }

        /// <summary>
        /// 退单物流单号 
        ///</summary>
        [MaxLength(100)]
        public string ReturnLogisticsNo { get; set; }

        public virtual MallOrderDetail MallOrderDetail { get; set; }

    }
}
