using Cloud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cloud.Mvc.Filters
{
    public class AsyncExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<AsyncExceptionFilter> _logger;
        public AsyncExceptionFilter(ILogger<AsyncExceptionFilter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 发生异常时，跳转至异常信息显示页
        /// </summary>
        /// <param name="filterContext"></param>
        public Task OnExceptionAsync(ExceptionContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (filterContext.ExceptionHandled)
                return Task.CompletedTask;

            Exception exception = filterContext.Exception;
            //exception.ToExceptionless().Submit();
            if (exception.GetType() == typeof(MyException))
            {
                var myException = exception as MyException;
                if (myException._isLog == 1)
                    _logger.LogError(exception, "AsyncExceptionFilter");
                filterContext.Result = new ObjectResult(new AjaxResponseGen { Msg = exception.Message });
                filterContext.ExceptionHandled = true;
                return Task.CompletedTask;
            }
            else
            {
                //记录异常
                _logger.LogError(exception, "AsyncExceptionFilter");
            }
            //过滤异步请求
            if (filterContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                filterContext.Result = new ObjectResult(new AjaxResponseGen { Msg = "发生异常，请联系管理员！" });
                filterContext.ExceptionHandled = true;
                return Task.CompletedTask;
            }
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
            return Task.CompletedTask;
        }
    }
}
