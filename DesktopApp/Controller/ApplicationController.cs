using System.Web.Http;

namespace DesktopApp.Controller
{
    [RoutePrefix("app")]
    [AllowAnonymous]
    public class ApplicationController : ApiController
    {
        [HttpGet]
        [Route("quit")]
        public void QuitApp()
        {

        }
    }
}
