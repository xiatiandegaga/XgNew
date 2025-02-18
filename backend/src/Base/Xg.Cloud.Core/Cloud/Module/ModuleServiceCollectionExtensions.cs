using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Cloud.Core.Module
{
    /// <summary>
    /// 模块服务扩展
    /// </summary>
    public static class ModuleServiceCollectionExtensions
    {
        /// <summary>
        /// 添加模块服务
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="moduleOptionsConfiguration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCloudModule<TModule>(this IServiceCollection services, IConfiguration configuration, Action<ModuleOptions> moduleOptionsConfiguration = null)
            where TModule : IAppModule
        {
            // 模块配置
            var moduleOptions = new ModuleOptions();
            moduleOptionsConfiguration?.Invoke(moduleOptions);

            // 创建模块管理器
            var moduleManager = new ModuleManager(moduleOptions);

            // 启动模块
            moduleManager.StartModule<TModule>(services);

            // 配置服务
            moduleManager.ConfigurationService(services, configuration);


            services.TryAddSingleton<IModuleManager>(moduleManager);
            return services;
        }
    }
}
