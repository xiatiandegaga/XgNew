using Microsoft.Extensions.DependencyInjection;
using Cloud.Core.Module;

namespace Cloud.SMS.Tencent
{
    [DependsOn(typeof(CloudSMSModule))]
    public class CloudSMSTencentModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(typeof(ISendVerifySms), typeof(TencentSms));
        }
    }
}
