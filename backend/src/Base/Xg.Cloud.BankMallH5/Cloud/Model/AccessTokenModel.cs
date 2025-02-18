using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.BankMallH5.Cloud.Model
{
    public class AccessTokenModel
    {
        /// <summary>
        /// 错误代码  0000为成功
        /// </summary>
        public string? ErrCode { get; set;}
        /// <summary>
        /// 错误说明
        /// </summary>
        public string? ErrInfo { get; set; }
        /// <summary>
        /// 授权令牌
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public long ExpiresIn { get; set; }
    }
}
