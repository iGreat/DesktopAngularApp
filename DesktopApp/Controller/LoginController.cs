using DesktopApp.Service;
using System.Web.Http;
using Desktop.Model;

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

        [Route("captcha")]
        [HttpGet]
        public Captcha GenerateCaptcha()
        {
            return _loginService.GenerateCaptcha();
        }

        [HttpPost]
        [Route("validateToken")]
        public bool ValidToken(object data)
        {
            return false;
        }
    }
}