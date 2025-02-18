using Cloud.Caching;
using Cloud.CloudQRCode;
using Cloud.Core.Module;
using Cloud.Cors;
using Cloud.Domain;
using Cloud.Emailing.MailKit;
using Cloud.Extensions;
using Cloud.Jwt;
using Cloud.LogisticsFuQing;
using Cloud.Mapster;
using Cloud.Models.HttpClientUtility;
using Cloud.Mvc;
using Cloud.Pay.Wechat;
using Cloud.Repositories.EntityFrameworkCore;
using Cloud.Repositories.EntityFrameworkCore.PostgreSQL;
using Cloud.Services;
using Cloud.SMS.Tencent;
using Cloud.Swagger;
using Cloud.TencentCos;
using Domain.IService.Base;
using Domain.Service.Base;
using Microsoft.Extensions.DependencyInjection;
using Xg.Cloud.BankMallH5;

namespace IdentityApi
{
    [DependsOn(
     typeof(CloudBankMallH5Module),
     typeof(CloudPayWechatModule),
     typeof(CloudLogisticsFuqingModule),
     typeof(CloudMapsterModule),
     typeof(CloudCachingModule),
     typeof(CloudDomainModule),
     typeof(CloudCorsModule),
     typeof(CloudJwtModule),
     typeof(CloudMvcModule),
     typeof(CloudSwaggerModule),
     typeof(CloudEntityFrameworkCoreModule),
     typeof(CloudEntityFrameworkCorePostgreSQLModule),
     typeof(CloudEmailingMailkitModule),
     typeof(CloudQRCodeModule),
     typeof(CloudSMSTencentModule),
     typeof(CloudTencentCosModule)
     )]
    public class CloudBackendModule : AppModule
    {

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;
            context.Services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
            context.Services.AddScoped(typeof(IBaseCacheService<,>), typeof(BaseCacheService<,>));
            context.Services.AddScoped(typeof(IBaseListCacheService<,>), typeof(BaseListCacheService<,>));
            context.Services.AddHttpClient();
            context.Services.AddCloudService(configuration);
            context.Services.AddSingleton<IHttpClientService, HttpClientService>();
            context.Services.AddHttpClient<IWeChatLoginClient, WeChatLoginClient>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }

    }
}
