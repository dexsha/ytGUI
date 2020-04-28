using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pyGUI
{
    public partial class Form1 : Form
    {
        public static Process ytdl = new Process();

        public Form1()
        {
            InitializeComponent();        
        }
        public void StartProcess()
        {
            try
            {
                ytdl.StartInfo.FileName = "C:\\xampp\\htdocs\\pyGUI\\youtube-dl.exe";
                ytdl.StartInfo.Arguments = "-o \"C:\\xampp\\htdocs\\pyGUI\\dl\\%(title)s %(timestamp)s\" -f " + txtQuality.Text + " " + txtUrl.Text;
                ytdl.StartInfo.UseShellExecute = false;
                ytdl.StartInfo.RedirectStandardOutput = true;
                ytdl.StartInfo.CreateNoWindow = true;
                ytdl.OutputDataReceived += (sender, e) => OutputHandler(lblOutput, e.Data);
                ytdl.Start();
                ytdl.BeginOutputReadLine();
            }
            catch
            {
                Console.WriteLine("Process error.");
            }
        }

        private void OutputHandler(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                label.Invoke((System.Action)(() => OutputHandler(label, text)));
            }
            else
            {
                label.Text = text;
                if (text == "") {
                    lblStatus.Text = "Stopped";
                }
                else if (text != "") {
                    lblStatus.Text = "Running";
                }
            }
            Console.WriteLine(text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ytdl.CloseMainWindow();
            }
            catch
            {
                Console.WriteLine("No process running.");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartProcess();
            
        }

        private void CreatePanel()
        {
            Panel pnlWorker = new Panel();
            pnlWorker.Width = pnlThreads.Width;
            pnlWorker.Height = 25;
            flowThreads.Controls.Add(pnlWorker);

            Label lblWorkerUrl = new Label();
            lblWorkerUrl.Top = 0;
            lblWorkerUrl.Left = 0;

            string[] channelName = txtUrl.Text.Split('/');

            lblWorkerUrl.Text = channelName[channelName.Length-2];
            pnlWorker.Controls.Add(lblWorkerUrl);

            Button btnWorkerStop = new Button();
            btnWorkerStop.Width = 50;
            btnWorkerStop.Height = 25;
            btnWorkerStop.Location = new Point(lblWorkerUrl.Width + 10, lblWorkerUrl.Location.Y);
            btnWorkerStop.Text = "Stop";
            pnlWorker.Controls.Add(btnWorkerStop);
            //btnWorkerStop.Click += new EventHandler(StopThread);

            //Label lblWorkerStatus = new Label();
            //lblWorkerStatus.Top = 0;
            //lblWorkerStatus.Left = lblWorkerUrl.Width + 10;
            //lblWorkerStatus.Text = txtUrl.Text;
            //pnlWorker.Controls.Add(lblWorkerUrl);
        }

        void StopThread(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CreatePanel();
        }
    }
}
