using Cloud.Core.Module;

namespace Cloud.Cors
{
    public class CloudCorsModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddCloudCors(context.Configuration);
        }
    }
}
