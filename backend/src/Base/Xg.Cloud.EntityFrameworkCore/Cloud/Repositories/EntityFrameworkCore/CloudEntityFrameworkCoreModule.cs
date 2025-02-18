using Cloud.Snowflake;
using Microsoft.Extensions.DependencyInjection;
using Cloud.Core.Module;

namespace Cloud.Repositories.EntityFrameworkCore
{
    public class CloudEntityFrameworkCoreModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            //常规仓储
            context.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            //实体缓存仓储
            context.Services.AddScoped(typeof(ICacheRepository<>), typeof(BaseCacheRepository<>));
            //list缓存仓储
            context.Services.AddScoped(typeof(IListCacheRepository<>), typeof(BaseListCacheRepository<>));
            //Snowflake
            context.Services.AddSingleton(typeof(ISnowflakeIdWorker), new SnowflakeIdWorker(0, 0));
        }
    }
}
