using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace DesktopApp.Common
{
    public class AngularServe
    {
        private static readonly string[] IndexFiles =
        {
            "index.html",
            "index.htm",
            "default.html",
            "default.htm"
        };

        private static readonly IDictionary<string, string> MimeTypeMappings =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                #region extension to MIME type list

                {".asf", "video/x-ms-asf"},
                {".asx", "video/x-ms-asf"},
                {".avi", "video/x-msvideo"},
                {".bin", "application/octet-stream"},
                {".cco", "application/x-cocoa"},
                {".crt", "application/x-x509-ca-cert"},
                {".css", "text/css"},
                {".deb", "application/octet-stream"},
                {".der", "application/x-x509-ca-cert"},
                {".dll", "application/octet-stream"},
                {".dmg", "application/octet-stream"},
                {".ear", "application/java-archive"},
                {".eot", "application/octet-stream"},
                {".exe", "application/octet-stream"},
                {".flv", "video/x-flv"},
                {".gif", "image/gif"},
                {".hqx", "application/mac-binhex40"},
                {".htc", "text/x-component"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".ico", "image/x-icon"},
                {".img", "application/octet-stream"},
                {".iso", "application/octet-stream"},
                {".jar", "application/java-archive"},
                {".jardiff", "application/x-java-archive-diff"},
                {".jng", "image/x-jng"},
                {".jnlp", "application/x-java-jnlp-file"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".js", "application/x-javascript"},
                {".mml", "text/mathml"},
                {".mng", "video/x-mng"},
                {".mov", "video/quicktime"},
                {".mp3", "audio/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".msi", "application/octet-stream"},
                {".msm", "application/octet-stream"},
                {".msp", "application/octet-stream"},
                {".pdb", "application/x-pilot"},
                {".pdf", "application/pdf"},
                {".pem", "application/x-x509-ca-cert"},
                {".pl", "application/x-perl"},
                {".pm", "application/x-perl"},
                {".png", "image/png"},
                {".prc", "application/x-pilot"},
                {".ra", "audio/x-realaudio"},
                {".rar", "application/x-rar-compressed"},
                {".rpm", "application/x-redhat-package-manager"},
                {".rss", "text/xml"},
                {".run", "application/x-makeself"},
                {".sea", "application/x-sea"},
                {".shtml", "text/html"},
                {".sit", "application/x-stuffit"},
                {".svg", "application/svg+xml"},
                {".swf", "application/x-shockwave-flash"},
                {".tcl", "application/x-tcl"},
                {".tk", "application/x-tcl"},
                {".txt", "text/plain"},
                {".war", "application/java-archive"},
                {".wbmp", "image/vnd.wap.wbmp"},
                {".wmv", "video/x-ms-wmv"},
                {".xml", "text/xml"},
                {".xpi", "application/x-xpinstall"},
                {".zip", "application/zip"},

                #endregion
            };

        private readonly Thread _serverThread;
        private readonly string _rootDirectory;
        private readonly HttpListener _listener;
        private readonly int _port;

        public AngularServe()
        {
            _rootDirectory = AppConfig.AngularContainer;
            if (!Directory.Exists(_rootDirectory))
            {
                AppLog.Info("Angular应用尚未准备完成.");
                return;
            }

            _port = AppConfig.AngularContainerPort;
            _listener = new HttpListener();

            _serverThread = new Thread(Listen);
            _serverThread.Start();
        }

        public void Stop()
        {
            AppLog.Info("停止angular容器.");

            _listener?.Abort();
            _serverThread?.Abort();
        }

        private void Listen()
        {
            if (!HttpListener.IsSupported)
                throw new System.Exception("不支持httplistener");

            if (IsPortInUse(_port))
            {
                AppLog.Info($"端口{_port}被其他应用占用");
                return;
            }

            _listener.Prefixes.Add($"http://127.0.0.1:{_port}/");
            _listener.Start();
            AppLog.Info("开启Angular容器.");

            while (true)
            {
                try
                {
                    if (_serverThread != null && _serverThread.IsAlive)
                        ThreadPool.QueueUserWorkItem(Process, _listener.GetContext());
                }
                catch (System.Exception ex)
                {
                    AppLog.Error(ex);
                }
            }
        }

        private void Process(object o)
        {
            if (o is HttpListenerContext context)
            {
                var filename = System.Web.HttpUtility.UrlDecode(context.Request.Url.AbsolutePath, Encoding.UTF8);
                filename = filename.Substring(1);

                if (string.IsNullOrEmpty(filename) || !HasServeFile(filename))
                {
                    foreach (var indexFile in IndexFiles)
                    {
                        if (File.Exists(Path.Combine(_rootDirectory, indexFile)))
                        {
                            filename = indexFile;
                            break;
                        }
                    }
                }

                filename = Path.Combine(_rootDirectory, filename);

                if (File.Exists(filename))
                {
                    try
                    {
                        using (var fs = File.Open(filename, FileMode.Open, FileAccess.ReadWrite))
                        {
                            context.Response.ContentType =
                                MimeTypeMappings.TryGetValue(Path.GetExtension(filename), out var mime)
                                    ? mime
                                    : "application/octet-stream";

                            context.Response.ContentEncoding = Encoding.UTF8;
                            context.Response.ContentLength64 = fs.Length;
                            context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                            context.Response.AddHeader("Last-Modified",
                                File.GetLastWriteTime(filename).ToString("r"));
                            var buffer = new byte[1024 * 16];
                            int nbytes;
                            while ((nbytes = fs.Read(buffer, 0, buffer.Length)) > 0)
                                context.Response.OutputStream.Write(buffer, 0, nbytes);

                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            context.Response.OutputStream.Flush();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        AppLog.Error(ex);
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }

                context.Response.OutputStream.Close();
            }
        }

        private bool HasServeFile(string filename)
        {
            var file = Path.Combine(_rootDirectory, filename);

            return File.Exists(file);
        }

        private bool IsPortInUse(int port)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            return tcpConnInfoArray.Any(i => i.LocalEndPoint.Port == port);
        }
    }
}
