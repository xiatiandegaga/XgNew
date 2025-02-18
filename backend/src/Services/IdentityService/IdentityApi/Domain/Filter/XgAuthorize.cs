using Domain.IService.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Filter
{
    public class XgAuthorize : ActionFilterAttribute
    {
        /// <summary>
        /// 权限字符串，例如 organization:user:view
        /// </summary>
        public readonly string _permissions;

        public XgAuthorize(string permissions)
        {
            _permissions = permissions;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_permissions != string.Empty && context.HttpContext.User.Identity.IsAuthenticated)
            {
                var _menuService = context.HttpContext.RequestServices.GetService<IMenuService>();
                var menu =await  _menuService.GetUserMenusAsync();
                if (!menu.Any(x=>x.Permission==_permissions))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            await next();
        }

    }
}
