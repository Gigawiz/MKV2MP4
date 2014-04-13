using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace Mkv_2_Mp4
{
    public partial class MissingFFmpeg : Form
    {
        public MissingFFmpeg()
        {
            InitializeComponent();
        }

        private void MissingFFmpeg_Load(object sender, EventArgs e)
        {
            downloadFFMPEG();
        }
        string osver = "32";
        private void downloadFFMPEG()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                osver = "64";
            }
            try
            {
                WebClient WC = new WebClient();
                WC.DownloadFileCompleted += new AsyncCompletedEventHandler(WC_DownloadFileCompleted);
                WC.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WC_DownloadProgressChanged);
                Uri dlurl = new Uri("http://theharmfulclan.com/mkv2mp4/ffmpeg-" + osver + ".exe");
                WC.DownloadFileAsync(dlurl, "ffmpeg.exe");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void WC_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void WC_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
