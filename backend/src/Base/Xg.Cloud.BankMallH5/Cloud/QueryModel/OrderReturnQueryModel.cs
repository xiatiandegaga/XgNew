using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.BankMallH5.Cloud.QueryModel
{
    public class OrderReturnQueryModel
    {

        /// <summary>
        /// 消息ID
        /// </summary>
        public string? msgId { get;set; }

        /// <summary>
        /// 报文请求时间 格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string? requestTimestamp { get; set; }

        /// <summary>
        /// 请求系统预留字段
        /// </summary>
        public string? srcReserve { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? merOrderId { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string? mid { get; set; }

        /// <summary>
        /// 业务类型 字符串 8..32 是 QRINSTALDEFAULT
        /// </summary>
        public string? instMid { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string? targetOrderId { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        public string? tid { get; set; }

        /// <summary>
        /// 要退货的金额
        /// </summary>
        public decimal refundAmount { get; set; }

        /// <summary>
        /// 退货说明
        /// </summary>
        public string? refundDesc { get; set; }

        /// <summary>
        /// 退货交易的订单号  如不指定，则系统自动生成。如商户指定，须以4位来源编号（由银商分配）开头。
        /// </summary>
        public string? refundOrderId { get; set; }

        /// <summary>
        /// 平台商户 分账金额  若下单接口中上送了分账标记字段divisionFlag，则该字段必传 且 退款接口platformAmount小于下单接口中上送的platformAmount全额退款时platformAmount不传
        /// </summary>
        public decimal platformAmount { get; set; }

    }
}
