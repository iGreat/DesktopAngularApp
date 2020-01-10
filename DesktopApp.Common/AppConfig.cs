using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace DesktopApp.Common
{
    public static class AppConfig
    {
        private static readonly Configuration Configuration;

        static AppConfig()
        {
            Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static string ServerAddress => Configuration.AppSettings.Settings["SERVER"].Value;

        public static string ConnectionString => Configuration.ConnectionStrings.ConnectionStrings["LOCALDB"].ConnectionString;

        public static int AngularContainerPort => Convert.ToInt32(Configuration.AppSettings.Settings["ANGULARCONTAINERPORT"].Value);

        public static string AngularContainer => Path.Combine(BaseDir, Configuration.AppSettings.Settings["ANGULARCONTAINER"].Value);

        private static string BaseDir
        {
            get
            {
                var uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            }
        }
    }
}
