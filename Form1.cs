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
        List<Process> processes = new List<Process>();

        int id = 0;

        public Form1()
        {
            InitializeComponent();
            
        }
        public void StartProcess()
        {
            try
            {
                Process ytdl = new Process();
                processes.Add(ytdl);
                CreatePanel();
                id += 1;

                ytdl.StartInfo.FileName = "C:\\xampp\\htdocs\\pyGUI\\resources\\youtube-dl.exe";
                ytdl.StartInfo.Arguments = "-o \"C:\\xampp\\htdocs\\pyGUI\\dl\\%(title)s %(timestamp)s\" -f " + txtQuality.Text + " " + txtUrl.Text;
                ytdl.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ytdl.StartInfo.UseShellExecute = false;
                ytdl.StartInfo.RedirectStandardOutput = true;
                ytdl.StartInfo.CreateNoWindow = false;
                ytdl.StartInfo.RedirectStandardOutput = true;
                ytdl.StartInfo.RedirectStandardInput = true;
                ytdl.OutputDataReceived += (sender, e) => OutputHandler(lblOutput, e.Data);
                ytdl.Start();
                ytdl.BeginOutputReadLine();
                string q = "";
                while (!ytdl.HasExited)
                {
                    q += ytdl.StandardOutput.ReadToEnd();
                }
                lblStatus.Text = q;
            }
            catch
            {
                Console.WriteLine("Process error.");
            }
        }

        private void CreatePanel()
        {
            Panel pnlWorker = new Panel();
            pnlWorker.Width = pnlThreads.Width;
            pnlWorker.Height = 25;
            flowThreads.Controls.Add(pnlWorker);

            string[] channelName = txtUrl.Text.Split('/');
            Label lblWorkerUrl = new Label();
            lblWorkerUrl.Top = 0;
            lblWorkerUrl.Left = 0;
            lblWorkerUrl.Text = channelName[channelName.Length - 2];
            pnlWorker.Controls.Add(lblWorkerUrl);

            Label lblWorkerOutput = new Label();
            lblWorkerOutput.Top = 0;
            lblWorkerOutput.Left = 150;
            lblWorkerOutput.Text = channelName[channelName.Length - 2];
            pnlWorker.Controls.Add(lblWorkerOutput);

            Button btnWorkerStop = new Button();
            btnWorkerStop.Width = 50;
            btnWorkerStop.Height = 25;
            btnWorkerStop.Location = new Point(lblWorkerUrl.Width + 10, lblWorkerUrl.Location.Y);
            btnWorkerStop.Text = "Stop";
            btnWorkerStop.Tag = id;
            btnWorkerStop.Click += StopEvent;
            pnlWorker.Controls.Add(btnWorkerStop);

            //Label lblWorkerStatus = new Label();
            //lblWorkerStatus.Top = 0;
            //lblWorkerStatus.Left = lblWorkerUrl.Width + 10;
            //lblWorkerStatus.Text = txtUrl.Text;
            //pnlWorker.Controls.Add(lblWorkerUrl);
        }

        private void StopEvent(object sender, EventArgs e)
        {
            int buttonId = (int)(sender as Button).Tag;
            processes[buttonId].CloseMainWindow();
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartProcess();
            
        }
    }
}
