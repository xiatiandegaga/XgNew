using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cloud.Swagger
{
    public static class SwaggerConfigureExtensions
    {
        public static void AddSwaggerConfigure(this IServiceCollection services, string names = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly().GetName().Name}.xml"), true);

                //var modelPrefix = Assembly.GetEntryAssembly()?.GetName().Name + ".Models.";
                //c.SchemaGeneratorOptions = new SchemaGeneratorOptions { SchemaIdSelector = type => type.ToString()[(type.ToString().IndexOf("Models.") + 7)..].Replace(modelPrefix, "").Replace("`1", "").Replace("+", ".") };
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // swagger文档配置
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Cloud接口文档",
                    Description = "Cloud HTTP API v1"
                });
                // 接口排序
                c.OrderActionsBy(o => o.RelativePath);

                // 配置 xml 文档
                if (names != null)
                {
                    foreach (var name in names.Split(','))
                    {

                        if (File.Exists(Path.Combine(AppContext.BaseDirectory, name)))
                        {
                            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, name));
                        }
                    }
                }

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                   }
                });
            });
        }
    }
}
