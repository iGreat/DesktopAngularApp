using DesktopApp.Common.Attribute;

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
    }
}
