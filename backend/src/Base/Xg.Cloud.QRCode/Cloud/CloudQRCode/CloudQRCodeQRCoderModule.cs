using Microsoft.Extensions.DependencyInjection;
using Cloud.Core.Module;

namespace Cloud.CloudQRCode
{
    public class CloudQRCodeModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(typeof(ICloudQRCode), typeof(CloudQRCode));
        }
    }
}
