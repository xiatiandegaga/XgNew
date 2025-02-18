using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.BankMallH5.Cloud.Model
{
    public class QrcodeModel: BaseReturnModel
    {
        /// <summary>
        /// 消息ID，原样
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// 报文应答时间 字符串 是 格式yyyy-MMdd HH:mm:ss
        /// </summary>
        public string ResponseTimeStamp { get; set; }

        /// <summary>
        /// 请求系统预留
        /// </summary>
        public string SrcReserve { get; set; }

        /// <summary>
        /// 商户名称 字符串 
        /// </summary>
        public string MerName { get; set; }

        /// <summary>
        /// 商户订单号 字符串
        /// </summary>
        public string MerOrderId { get; set; }

        /// <summary>
        /// 商户号 字符串
        /// </summary>
        public string Mid { get; set; }

        /// <summary>
        ///  终端号 字符串
        /// </summary>
        public string Tid { get; set; }

        /// <summary>
        /// 平台流水号
        /// </summary>
        public string SeqId { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string Status { get; set; }
    
        /// <summary>
        /// 支付总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
 
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string TargetOrderId { get; set; }

        /// <summary>
        /// 目标平台代码
        /// </summary>
        public string TargetSys { get; set; }

        /// <summary>
        /// 合作方
        /// </summary>
        public string ConnectSys { get; set; }

        /// <summary>
        /// 目标平台的状
        /// </summary>
        public string TargetStatus { get; set; }


        /// <summary>
        ///  支付二维码 
        /// </summary>
        public string QrCode { get; set; }
    }
}
