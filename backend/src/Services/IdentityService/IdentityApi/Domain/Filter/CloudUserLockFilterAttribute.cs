using Cloud.Models;
using Domain.IService.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xg.Cloud.Core;

namespace Domain.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CloudUserLockFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cloudLock = context.HttpContext.RequestServices.GetRequiredService<ICloudLock>();
            var authUser = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationPrincipalService>();
            var controllerName = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;
            var actionName = context.RouteData.Values["action"]?.ToString() ?? string.Empty;
            var lockKey = $"{await authUser.GetAuthenticatedUserIdAsync()}:{controllerName}:{actionName}";
            var isLocked = await cloudLock.TryAdd(lockKey);
            if (!isLocked)
            {
                throw new MyException("操作过于频繁！");
            }
            try
            {
                await next();
            }
            finally
            {
                await cloudLock.TryRemove(lockKey);
            }
        }
    }
}
