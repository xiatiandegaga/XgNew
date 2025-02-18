using Cloud.Models;
using COSXML;
using COSXML.Auth;
using Microsoft.Extensions.DependencyInjection;
using Cloud.Core.Module;
using System;

namespace Cloud.TencentCos
{
    public class CloudTencentCosModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;
            //腾讯云cos
            try
            {
                context.Services.Configure<CosConfigOptions>(configuration.GetSection("CosConfigOptions"));
                CosXmlConfig config = new CosXmlConfig.Builder()
                   .SetRegion(configuration["CosConfigOptions:Region"])
                   .SetDebugLog(false)
                   .Build();
                var qCloudCredentialProvider = new DefaultQCloudCredentialProvider(configuration["CosConfigOptions:SecretId"], configuration["CosConfigOptions:SecretKey"], 600);
                context.Services.AddSingleton(typeof(CosXml), new CosXmlServer(config, qCloudCredentialProvider));
                context.Services.AddSingleton(typeof(CosUtility), typeof(CosUtility));
            }
            catch (Exception ex)
            {
                throw new MyException(ex.Message);
            }
        }
    }
}
