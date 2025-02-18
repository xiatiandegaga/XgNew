using Cloud.Core.Module;
using Cloud.WebApiClient;
using Identity.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.Shared
{
    public class IdentitySharedModule : AppModule
    {
        public static readonly  string RemoteServiceName = "Identity";
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}
