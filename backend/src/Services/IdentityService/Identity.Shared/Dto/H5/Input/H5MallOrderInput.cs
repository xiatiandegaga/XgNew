using Identity.Shared.Dto.Admin.Input;
using System.Collections.Generic;

namespace Identity.Shared.Dto.H5.Input
{
    public class H5MallOrderModifyReturnLogisticsNoInput
    {
        /// <summary>
        /// 退单的明细单id
        ///</summary>
        public long Id { get; set; }

        /// <summary>
        /// 退单物流单号 
        ///</summary>
        public string ReturnLogisticsNo { get; set; }
    }

    public class H5MallOrderReturnInput
    {
        public long Id { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 退单收货地址
        /// </summary>
        //public ReceiveInfoModel ReceiveInfo { get; set; }

        /// <summary>
        /// 申请原因
        /// </summary>
        public string ApplicationReason { get; set; }

        /// <summary>
        /// 申请说明
        /// </summary>
        public string ApplicationDescription { get; set; }

        /// <summary>
        /// 申请图片 
        ///</summary>
        public string ApplicationImgs { get; set; }
    }

    public class H5MallOrderPayInput
    {
        /// <summary>
        /// 订单支付的总金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 分期周期
        /// </summary>
        public int NumberOfInstallments { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public ReceiveInfoInput ReceiveInfo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单明细
        /// </summary>
        public List<H5MallOrderSettleList> OrderItems { get; set; }


    }

    public class H5MallOrderSettleList
    {

        /// <summary>
        /// 商品的skuid（必填）
        /// </summary>
        public long ProductSkuId { get; set; }


        /// <summary>
        ///商品数量（必填）
        /// </summary>
        public int ProductQuantity { get; set; }

        /// <summary>
        /// 选择的商品属性 json字符串 格式如：[{"attrKeyName":"颜色","attrValueName":"白色"},{"attrKeyName":"尺码","attrValueName":"xxl"}]
        /// </summary>
        public string ProductSkuAttrs { get; set; }

    }
}
