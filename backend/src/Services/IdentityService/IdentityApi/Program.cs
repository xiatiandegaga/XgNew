
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace IdentityApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
             .CreateBootstrapLogger();
            try
            {
                Log.Information("Starting web host.");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseSerilog((ctx, conf) =>
                 {
                     conf.ReadFrom.Configuration(ctx.Configuration);
                         //.WriteTo.Console()
                         //.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 15, rollOnFileSizeLimit: true);

                 })
           /*     .UseNacosConfig(section: "NacosConfig", parser: null, logAction: null)*/
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.Limits.MaxRequestBodySize = null;
                    });
                });
          


    }
}
