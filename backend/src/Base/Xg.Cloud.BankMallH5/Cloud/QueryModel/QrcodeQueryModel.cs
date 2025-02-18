using System.Reflection;
using Xg.Cloud.BankMallH5.Cloud.Model;

namespace Xg.Cloud.BankMallH5.Cloud.QueryModel
{
    public class QrcodeQueryModel
    {
        /// <summary>
        /// 消息ID  原样返回
        /// </summary>
        public string? msgId { get;set;}

        /// <summary>
        /// 报文请求时间 格式yyyy-MM-dd
        /// </summary>
        public string? requestTimestamp { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string? merOrderId { get; set; }

        /// <summary>
        /// 请求系统预留字段
        /// </summary>
        public string? srcReserve { get; set; }
   
        /// <summary>
        /// 商户号
        /// </summary>
        public string? mid { get; set; }
        
        /// <summary>
        /// 终端号
        /// </summary>
        public string? tid { get; set; }
     
        /// <summary>
        /// 业务类型
        /// </summary>
        public string? instMid { get; set; } = "QRINSTALDEFAULT";
     
        /// <summary>
        /// 订单描述
        /// </summary>
        public string? orderDesc { get; set; }

        /// <summary>
        /// 支付总金额
        /// </summary>
        public int totalAmount { get; set; }

        /// <summary>
        /// 申码时间 字符串 是 yyyyMMddHHmmss
        /// </summary>
        public string? timeStart { get; set; }

        /// <summary>
        /// 二维码过期时间 yyyyMMddHHmmss
        /// </summary>
        public string? expireTime { get; set; }

        /// <summary>
        /// 支付结果通知地址
        /// </summary>
        public string? notifyUrl { get; set; }

 

        public List<Goods> goods { get; set; }


        /// <summary>
        /// 分账标记
        /// </summary>
        public bool divisionFlag { get; set; }

            /// <summary>
            /// 平台商户分账金额
        /// </summary>
        public int? platformAmount { get; set; }


        public string? subOrders { get; set; }



        /// <summary>
        /// nstalBankNameList 支持的银行列表  对于多个银行用”,”（半角）号隔开，详见“银行
        /// </summary>
        public string? instalBankNameList { get; set; }

        /// <summary>
        /// 限定的分期数列表 对于多个分期数”,”（半角）号隔开
        /// </summary>
        public string? limitInstalNumList { get; set; }

        /// <summary>
        /// 是否强制使用限定的分期数 强制使用商户限定的期数信息进行分期 01：是，强制使用限定期数进行分期支付； 02：否，默认展示限定信息，用户在支付时可以重选（不上送该字段默认为01，在上送“限定的分期数列表”场景下生效）
        /// </summary>
        public string? isForceLimitInstalNum { get; set; } = "01";

        /// <summary>
        /// 支付完跳转地址
        /// </summary>
        public string? returnUrl { get; set; }
        /// <summary>
        /// 是否联合登录
        /// </summary>
        public bool unionLogin { get; set; }

        /// <summary>
        /// 手机号，使用UTF-8的base64编码
        /// </summary>
        public string? mobile { get; set; }
    }
}
