using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.WeChat.Input
{
    public class WeChatTokenInput
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒。目前是7200秒之内的值。
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 错误码（0表示成功）
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }

    }

    public class WeChatCodeInput
    {
        /// <summary>
        /// 小程序code（用于换openid）
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 转发人
        /// </summary>
        public long Pid { get; set; }
        /// <summary>
        /// 海报类型1 活动海报 2 通用模块 3 商品海报（包括秒杀商品）
        /// </summary>
        //public string PostersType { get; set; }
        /// <summary>
        /// 海报详情id
        /// </summary>
        //public long PostersKey { get; set; }
        /// <summary>
        /// 首登门店
        /// </summary>
        //public long FieldId { get; set; }
    }
}
