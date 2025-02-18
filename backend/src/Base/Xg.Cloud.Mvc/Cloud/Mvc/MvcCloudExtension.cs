using Cloud.Mvc.Filters;
using Cloud.Utilities.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Panda.DynamicWebApi;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloud.Mvc
{
    public class GroupNameConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var groupName = controllerNamespace!.Split('.').LastOrDefault();
            controller.ApiExplorer.GroupName = groupName;
        }
    }

    public static class MvcCloudExtension
    {
        public static void AddCloudMvc(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddControllers(options =>
            {
                options.Filters.Clear();
                options.Filters.Add<AsyncExceptionFilter>();
                options.Filters.Add<AsyncAuthorizationFilter>();
                options.Filters.Add<CloudActionFilter>();
                options.Conventions.Add(new GroupNameConvention());

            }).AddJsonOptions(options =>
            {
                JsonUtility.InitJsonOptions(options.JsonSerializerOptions);
            });
            services.AddDynamicWebApi(options =>
            {
                // 指定全局默认的 api 前缀
                options.DefaultApiPrefix = "api";
                // 指定全局默认的去除控制器后缀
                options.RemoveControllerPostfixes = new List<string>() { "App" };
                // 清空API结尾，不删除API结尾；若不清空 CreatUserAsync 将变为 CreateUser
                options.RemoveActionPostfixes.Clear();
                // 自定义 ActionName 处理函数;
                options.GetRestFulActionName = (actionName) => actionName;
            });
           services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
