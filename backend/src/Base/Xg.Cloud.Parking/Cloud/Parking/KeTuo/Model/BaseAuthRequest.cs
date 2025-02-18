using Cloud.Extensions;

namespace Cloud.Parking.KeTuo.Model
{
    public class BaseAuthRequest
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 车场id（只有获取停车场列表时不需要）
        /// </summary>
        public int parkId { get; set; }

        /// <summary>
        ///每次请求附带当前时间戳(格林威治时间1970年01月01日00时00分00秒起至现在的总毫秒数)超过10S请求失效
        /// </summary>
        public string ts { get; set; }

        /// <summary>
        ///每次请求的唯一标识，如果是异步接口，异步响应消息的reqId会与原请求的reqId一致。每次请求必须唯一！！！
        /// </summary>
        public string reqId { get; set; }


    }
}
