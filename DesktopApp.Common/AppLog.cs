using NLog;
using NLog.Config;
using NLog.Targets;
using System.Text;

namespace DesktopApp.Common
{
    public static class AppLog
    {
        private static readonly ILogger Logger;

        static AppLog()
        {
            var fileTarget = new FileTarget("file_log")
            {
                Encoding = Encoding.UTF8,
                LineEnding = LineEndingMode.CRLF,
                Layout = "${longdate} ${level} ${message} ${exception}",
                FileName = "${basedir}/Logs/event.log",
                MaxArchiveFiles = 7,
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Date
            };

            var config = new LoggingConfiguration();
            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(fileTarget);

            var consoleTarget = new ConsoleTarget("console_log")
            {
                Layout = "${longdate} ${level} ${message} ${exception}",
                AutoFlush = true,
                Encoding = Encoding.UTF8
            };
            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);

            LogManager.Configuration = config;
            Logger = LogManager.GetLogger("DesktopDemo");
        }

        public static void Error(object error)
        {
            Logger.Error(error);
        }

        public static void Info(object info)
        {
            Logger.Info(info);
        }
    }
}
