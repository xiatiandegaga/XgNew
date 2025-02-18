using Cloud.Core.Module;
using Cloud.Utilities;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Nacos.V2.Naming.Dtos;
using System.Reflection;

namespace Cloud.Mapster
{
    public class CloudMapsterModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            var assemblies = AssemblyUtility.GetEntryAssembly(n => n.Name.EndsWith("Application")).ToList();
            assemblies.Add(Assembly.GetEntryAssembly());
            config.Scan(assemblies.ToArray());
            context.Services.AddSingleton(config);
            context.Services.AddSingleton<IMapper, ServiceMapper>();
            //var assemblies = AssemblyUtility.GetEntryAssembly(n => n.Name.EndsWith("Application")).ToList();
            //assemblies.Add(Assembly.GetEntryAssembly());
            //var arrProfile = assemblies.Select(t => t.DefinedTypes).First().Where(t => t.IsSubclassOf(typeof(Profile)) && !t.IsInterface && !t.IsAbstract).ToArray();
            //context.Services.AddAutoMapper(arrProfile);
        }

        public override void OnApplicationInitialization(
          ApplicationInitializationContext context)
        {
            var app = DIUtility.DIApp;
            MapsterHelper.serviceProvider = app.ApplicationServices;
        }
    }
}
