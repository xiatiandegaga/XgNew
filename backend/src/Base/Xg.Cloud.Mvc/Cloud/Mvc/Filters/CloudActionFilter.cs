using Cloud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using Xg.Cloud.Core;

namespace Cloud.Mvc.Filters
{
    public class CloudActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<CloudActionFilter> _logger;
        public CloudActionFilter(ILogger<CloudActionFilter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 包装返回结果
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionWrapper = controllerActionDescriptor?.MethodInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();
            var controllerWrapper = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();
            //如果包含NoWrapperAttribute则说明不需要对返回结果进行包装，直接返回原始值
            if (actionWrapper != null || controllerWrapper != null)
            {
                return;
            }
            var rspResult = new AjaxResponseGen<object>
            {
                Code = CommonConst.Ajax_Success
            };
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                //AjaxResponseGen<T>类型的则不需要进行再次包装了
                if (objectResult.DeclaredType.IsGenericType && objectResult.DeclaredType?.GetGenericTypeDefinition() == typeof(AjaxResponseGen<>))
                {
                    return;
                }
                rspResult.Data = objectResult.Value;
            }
            if (context.Result is UnauthorizedResult)
            {
                return;
            }
            context.Result = new ObjectResult(rspResult);
            return;
        }
    }
}
