using DesktopApp.Common;
using Microsoft.Owin.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                Application.ThreadException += OnThreadException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                StartServer();
                StartAngularContainer();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                AppLog.Error(e);
            }
            finally
            {
                ClearAppIcon();
                StopServer();
                StopAngularContainer();
            }
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AppLog.Error(e.Exception);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AppLog.Error(e.ExceptionObject);
        }

        private static void ClearAppIcon()
        {
            var notifyIcon = MainForm.NotifyIcon;

            if (notifyIcon == null)
                return;

            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }

        #region WEB API SERVER

        private static IDisposable LocalServer { get; set; }

        public static async void StartServer()
        {
            await Task.Run(() =>
            {
                LocalServer = WebApp.Start<Startup>(AppConfig.ServerAddress);
                AppLog.Info($"WEBAPI服务器启动:{AppConfig.ServerAddress}");
            });
        }

        public static async void StopServer()
        {
            await Task.Run(() =>
            {
                LocalServer?.Dispose();
                AppLog.Info("WEBAPI服务器关闭.");
            });
        }

        #endregion

        #region ANGULAR CONTAINER

        private static AngularServe AngularServe { get; set; }

        public static async void StartAngularContainer()
        {
            await Task.Run(() =>
            {
                AngularServe = new AngularServe();
            });
        }

        public static async void StopAngularContainer()
        {
            await Task.Run(() =>
            {
                AngularServe?.Stop();
            });
        }

        #endregion
    }
}