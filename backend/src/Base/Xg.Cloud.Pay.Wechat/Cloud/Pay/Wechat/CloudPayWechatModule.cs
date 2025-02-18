using Cloud.Core.Module;
using Cloud.Utilities;
using Essensoft.Paylink.WeChatPay;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Pay.Wechat
{
    public class CloudPayWechatModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Configuration;
            // 添加Paylink依赖注入
            context.Services.AddWeChatPay();
            context.Services.Configure<WeChatPayOptions>(configuration.GetSection("WeChatPay"));
        }

        public override void OnApplicationInitialization(
          ApplicationInitializationContext context)
        {
        
        }
    }
}
