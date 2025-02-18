using System;
using System.Collections.Generic;

namespace Cloud.LogisticsFuQing.Models
{
    public class FuQingNoQueryResultResponse
    {
        /// <summary>
        /// 快递单号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 快递类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 快递节点明细
        /// </summary>
        public List<FuQingNoQueryResultResponseDetail> List { get; set; }

        /// <summary>
        ///  0：快递收件(揽件)1.在途中 2.正在派件 3.已签收 4.派送失败 5.疑难件 6.退件签收  */
        /// </summary>
        public string Deliverystatus { get; set; }

        /// <summary>
        ///  0：快递收件(揽件)1.在途中 2.正在派件 3.已签收 4.派送失败 5.疑难件 6.退件签收  */
        /// </summary>
        public string DeliverystatusName { get; set; }

        /// <summary>
        /// 是否签收
        /// </summary>
        public string Issign { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpName { get; set; }

        /// <summary>
        ///  快递公司官网
        /// </summary>
        public string ExpSite { get; set; }

        /// <summary>
        ///  快递公司电话
        /// </summary>
        public string ExpPhone { get; set; }

        /// <summary>
        ///  快递员 或 快递站(没有则为空)
        /// </summary>
        public string Courier { get; set; }

        /// <summary>
        ///  快递员电话 (没有则为空)
        /// </summary>
        public string CourierPhone { get; set; }

        /// <summary>
        ///  快递轨迹信息最新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///  发货到收货消耗时长 (截止最新轨迹) 
        /// </summary>
        public string TakeTime { get; set; }

        /// <summary>
        ///  快递公司LOGO
        /// </summary>
        public string Logo { get; set; }

        public string LogoName { get; set; }

        /// <summary>
        /// 物流公司 
        ///</summary>
        public string LogisticsCompany { get; set; }

        /// <summary>
        /// 物流公司 
        ///</summary>
        public string LogisticsCompanyName { get; set; }

        /// <summary>
        /// 物流公司logo
        ///</summary>
        public string LogisticsCompanyLogo { get; set; }

        /// <summary>
        /// 物流单号 
        ///</summary>
        public string LogisticsNo { get; set; }

        /// <summary>
        /// 订单状态（0待付款 1已付款待发货 2待收货 3已完成 4售后 ）
        ///</summary>
        public string OrderStatus { get; set; }
    }
}
