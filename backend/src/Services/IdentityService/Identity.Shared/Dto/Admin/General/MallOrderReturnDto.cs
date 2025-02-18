using Cloud.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.General
{
    public class MallOrderReturnDto : BaseEntity<long>
    {
        /// <summary>
        /// 订单号 
        ///</summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 关联子单订单号 
        ///</summary>
        public string RefOrderNo { get; set; }

        /// <summary>
        /// 关联主单订单号 
        ///</summary>
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
        public int DetailStatus { get; set; }
        /// <summary>
        /// 产品id 
        ///</summary>
        public long ProductSkuId { get; set; }
        /// <summary>
        /// 产品名称 
        ///</summary>
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
        public string ProductSkuAttrs { get; set; }
        /// <summary>
        /// 产品图片 
        ///</summary>
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
        public string ResponseResult { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserMobile { get; set; }

        /// <summary>
        /// 审核用户
        /// </summary>
        public string CheckUserName { get; set; }

        /// <summary>
        /// 产品价格（单位：元） 
        ///</summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 总金额（单位：元） 
        ///</summary>
        public decimal TotalProductAmount { get; set; }

        /// <summary>
        /// 详细状态
        /// </summary>
        public string DetailStatusName { get; set; }

        /// <summary>
        /// 申请原因
        /// </summary>
        public string ApplicationReason { get; set; }

        public string ApplicationReasonName { get; set; }

        /// <summary>
        /// 申请说明
        /// </summary>
        public string ApplicationDescription { get; set; }

        /// <summary>
        /// 申请图片 
        ///</summary>
        public string ApplicationImgs { get; set; }


        /// <summary>
        /// 退单物流单号 
        ///</summary>
        public string ReturnLogisticsNo { get; set; }
    }
}
