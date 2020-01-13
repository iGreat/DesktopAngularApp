using DesktopApp.Common;
using DesktopApp.Common.Exception;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using DesktopApp.Config;
using Unity.AspNet.WebApi;

namespace DesktopApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = new UnityDependencyResolver(UnityConfig.Container)
            };

            RegisterWebApi(config);

            appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.UseWebApi(config);

            config.EnsureInitialized();
        }

        public void RegisterWebApi(HttpConfiguration configuration)
        {
            configuration.Services.Replace(typeof(IExceptionHandler), new AppExceptionHandler());

            configuration.MapHttpAttributeRoutes();

            configuration.Formatters.JsonFormatter
                .SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

    }

    public class AppExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            if (context.Exception is CustomException ce)
            {
                AppLog.Info(ce);

                context.Result = new ResponseMessageResult(new HttpResponseMessage
                {
                    Content = new StringContent(ce.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            else
            {
                AppLog.Error(context.Exception);

                context.Result = new ResponseMessageResult(new HttpResponseMessage
                {
                    Content = new StringContent("对不起发生了错误，请检查日志"),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }

            return Task.FromResult(0);
        }
    }
}
