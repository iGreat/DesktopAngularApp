using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DesktopApp.Common;
using DesktopApp.Common.Exception;
using DesktopApp.Service;

namespace DesktopApp.Config
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ApiAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly ILoginService _loginService;

        public ApiAuthorizeAttribute(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            await OnAuthorization(actionContext);
            
            return actionContext.Response ?? await continuation();
        }

        private async Task OnAuthorization(HttpActionContext actionContext)
        {
            var controllerDescriptor = actionContext.ControllerContext.ControllerDescriptor;
            var actionDescriptor = actionContext.ActionDescriptor;
            
            if(controllerDescriptor
                   .GetCustomAttributes<AllowAnonymousAttribute>()
                   .Any()
               && ! actionDescriptor
                   .GetCustomAttributes<ApiAuthorizeAttribute>()
                   .Any()
               || actionDescriptor
                   .GetCustomAttributes<AllowAnonymousAttribute>()
                   .Any())
                return;
            
            if(!await IsAuthorized())
                HandleUnauthorizedRequest();
        }

        private async Task<bool> IsAuthorized()
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["ASPXAUTH"];
                if (string.IsNullOrWhiteSpace(token))
                    return false;

                var user = await _loginService.DecryptToken(token);
                return user.IsValid;
            }
            catch (Exception e)
            {
                AppLog.Error(e);
                return false;
            }
        }

        private void HandleUnauthorizedRequest()
        {
            throw new CustomException("你没有访问权限");
        }
    }
}