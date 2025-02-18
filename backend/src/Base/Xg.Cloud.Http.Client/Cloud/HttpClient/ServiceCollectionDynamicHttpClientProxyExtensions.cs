using Cloud.Utilities;
using Cloud.WebApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Cloud.HttpClient
{
    public static class ServiceCollectionDynamicHttpClientProxyExtensions
    {
        public static IServiceCollection AddHttpClientProxies(
           [NotNull] this IServiceCollection services,
           [NotNull] IConfiguration configuration,
           [NotNull] Assembly assembly,
           [NotNull] string remoteServiceConfigurationName,
            Type type=null
           )
        {
            CheckUtility.NotNull(services, nameof(assembly));
            if (type == null)
                type = typeof(ICloudRemoteService);
            var serviceTypes = assembly.GetTypes().Where(x=>IsSuitableForDynamicClientProxying(x, type)).ToArray();

            foreach (var serviceType in serviceTypes)
            {
              services.AddHttpApi(serviceType,options => {
                    options.HttpHost = new Uri(configuration[$"RemoteServices:{remoteServiceConfigurationName}:BaseUrl"]);
                    JsonConfigInit.InitJsonSerializeOptions()(options);
                });
            }

            return services;
        }

        private static bool IsSuitableForDynamicClientProxying(Type type,Type sourceType)
        {
            //TODO: Add option to change type filter

            return type.IsInterface
                && type.IsPublic
                && !type.IsGenericType
                && sourceType.IsAssignableFrom(type);
        }
    }
}
