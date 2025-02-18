using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Cloud.Mvc.Filters
{
    public class AsyncAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public AsyncAuthorizationFilter(IConfiguration configuration)
        {

        }
        public Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }
            var endpoint = filterContext.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
                return Task.CompletedTask;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new UnauthorizedResult();
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
