using Cloud.Domain.Entities;
using Identity.Shared.Dto.Admin.Input;
using System;
using System.Collections.Generic;
namespace Identity.Shared.Dto.Admin.General
{
    /// <summary>
    /// Dto
    ///</summary>
    public class MallOrderDto : BaseEntity<long>
    {
        /// <summary>
        /// 订单号 
        ///</summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 用户id 
        ///</summary>
        public long UserId { get; set; }
        /// <summary>
        /// 订单状态（0待付款 1已付款待发货 2待收货 3已完成 4售后 ）
        ///</summary>
        public short Status { get; set; }

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
        /// 总金额（单位：元） 
        ///</summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 实际支付金额（单位：元） 
        ///</summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 管理员后台调整订单使用的折扣金额 (单位：元)
        ///</summary>
        public decimal AdminDiscountAmount { get; set; }
        /// <summary>
        /// 支付方式 
        ///</summary>
        public string PayType { get; set; }
        /// <summary>
        /// 物流公司 
        ///</summary>
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// 物流单号 
        ///</summary>
        public string LogisticsNo { get; set; }
        /// <summary>
        /// 订单支付单号（微信、支付宝等） 
        ///</summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 收货地址信息
        /// </summary>
        public ReceiveInfoInput ReceiveInfoModel { get; set; }
        /// <summary>
        /// 订单备注 
        ///</summary>
        public string Remark { get; set; }
        /// <summary>
        /// 支付时间 
        ///</summary>
        public DateTime? PaymentTime { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        public DateTime CreateTime { get; set; }
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
        /// 用户名称
        ///</summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户手机号
        ///</summary>
        public string UserMobile { get; set; }
        /// <summary>
        /// 订单状态名称
        ///</summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 物流公司 
        ///</summary>
        public string LogisticsCompanyName { get; set; }
        /// <summary>
        /// 物流公司logo
        ///</summary>
        public string LogisticsCompanyLogo { get; set; }

        /// <summary>
        /// 收货地址信息
        /// </summary>
        public string ReceiveInfo { get; set; }
        /// <summary>
        /// 详情信息
        /// </summary>
        public List<MallOrderDetailDto> MallOrderDetails { get; set; }
    }
}
