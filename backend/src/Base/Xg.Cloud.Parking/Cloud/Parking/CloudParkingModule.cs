using Cloud.Core.Module;
using Cloud.HttpClient;
using Cloud.Parking.KeTuo;
using Microsoft.Extensions.Logging;

namespace Cloud.Parking
{
    public class CloudParkingModule : AppModule
    {
        public static readonly string RemoteServiceName = "Parking";

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {

            var configuration = context.Configuration;
            context.Services.AddHttpClientProxies(configuration, this.GetType().Assembly, RemoteServiceName, typeof(IKeTuoApiService));

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}
