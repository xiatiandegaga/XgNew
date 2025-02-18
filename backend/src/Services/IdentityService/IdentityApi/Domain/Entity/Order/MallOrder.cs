using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity.Order
{
    /// <summary>
    /// 
    ///</summary>
    public class MallOrder : BaseEntity<long>
    {
        /// <summary>
        /// 订单号 
        ///</summary>
        [MaxLength(100)]
        public string OrderNo { get; set; }
        /// <summary>
        /// 用户id 
        ///</summary>
        public long UserId { get; set; }
        /// <summary>
        /// 订单状态（0待付款 1已付款待发货 2待收货 3已完成 4售后 ） 
        ///</summary>
        public int Status { get; set; }

        /// <summary>
        /// 总金额（分） 
        ///</summary>
        public int TotalPrice { get; set; }
        /// <summary>
        /// 实际支付金额（分） 
        ///</summary>
        public int PayPrice { get; set; }
        /// <summary>
        /// 管理员后台调整订单使用的折扣金额 
        ///</summary>
        public int AdminDiscountPrice { get; set; }
        /// <summary>
        /// 支付方式 
        ///</summary>
        [MaxLength(100)]
        public string PayType { get; set; }
        /// <summary>
        /// 物流公司 
        ///</summary>
        [MaxLength(100)]
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// 物流单号 
        ///</summary>
        [MaxLength(100)]
        public string LogisticsNo { get; set; }
        /// <summary>
        /// 订单支付单号（微信、支付宝等） 
        ///</summary>
        [MaxLength(100)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 收货地址信息：Json格式
        /// </summary>
        [MaxLength(2000)]
        public string ReceiveInfo { get; set; }
        /// <summary>
        /// 订单备注 
        ///</summary>
        [MaxLength(500)]
        public string Remark { get; set; }
        /// <summary>
        /// 支付时间 
        ///</summary>
        public DateTime? PaymentTime { get; set; }
        /// <summary>
        /// 发货时间 
        ///</summary>
        public DateTime? DeliveryTime { get; set; }
        /// <summary>
        /// 确认收货时间 
        ///</summary>
        public DateTime? ReceiveTime { get; set; }
        /// <summary>
        /// 评价时间 
        ///</summary>
        public DateTime? CommentTime { get; set; }

        /// <summary>
        /// 分期期数
        /// </summary>
        public int NumberOfInstallments { get; set; }

        /// <summary>
        /// 传给第三方的订单号（比如给银联，每次支付要更新订单号，太bt了）
        /// </summary>
        [MaxLength(100)]
        public string ThirdOrderNo { get; set; }


        public virtual List<MallOrderDetail> MallOrderDetails { get; set; }
    }
}
