using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.BankMallH5.Cloud.Model
{
    public class OrderCloseModel: BaseReturnModel
    {
        /// <summary>
        /// 链接系统
        /// </summary>
        public string? connectSys { get; set; }

        /// <summary>
        /// 结算时间  报文响应时间，格式：yyyy-MM-dd
        /// </summary>
        public string? settleDate { get; set; }

        /// <summary>
        /// 清分ID  如果来源方传了bankRefId就等于bankRefId，否则等于seqId
        /// </summary>
        public string? settleRefId { get; set; }

        /// <summary>
        /// 支付渠道商户号，各渠道情况不同，酌情转换。
        /// </summary>
        public string? targetMid { get; set; }
        /// <summary>
        /// 请求系统预留字段
        /// </summary>
        public string? srcReserve { get; set; }
        /// <summary>
        /// 报文响应时间  格式：yyyyMM-dd HH:mm:ss
        /// </summary>
        public string? responseTimestamp { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string? mid { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        public string? tid { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? merOrderId { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string? merName { get; set; }

        /// <summary>
        /// 平台流水号
        /// </summary>
        public string? seqId { get; set; }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string? status { get; set; }

        /// <summary>
        /// 目标平台的状态
        /// </summary>
        public string? targetStatus { get; set; }

        /// <summary>
        /// 目标平台代码
        /// </summary>
        public string? targetSys { get; set; }

        /// <summary>
        /// 支付总金额
        /// </summary>
        public string? totalAmount { get; set; }

    }
}
