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
    	//Declare global variable(s)
        string newVersion = "";

        public Form1()
        {
            InitializeComponent();
        }

        //Create method to execute the update download
        public void DownloadUpdate()
        {
        	//URL of the updated file
            string url = "https://www.dropbox.com/s/awu5t91jj6aken7/AutoUpdate.exe?dl=1";

            //Declare new WebClient object
            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            wc.DownloadFileAsync(new Uri(url), Application.StartupPath + "/AutoUpdate(1).exe");
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
        	//Show a message when the download has completed
            MessageBox.Show("Your application is now up-to-date!\n\nThe application will now restart!", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }

        //Create method to check for an update
        public void GetUpdate()
        {
        	//Declare new WebClient object
            WebClient wc = new WebClient();
            string textFile = wc.DownloadString("https://www.dropbox.com/s/mtdgffxqsalidil/AutoUpdaterVersion.txt?dl=1");
            newVersion = textFile;
            label1.Text = label1.Text + Application.ProductVersion;
            label2.Text = label2.Text + newVersion;

            //If there is a new version, call the DownloadUpdate method
            if (newVersion != Application.ProductVersion)
            {
                MessageBox.Show("An update is available! Click OK to download and restart!", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DownloadUpdate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        	//Check for the update
            GetUpdate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        	//This renames the original file so any shortcut works and names it accordingly after the update
            if (System.IO.File.Exists(Application.StartupPath + "/AutoUpdate(1).exe"))
            {
                System.IO.File.Move(Application.StartupPath + "/AutoUpdate.exe", Application.StartupPath + "/AutoUpdate(2).exe");
                System.IO.File.Move(Application.StartupPath + "/AutoUpdate(1).exe", Application.StartupPath + "/AutoUpdate.exe");
                System.IO.File.Delete(Application.StartupPath + "/AutoUpdate(2).exe");
            }
        }
    }
}
