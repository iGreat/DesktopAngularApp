using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class MainForm : Form
    {
        public static NotifyIcon NotifyIcon { get; }

        static MainForm()
        {
            NotifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "混合桌面",
                ContextMenu = new ContextMenu(new[]
                {
                    new MenuItem("打开应用界面", (s, e) => { OpenAngularApp(); }),
                    new MenuItem("退出程序", (s, e) => { QuitApp(); })
                }),
                Visible = false
            };
            NotifyIcon.MouseClick += (s, e) => { OpenAngularApp(); };
        }

        public MainForm()
        {
            Visible = false;
            InitializeComponent();
            NotifyIcon.Visible = true;
        }

        public static void OpenAngularApp()
        {

        }

        public static void QuitApp()
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OpenAngularApp();
        }
    }
}