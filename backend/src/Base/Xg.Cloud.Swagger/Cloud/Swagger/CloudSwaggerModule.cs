using Cloud.Core.Module;
using Cloud.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Cloud.Swagger
{
    public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiDescriptionGroupCollectionProvider provider;


        public SwaggerConfigureOptions(IApiDescriptionGroupCollectionProvider provider) => this.provider = provider;


        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiDescriptionGroups.Items)
            {
                options.SwaggerDoc(description.GroupName, null);
            }
        }

    }
    public class CloudSwaggerModule : AppModule
    {
        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
            context.Services.AddSwaggerConfigure("Domain.xml,Application.xml,IdentityApi.xml,Identity.Shared.xml,Xg.Cloud.Core.xml");
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = DIUtility.DIApp;
            var env = context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                    });
                });
                app.UseSwaggerUI(options =>
                {
                    var apiDescriptionGroups = context.ServiceProvider.GetRequiredService<IApiDescriptionGroupCollectionProvider>().ApiDescriptionGroups.Items;
                    foreach (var description in apiDescriptionGroups)
                    {
                        options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName);
                    }
                    options.EnablePersistAuthorization();
                });
            }
            else if (env.IsStaging())
            {
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}" } };
                    });
                });
                app.UseSwaggerUI(options =>
                {
                    var apiDescriptionGroups = context.ServiceProvider.GetRequiredService<IApiDescriptionGroupCollectionProvider>().ApiDescriptionGroups.Items;
                    foreach (var description in apiDescriptionGroups)
                    {
                        options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName);
                    }
                    options.EnablePersistAuthorization();
                });
            }
            else
            {

            };
        }
    }
}
