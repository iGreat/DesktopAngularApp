using System;
using System.Threading.Tasks;
using Desktop.Model;
using DesktopApp.Common;
using DesktopApp.Common.Attribute;
using DesktopApp.Common.Util;

namespace DesktopApp.Service.Provider
{
    [UnityService(typeof(ILoginService))]
    public class LoginService : BaseService, ILoginService
    {
        public LoginService() : base(mapperConfig =>
        {
            #region auto mapper

            #endregion
        })
        {
        }

        public Captcha GenerateCaptcha()
        {
            const string dicString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            var random = new Random();
            var text = new char[4];
            for (var i = 0; i < 4; ++i)
            {
                text[i] = dicString[random.Next(0, dicString.Length - 1)];
            }

            var captchaId = Guid.NewGuid();

            MemoryCacheUtil.SetItem(captchaId.ToString(), text);

            return new Captcha
            {
                Id = captchaId,
                Image = Convert.ToBase64String(CaptchaUtil.CreateCaptcha(new string(text)))
            };
        }

        public Task<User> DecryptToken(string token)
        {
            return null;
        }
    }
}