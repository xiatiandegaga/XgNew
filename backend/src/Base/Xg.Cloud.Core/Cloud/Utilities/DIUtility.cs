using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cloud.Utilities
{
    public class DIUtility
    {
        public static IServiceProvider DIServiceProvider;

        public static IServiceCollection DIService;

        public static IApplicationBuilder DIApp;
    }
}
