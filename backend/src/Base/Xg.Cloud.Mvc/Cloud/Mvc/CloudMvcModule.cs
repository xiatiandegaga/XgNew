using Cloud.Core.Module;
using Cloud.Utilities;
using Microsoft.AspNetCore.Builder;

namespace Cloud.Mvc
{
    public class CloudMvcModule : AppModule
    {

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddCloudMvc();
        }

        public override void OnApplicationInitialization(
          ApplicationInitializationContext context)
        {
            var app = DIUtility.DIApp;

            app.UseRouting();

            app.UseCors("Cors");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
