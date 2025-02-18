using Cloud.Core.Module;
using Cloud.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCloudModule<CloudBackendModule>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory _scopeFactory)
        {
            DIUtility.DIApp = app;
            app.ApplicationServices.UseCloudModule();
            if (env.IsDevelopment() && Configuration["IsAddSeed"]== "true")
            {
                FakeDataSeeder.SeedAsync(_scopeFactory, Configuration);
            }
        }
    }
}
