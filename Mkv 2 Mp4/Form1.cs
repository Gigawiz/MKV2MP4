using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Reflection;

namespace Mkv_2_Mp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            if (radioButton1.Checked == true)
            {
                OFD.Title = "Select an MKV File to Convert";
                OFD.Filter = "Matroska video/audio file (.mkv)|*.mkv";
            }
            else if (radioButton2.Checked == true)
            {
                OFD.Title = "Select an MP4 File to Convert";
                OFD.Filter = "MP4 Container (.mp4)|*.mp4";
            }
            OFD.FilterIndex = 1;
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = OFD.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            if (radioButton1.Checked == true)
            {
                SFD.Title = "Select a filename to save the converted file to";
                SFD.Filter = "MP4 Container (.mp4)|*.mp4";
            }
            else if (radioButton2.Checked == true)
            {
                SFD.Title = "Select a filename to save the converted file to";
                SFD.Filter = "Matroska video/audio file (.mkv)|*.mkv";
            }
            SFD.FilterIndex = 1;
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = SFD.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("You forgot to fill out one of the fields!");
                return;
            }
            if (radioButton1.Checked == true)
            {
                if (!textBox1.Text.Contains(".mkv"))
                {
                    MessageBox.Show("That input file is not an MKV file!");
                    return;
                }
                if (!textBox2.Text.Contains(".mp4"))
                {
                    MessageBox.Show("That output extension is incorrect! Please save the file as an MP4 Container!");
                    return;
                }
            }
            else if (radioButton2.Checked == true)
            {
                if (!textBox1.Text.Contains(".mp4"))
                {
                    MessageBox.Show("That input file is not an MP4 file!");
                    return;
                }
                if (!textBox2.Text.Contains(".mkv"))
                {
                    MessageBox.Show("That output extension is incorrect! Please save the file as an MKV Container!");
                    return;
                }
            }
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\ffmpeg.exe"))
            {
                MissingFFmpeg dlffmpeg = new MissingFFmpeg();
                dlffmpeg.ShowDialog();
            }
            MessageBox.Show("Preparing to convert.... Do not close any Command windows that may pop up or conversion will fail!...");
            button2.Enabled = true;
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            if (radioButton1.Checked == true)
            {
                convertToMp4(textBox1.Text, textBox2.Text);
            }
            else if (radioButton2.Checked == true)
            {
                convertToMkv(textBox1.Text, textBox2.Text);
            }
            button2.Enabled = false;
            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("ffmpeg"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (File.Exists(textBox2.Text))
            {
                File.Delete(textBox2.Text);
            }
            button2.Enabled = false;
            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void convertToMkv(string infile, string outfile)
        {
            // Initialize the process and its StartInfo properties.
            Process proc = new Process();
            ProcessStartInfo PSI = new ProcessStartInfo();
            PSI.FileName = Directory.GetCurrentDirectory() + @"\ffmpeg.exe";
            PSI.Arguments = "-i "+infile+" -vcodec copy -acodec copy " + outfile;
            proc.StartInfo = PSI;
            proc.Start();
        }

        private void convertToMp4(string infile, string outfile)
        {
            // Initialize the process and its StartInfo properties.
            Process proc = new Process();
            ProcessStartInfo PSI = new ProcessStartInfo();
            PSI.FileName = Directory.GetCurrentDirectory() + @"\ffmpeg.exe";
            PSI.Arguments = "-i " + infile + " -vcodec copy -acodec copy " + outfile;
            proc.StartInfo = PSI;
            proc.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.autoupd)
            {
                checkBox2.Checked = false;
            }
            if (!Properties.Settings.Default.keepffmpeg)
            {
                checkBox1.Checked = false;
            }
            if (Properties.Settings.Default.betaupd)
            {
                checkBox3.Checked = true;
            }
            if (checkBox2.Checked)
            {
                timer1.Start();
            }
            if (File.Exists("ffmpeg-64.exe") && File.Exists("ffmpeg-32.exe"))
            {
                MessageBox.Show("First time setup... Please Wait...");
                if (Environment.Is64BitOperatingSystem)
                {
                    File.Copy("ffmpeg-64.exe", "ffmpeg.exe");
                }
                else
                {
                    File.Copy("ffmpeg-32.exe", "ffmpeg.exe");
                }
                File.Delete("ffmpeg-64.exe");
                File.Delete("ffmpeg-32.exe");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                if (File.Exists("ffmpeg.exe"))
                {
                    File.Delete("ffmpeg.exe");
                }
                if (File.Exists("ffmpeg-32.exe"))
                {
                    File.Delete("ffmpeg-32.exe");
                }
                if (File.Exists("ffmpeg-64.exe"))
                {
                    File.Delete("ffmpeg-64.exe");
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox abt = new AboutBox();
            abt.Show();
        }

        int progver = 1;

        private void asyncUpdateCheck()
        {
            #region system update
            try
            {
                //set the update url
                string updateurl = "http://theharmfulclan.com/mkv2mp4/version.txt";
                var client = new WebClient();
                client.DownloadStringCompleted += (sender, e) =>
                {
                    int build = Convert.ToInt32(e.Result);
                    //compare the program's current build version to the one stored on the server
                    int thisbuild = progver;
                    //if the number on the server is bigger than the local build, then an update is available!
                    if (build > thisbuild)
                    {
                        if (File.Exists("updater.exe"))
                        {
                            File.Delete("updater.exe");
                        }
                        WebClient UPDTR = new WebClient();
                        UPDTR.DownloadFileCompleted += new AsyncCompletedEventHandler(UPDTR_DownloadFileCompleted);
                        if (checkBox3.Checked)
                        {
                            UPDTR.DownloadFileAsync(new Uri("http://theharmfulclan.com/mkv2mp4/beta_updater.exe"), "updater.exe");
                        }
                        else
                        {
                            UPDTR.DownloadFileAsync(new Uri("http://theharmfulclan.com/mkv2mp4/updater.exe"), "updater.exe");
                        }
                    }
                };

                client.DownloadStringAsync(new Uri(updateurl));
                //the build number on the server was less than or equal to the local version, so no need to update.
            }
            catch
            {
                //we died somewhere on the update connection. Let's not disturb the user
            }
            #endregion
        }

        void UPDTR_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                UpdateFound updshw = new UpdateFound(progver);
                updshw.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (radioButton4.Checked == true)
            {
                FBD.Description = "Select the folder containing MKV files to convert";
            }
            else if (radioButton3.Checked == true)
            {
                FBD.Description = "Select the folder containing MP4 files to convert";
            }
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = FBD.SelectedPath;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.Description = "Select the folder to save the converted files to";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = FBD.SelectedPath;
            }
        }
        private Queue<string> _conversionFiles = new Queue<string>();
        string srchqry = "*.mkv";
        private void button6_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                srchqry = "*.mp4";
            }
            if (!Directory.Exists(textBox3.Text))
            {
                MessageBox.Show("The input directory you have entered does not exist!");
                return;
            }
            if (Directory.GetFiles(textBox3.Text, srchqry, SearchOption.AllDirectories).Length < 1)
            {
                MessageBox.Show("The Specified input directory does not contain any " + srchqry + " files! Please check the location and try again!");
                return;
            }
            if (!Directory.Exists(textBox4.Text))
            {
                Directory.CreateDirectory(textBox4.Text);
            }
            tabControl1.SelectedTab = tabPage3;
            try
            {
                foreach (string file in Directory.GetFiles(textBox3.Text, srchqry, SearchOption.AllDirectories))
                {
                    listBox1.Items.Add(file);
                    _conversionFiles.Enqueue(file);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            total = _conversionFiles.Count;
            progressBar1.Maximum = total;
            batchConvert();
        }
        int completed = 0;
        int total = 0;
        private void batchConvert()
        {
            if (_conversionFiles.Any())
            {
                //get our file location from the queue
                var file = _conversionFiles.Dequeue();
                //get the new filename
                string FileName = file.Substring(file.LastIndexOf("\\") + 1,
                                (file.Length - file.LastIndexOf("\\") - 1));

                string fileExt = FileName.Substring(FileName.IndexOf('.'), 4);
                //convert in queue
                Process proc = new Process();
                ProcessStartInfo PSI = new ProcessStartInfo();
                PSI.FileName = Directory.GetCurrentDirectory() + @"\ffmpeg.exe";
                PSI.Arguments = "-i \"" + file + "\" -vcodec copy -acodec copy \"" + textBox4.Text + "\\" + FileName.Replace(fileExt, ".mp4");
                proc.StartInfo = PSI;
                proc.Start();
                proc.WaitForExit();
                if (proc.HasExited)
                {
                    //add number
                    completed++;
                    progressBar1.Value = completed;
                    //redo
                    batchConvert();
                }
            }
            //download complete!
            completed = total;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            asyncUpdateCheck();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            asyncUpdateCheck();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.keepffmpeg = true;
            }
            else if (!checkBox1.Checked)
            {
                Properties.Settings.Default.keepffmpeg = false;
            }
            Properties.Settings.Default.Save();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Properties.Settings.Default.autoupd = true;
            }
            else if (!checkBox2.Checked)
            {
                Properties.Settings.Default.autoupd = false;
            }
            Properties.Settings.Default.Save();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                Properties.Settings.Default.betaupd = true;
            }
            else if (!checkBox3.Checked)
            {
                Properties.Settings.Default.betaupd = false;
            }
            Properties.Settings.Default.Save();
        }
    }
}
