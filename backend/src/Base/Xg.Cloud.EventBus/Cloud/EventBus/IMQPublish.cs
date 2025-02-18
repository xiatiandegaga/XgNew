namespace Cloud.EventBus
{
    public interface IMQPublish
    {
        /// <summary>
        /// 推送到队列里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey">路由键</param>
        /// <param name="message">待推送的消息</param>
        /// <param name="exchangeType">交换机类型，默认direct</param>
        void PushMessage<T>(string routingKey, T message, string exchangeType = "direct") where T : class;

        /// <summary>
        /// 推送到延时队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey">路由键</param>
        /// <param name="message">待推送的消息</param>
        /// <param name="delayTime">延时时间，默认30分钟（单位：秒）</param>
        void PushDelayMessage<T>(string routingKey, T message, int delayTime =30*60) where T : class;
    }
}
