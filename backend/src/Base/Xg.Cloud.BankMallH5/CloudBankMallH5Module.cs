using Cloud.Core.Module;
using Cloud.HttpClient;
using Microsoft.Extensions.DependencyInjection;

namespace Xg.Cloud.BankMallH5
{
    public class CloudBankMallH5Module: AppModule
    {
        public static readonly string RemoteServiceName = "CloudBankMallH5";

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            try
            {
                var configuration = context.Configuration;
                context.Services.AddScoped(typeof(IBankMallH5Service), typeof(BankMallH5Service));
                //context.Services.AddHttpClientProxies(configuration, this.GetType().Assembly, RemoteServiceName, typeof(IApiService));
            }
            catch (Exception ex)
            {

            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}
