namespace Cloud.EventBus.RabbitMQ
{
    /* public record RabbitMQConnectOptions(string MQHost,string MQUserName,string MQPassword,int MQPort);*/

    public class RabbitMQConnectOptions
    {
        public string MQHost { get; set; }
        public string MQUserName { get; set; }
        public string MQPassword { get; set; }
        public int MQPort { get; set; }
        public string VHost { get; set; }

        public string QueueName { get; set; }


    }


}
