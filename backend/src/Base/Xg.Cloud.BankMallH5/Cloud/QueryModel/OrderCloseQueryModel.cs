using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.BankMallH5.Cloud
{
    public class OrderCloseQueryModel
    {


        /// <summary>
        /// 报文请求时间 格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string? requestTimestamp { get; set; }


        /// <summary>
        /// 商户订单号 原交易订单号
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
        /// 终端号
        /// </summary>
        public string? tid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msgId { get;set; }
        /// <summary>
        /// 
        /// </summary>
        public string srcReserve {  get; set; }



    }
}
