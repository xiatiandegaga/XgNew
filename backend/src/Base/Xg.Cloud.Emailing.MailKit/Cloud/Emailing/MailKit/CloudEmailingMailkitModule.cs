using Microsoft.Extensions.DependencyInjection;
using Cloud.Core.Module;

namespace Cloud.Emailing.MailKit
{
    [DependsOn(typeof(CloudEmailingModule))]
    public class CloudEmailingMailkitModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(typeof(ISendMail), typeof(SendMail));
        }
    }
}
