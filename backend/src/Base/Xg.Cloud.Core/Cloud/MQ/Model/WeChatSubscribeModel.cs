using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.MQ.Model
{
    /// <summary>
    /// 微信小程序订阅模型
    /// </summary>
    public class WeChatSubscribeModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 模板编号
        /// </summary>
        public string TemplateType { get;set;}
        /// <summary>
        /// 推送用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 订阅用户id
        /// </summary>
        public long ReceiveUserId { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PustDate { get; set; }
    }
}
