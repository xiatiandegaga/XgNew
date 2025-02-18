using Cloud.Models;
using Cloud.Utilities.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloud.EventBus.RabbitMQ
{
    public class RabbitMQPublish : IMQPublish
    {

        private IModel _channel;
        private readonly RabbitMQConnectOptions _mqConfig;
        private readonly ConfigOptions _configOptions;
        private readonly ILogger<RabbitMQPublish> _logger;


        public RabbitMQPublish(IOptions<RabbitMQConnectOptions> mqOptions, IOptions<ConfigOptions> configOptions, ILogger<RabbitMQPublish> logger)
        {
            _mqConfig = mqOptions.Value;
            _configOptions = configOptions.Value;
            _logger = logger;
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _mqConfig.MQHost ,
                    UserName = _mqConfig.MQUserName,
                    Password = _mqConfig.MQPassword,
                    Port = _mqConfig.MQPort,
                    VirtualHost = _mqConfig.VHost,
                    AutomaticRecoveryEnabled = true
                };
                Policy.Handle<Exception>()
                                            .Retry(5, (exception, retryCount, context) =>
                                            {
                                                _logger.LogError($"rabbitmq初始化构造开始第 {retryCount} 次重试");
                                            }).Execute(() =>
                                            {
                                                var connection = factory.CreateConnection();
                                                _channel = connection.CreateModel();
                                            });
            }
            catch (Exception ex)
            {
                logger.LogError($"rabbitmq初始化失败-{ex}");
            }

        }

        /// <summary>
        /// 推送到队列里
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey">路由键</param>
        /// <param name="message">待推送的消息</param>
        /// <param name="exchangeType">交换机类型，默认direct</param>
        public virtual void PushMessage<T>(string routingKey, T message, string exchangeType = "direct") where T : class
        {
            var modelName = $"{_configOptions.Project.Current}.{message.GetType().FullName}";
            _logger.LogError($"PushMessage,routingKey:{routingKey}");

            _logger.LogError($"PushMessage,modelName:{modelName}");
            //声明交换机
            _channel.ExchangeDeclare(exchange: modelName, type: exchangeType, durable: true);
            _channel.QueueDeclare(queue: modelName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            var body = JsonUtility.SerializeByte(message);
            var basicProperties = _channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;
            _channel.BasicPublish(exchange: modelName,
                                    routingKey: routingKey,
                                    basicProperties: basicProperties,
                                    body: body);
        }

        /// <summary>
        /// 推送到延时队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey">路由键</param>
        /// <param name="message">待推送的消息</param>
        /// <param name="delayTime">延时时间，默认30分钟（单位：秒）</param>
        public virtual void PushDelayMessage<T>(string routingKey, T message, int delayTime = 30 * 60) where T : class
        {
            var args = new Dictionary<string, object>
            {
                { "x-delayed-type", "direct" } //x-delayed-type必须加
            };
            string exchangeType = "x-delayed-message";
            var modelName = $"{_configOptions.Project.Current}.{message.GetType().FullName}";
            _logger.LogError($"PushMessage,routingKey:{routingKey}");
            _logger.LogError($"PushMessage,modelName:{modelName}");
            //声明交换机
            _channel.ExchangeDeclare(exchange: modelName, type: exchangeType, durable: true, autoDelete: false, args);
            _channel.QueueDeclare(queue: modelName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            var body = JsonUtility.SerializeByte(message);
            var basicProperties = _channel.CreateBasicProperties();
            basicProperties.DeliveryMode = 2;
            //延时时间从header赋值
            var headers = new Dictionary<string, object>
            {
                { "x-delay", delayTime * 1000 }
            };
            basicProperties.Headers = headers;
            _channel.BasicPublish(exchange: modelName,
                                    routingKey: routingKey,
                                    basicProperties: basicProperties,
                                    body: body);
        }
    }
}
