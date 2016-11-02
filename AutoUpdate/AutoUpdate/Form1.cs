using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdate
{
    public partial class Form1 : Form
    {
        string newVersion = "";

        public Form1()
        {
            InitializeComponent();
        }

        public void DownloadUpdate()
        {
            string url = "https://www.dropbox.com/s/awu5t91jj6aken7/AutoUpdate.exe?dl=1";

            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            wc.DownloadFileAsync(new Uri(url), Application.StartupPath + "/AutoUpdate(1).exe");
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Your application is now up-to-date!\n\nThe application will now restart!", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        public void GetUpdate()
        {
            WebClient wc = new WebClient();
            string textFile = wc.DownloadString("https://www.dropbox.com/s/mtdgffxqsalidil/AutoUpdaterVersion.txt?dl=1");
            newVersion = textFile;
            label1.Text = label1.Text + Application.ProductVersion;
            label2.Text = label2.Text + newVersion;

            if (newVersion != Application.ProductVersion)
            {
                MessageBox.Show("An update is available! Click OK to download and restart!", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DownloadUpdate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetUpdate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (System.IO.File.Exists(Application.StartupPath + "/AutoUpdate(1).exe"))
            {
                System.IO.File.Move(Application.StartupPath + "/AutoUpdate.exe", Application.StartupPath + "/AutoUpdate(2).exe");
                System.IO.File.Move(Application.StartupPath + "/AutoUpdate(1).exe", Application.StartupPath + "/AutoUpdate.exe");
                System.IO.File.Delete(Application.StartupPath + "/AutoUpdate(2).exe");
            }
        }
    }
}
