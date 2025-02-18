using Microsoft.Extensions.DependencyInjection;
using System;


namespace Cloud.Core.Module
{
    public static class ModuleIApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用Module
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseCloudModule(this IServiceProvider serviceProvider)
        {
            var moduleManager = serviceProvider.GetRequiredService<IModuleManager>();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                moduleManager.ApplicationShutdown();
            };


            return moduleManager.ApplicationInitialization(serviceProvider);
        }
    }
}
