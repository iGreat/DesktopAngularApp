using DesktopApp.Service;
using System.Web.Http;

namespace DesktopApp.Controller
{
    [RoutePrefix("login")]
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("validateToken")]
        public bool ValidToken(object data)
        {
            return false;
        }
    }
}