using Cloud.Core.Module;

namespace Cloud.Jwt
{
    public class CloudJwtModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;
            context.Services.AddJwtConfigure(configuration);
        }
    }
}
