using System.Threading.Tasks;
using Desktop.Model;

namespace DesktopApp.Service
{
    public interface ILoginService
    {
        Captcha GenerateCaptcha();
        
        Task<User> DecryptToken(string token);
    }
}
