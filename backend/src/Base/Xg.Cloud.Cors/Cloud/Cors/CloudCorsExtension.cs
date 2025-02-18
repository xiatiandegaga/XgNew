using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cloud.Cors
{
    public static class CloudCorsExtension
    {
        public static void AddCloudCors(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(c =>
            {
                c.AddPolicy("Cors", p =>
                {
                    p
                    .WithOrigins(configuration["Cors:Origins"].Split(','))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()//允许跨域cookie
                    .SetPreflightMaxAge(TimeSpan.FromHours(24));
                });
            });
        }
    }
}
