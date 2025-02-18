using Cloud.Core.Module;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cloud.CustomPolly
{
    internal class CloudCustomPollyModule : AppModule
    {

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            RegisterServices(this.GetType().Assembly, context.Services);

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }

        private static void RegisterServices(Assembly asm, IServiceCollection services)
        {
            foreach (var type in asm.GetExportedTypes())
            {
                bool hasPollyCommand = type.GetMethods().Any(m =>
                    m.GetCustomAttribute(typeof(PollyCommandAttribute)) != null);
                if (hasPollyCommand)
                {
                    services.AddSingleton(type);
                }
            }
        }
    }
}
