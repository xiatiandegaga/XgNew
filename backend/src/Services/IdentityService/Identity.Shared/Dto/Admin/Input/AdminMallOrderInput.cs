using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.Input
{
    /// <summary>
    /// 收货地址
    /// </summary>
    public class ReceiveInfoInput
    {
        public long Id { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 收货人地址-省 
        ///</summary>
        public string ReceiverProvinceName { get; set; }
        /// <summary>
        /// 收货人地址-市 
        ///</summary>
        public string ReceiverCityName { get; set; }
        /// <summary>
        /// 收货人地址-区 
        ///</summary>
        public string ReceiverCountyName { get; set; }
        /// <summary>
        /// 收货人详细地址 
        ///</summary>
        public string ReceiverDetailInfo { get; set; }
        /// <summary>
        /// 收货人邮编 
        ///</summary>
        public string ReceiverPostCode { get; set; }
        /// <summary>
        /// 收货人电话 
        ///</summary>
        public string ReceiverMobile { get; set; }
        /// <summary>
        /// 收货人姓名 
        ///</summary>
        public string ReceiverName { get; set; }
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    public class AdminMallOrderInput : PageQueryCommonInput
    {
        /// <summary>
        /// 订单状态（0待付款 1待发货 2待收货 3已完成 4售后 ）
        /// </summary>
        public int? Status { get; set; }
    }

    /// <summary>
    /// 退单审核
    /// </summary>
    public class AdminOrderCheckInput
    {
        public long Id { get; set; }

        public int Status { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    /// 发货
    /// </summary>
    public class AdminOrderSendOutInput
    {
        public long Id { get; set; }
        /// <summary>
        /// 物流公司 
        ///</summary>
        public string LogisticsCompany { get; set; }
        /// <summary>
        /// 物流单号 
        ///</summary>
        public string LogisticsNo { get; set; }
    }
}
