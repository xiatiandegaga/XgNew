using Microsoft.Extensions.DependencyInjection;
using Cloud.Core.Module;

namespace Cloud.EventBus.RabbitMQ
{
    [DependsOn(typeof(CloudEventBusModule))]
    public class CloudEventBusRabbitMQModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(typeof(IMQPublish), typeof(RabbitMQPublish));
            context.Services.Configure<RabbitMQConnectOptions>(context.Configuration.GetSection("RabbitMQConnectOptions"));

        }
    }
}
