using Microsoft.Extensions.DependencyInjection;
using Cloud.Core.Module;

namespace Cloud.Caching
{
    public class CloudCachingModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //缓存
            context.Services.AddSingleton(typeof(ICache), typeof(FreeRedisCache));
            //缓存配置
            context.Services.AddSingleton(typeof(ICacheConfig<>), typeof(CacheConfig<>));
        }
    }
}
