using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.BankMallH5.Cloud.QueryModel
{
    public class AccessTokenQueryModel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public string? AppId { get; set;}
        /// <summary>
        /// 时间戳yyyyMMddHHmmss
        /// </summary>
        public string? Timestamp { get; set;}
        /// <summary>
        /// 随机数
        /// </summary>
        public string? Nonce { get; set;}
        /// <summary>
        /// 签名方法
        /// </summary>
        public string SignMethod { get; set; } = "SHA256";
        /// <summary>
        /// 签名  SHA256_hex(appId+timestamp+nonce+appKey)
        /// </summary>
        public string? Signature { get; set; }
    }
}
