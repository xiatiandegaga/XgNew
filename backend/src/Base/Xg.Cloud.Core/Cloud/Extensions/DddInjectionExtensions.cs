using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Linq;
using System.Reflection;
using Xg.Cloud.Core;

namespace Cloud.Extensions
{
    public static class DddInjectionExtensions
    {
        public static void AddCloudApplication(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            //Assembly[] assemblies = AssemblyUtility.GetEntryAssembly(n => n.Name.EndsWith("Domain") || n.Name.EndsWith("Application"));
            //var lstDomainService = new List<TypeInfo>();
            //assemblies.Where(t => t.FullName.Contains("Domain")).Select(t => t.DefinedTypes)?.ToList().ForEach(
            //    x =>
            //    {
            //        var types = x.Where(t => t.Name.EndsWith("Service") && !t.IsInterface && !t.IsAbstract);
            //        if (types != null && types.Count() != 0)
            //        {
            //            lstDomainService.AddRange(types);
            //        }
            //    });
            //if (lstDomainService != null)
            //{
            //    lstDomainService.ForEach(t =>
            //    {
            //        services.AddScoped(t.ImplementedInterfaces.First(), t);
            //    });
            //}

            //var lstAppService = new List<TypeInfo>();
            //assemblies.Where(t => t.FullName.Contains("Application")).Select(t => t.DefinedTypes)?.ToList().ForEach(
            //       x =>
            //       {
            //           var types = x.Where(t => t.Name.EndsWith("App") && !t.IsInterface && !t.IsAbstract).ToList();
            //           if (types != null && types.Count() != 0)
            //           {
            //               lstAppService.AddRange(types);
            //           }
            //       });
            //if (lstAppService != null)
            //{
            //    lstAppService.ForEach(t =>
            //    {
            //        services.AddScoped(t, t);
            //    });
            //}

            var currentAssembly = Assembly.GetCallingAssembly().GetName();
            var assembly = Assembly.Load(currentAssembly);
            var mappingAppInterface = typeof(ICloudApp);
            var mappingAppTypes = assembly.GetTypes()
                  .Where(x => x.GetInterfaces().Any(y => y == mappingAppInterface) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType).ToList();
            if (mappingAppTypes != null)
            {
                mappingAppTypes.ForEach(t =>
                {
                    services.AddScoped(t, t);
                });
            }
        }

        public static void AddCloudService(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            var currentAssembly = Assembly.GetCallingAssembly().GetName();
            var assembly = Assembly.Load(currentAssembly);
            var mappingServiceInterface = typeof(ICloudService);
            var mappingServiceTypes = assembly.GetTypes()
                  .Where(x => x.GetInterfaces().Any(y => y == mappingServiceInterface) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType).ToList();
            if (mappingServiceTypes != null)
            {
                mappingServiceTypes.ForEach(t =>
                {
                    services.AddScoped(t.GetTypeInfo().ImplementedInterfaces.Where(x=>!x.IsGenericType && x!= mappingServiceInterface).First(), t);
                });
            }
        }
    }
}
