using Cloud.Core.Module;
using Cloud.LogisticsFuQing.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Cloud.LogisticsFuQing
{
    public class CloudLogisticsFuqingModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<LogisticsConfigOptions>(context.Configuration.GetSection("LogisticsConfigOptions"));
            context.Services.AddSingleton<ILogisticsQuery, FuQingLogisticsQuery>();
            context.Services.AddSingleton<ILogisticsQuery, FuQingLogisticsQuery>();
        }
    }
}
