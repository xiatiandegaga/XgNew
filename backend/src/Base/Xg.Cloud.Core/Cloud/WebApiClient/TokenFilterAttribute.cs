using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Cloud.WebApiClient
{
    public class TokenFilterAttribute : ApiActionAttribute
    {
        public override Task OnRequestAsync(ApiRequestContext context)
        {
            var currentHttpContext = ((IHttpContextAccessor)context.HttpContext.ServiceProvider.GetService(typeof(IHttpContextAccessor))).HttpContext;
            var token = currentHttpContext.Request.Headers["Authorization"].ToString();
            context.HttpContext.RequestMessage.Headers.Add("Authorization", token);
            return Task.CompletedTask;
        }
    }

}
