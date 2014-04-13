using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace Mkv_2_Mp4
{
    public partial class UpdateFound : Form
    {
        int progver = 0;
        public UpdateFound(int progvers)
        {
            InitializeComponent();
            progver = progvers + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("updater.exe");
            Environment.Exit(0);
        }

        private void UpdateFound_Load(object sender, EventArgs e)
        {
            string newsurl = "http://theharmfulclan.com/mkv2mp4/updatechangelogs/updv"+ progver.ToString() +".txt";
            var client = new WebClient();
            client.DownloadStringCompleted += (Sender, ec) =>
            {
                string[] newslist = ec.Result.Split('~');
                foreach (string newsitem in newslist)
                {
                    listBox1.Items.Add(newsitem);
                }
            };

            client.DownloadStringAsync(new Uri(newsurl));
        }
    }
}
