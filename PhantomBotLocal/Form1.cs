using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.IO;
using Newtonsoft.Json;
namespace PhantomBotLocal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static ChromiumWebBrowser panel;
        static ChromiumWebBrowser player;

        private void Form1_Load(object sender, EventArgs e)
        {
            var Settings = new settings();
            Boolean setsLoaded = false;
            if (Directory.Exists("./cache")){
                if (File.Exists("./cache/credentials.json"))
                {
                    Settings = JsonConvert.DeserializeObject<settings>(File.ReadAllText("./cache/credentials.json"));
                    setsLoaded = true;
                }
            }
            if (!setsLoaded) //Settings were not loaded.
            {
                Settings.username = Microsoft.VisualBasic.Interaction.InputBox("What is your Username for PhantomBot?", "Username", "Username", -1, -1);
                Settings.password = Microsoft.VisualBasic.Interaction.InputBox("What is your Password for PhantomBot?", "Password", "Password", -1, -1);
                Settings.phantomboturl = Microsoft.VisualBasic.Interaction.InputBox("What is the URL for Phantombot? EG https://examplesite.com:25000", "URL", "", -1, -1);
                File.WriteAllText("./cache/credentials.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));
                MessageBox.Show("If nothing loads, delete or edit the file in ./cache, called \"credentials.json\"");
            }

            Cef.Initialize(new CefSettings { CachePath = "./cache/", PersistSessionCookies = true });
            panel = new ChromiumWebBrowser(Settings.phantomboturl + "/panel");
            panel.RequestHandler = new MyRequestHandler(Settings.username, Settings.password);
            panel.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(panel);
            player = new ChromiumWebBrowser(Settings.phantomboturl + "/ytplayer");
            player.RequestHandler = new MyRequestHandler(Settings.username, Settings.password);
            player.Dock = DockStyle.Fill;
            tabPage2.Controls.Add(player);
            this.WindowState = FormWindowState.Maximized;


        }

        class settings
        {
            public string username;
            public string password;
            public string phantomboturl;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

    }
}