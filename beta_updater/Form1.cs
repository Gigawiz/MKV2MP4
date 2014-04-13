using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace beta_updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WebClient DLUPD = new WebClient();
            DLUPD.DownloadFileCompleted += new AsyncCompletedEventHandler(DLUPD_DownloadFileCompleted);
            DLUPD.DownloadFileAsync(new Uri("http://theharmfulclan.com/mkv2mp4/mkv2mp4_beta.exe"), "Mkv 2 Mp4.exe");
        }

        void DLUPD_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start("Mkv 2 Mp4.exe");
            Environment.Exit(0);
        }
    }
}
