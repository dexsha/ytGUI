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
        List<Label> outputLabels = new List<Label>();
        List<ThreadHelperClass> threadHelper = new List<ThreadHelperClass>();

        int id = 0;

        public Form1()
        {
            InitializeComponent();
            
        }
        public void StartProcess()
        {
            try
            {
                
                
                CreatePanel();
                Process ytdl = new Process();
                processes.Add(ytdl);
                ThreadHelperClass tHelper = new ThreadHelperClass();
                threadHelper.Add(tHelper);

                id += 1;

                ytdl.StartInfo.FileName = "C:\\xampp\\htdocs\\pyGUI\\resources\\youtube-dl.exe";
                ytdl.StartInfo.Arguments = "-o \"C:\\xampp\\htdocs\\pyGUI\\dl\\%(title)s %(timestamp)s\" -f " + txtQuality.Text + " " + txtUrl.Text;
                ytdl.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ytdl.StartInfo.UseShellExecute = false;
                ytdl.StartInfo.RedirectStandardOutput = true;
                ytdl.StartInfo.CreateNoWindow = false;
                ytdl.StartInfo.RedirectStandardOutput = true;
                ytdl.StartInfo.RedirectStandardInput = true;
                ytdl.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    tHelper.SetText(this, outputLabels[id-1], e.Data);
                });
                ytdl.Start();
                ytdl.BeginOutputReadLine();
            }
            catch
            {
                Console.WriteLine("Process error.");
            }
        }
        public class ThreadHelperClass
        {
            delegate void SetTextCallback(Form f, Control ctrl, string text);

            public void SetText(Form form, Control ctrl, string text)
            {
                if (ctrl.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    form.Invoke(d, new object[] { form, ctrl, text });
                }
                else
                {
                    ctrl.Text = text;
                }
            }
        }

        private void CreatePanel()
        {
            flowThreads.Height = pnlThreads.Height;

            TableLayoutPanel layoutWorker = new TableLayoutPanel();
            layoutWorker.Width = flowThreads.Width - 5;
            layoutWorker.Height = 27;
            layoutWorker.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            layoutWorker.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            layoutWorker.RowCount = 1;
            flowThreads.Controls.Add(layoutWorker);

            string[] channelName = txtUrl.Text.Split('/');
            Label lblChannelName = new Label();
            lblChannelName.Width = 100;
            lblChannelName.Height = 25;
            lblChannelName.TextAlign = ContentAlignment.MiddleLeft;
            lblChannelName.Text = channelName[channelName.Length - 2];
            layoutWorker.Controls.Add(lblChannelName);

            Label lblOutput = new Label();
            lblOutput.Width = 150;
            lblOutput.Height = 25;
            lblOutput.TextAlign = ContentAlignment.MiddleLeft;
            lblOutput.Text = "";
            lblOutput.Tag = id;
            layoutWorker.Controls.Add(lblOutput);
            outputLabels.Add(lblOutput);

            Label lblStatus = new Label();
            lblStatus.Width = 100;
            lblStatus.Height = 25;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.Text = "Status";
            layoutWorker.Controls.Add(lblStatus);

            Button btnWorkerStop = new Button();
            btnWorkerStop.Width = 50;
            btnWorkerStop.Height = 25;
            btnWorkerStop.Margin = new Padding(-5, -5, 0, 0);
            btnWorkerStop.Text = "Stop";
            btnWorkerStop.Tag = id;
            btnWorkerStop.Click += StopEvent;
            btnWorkerStop.Visible = true;
            layoutWorker.Controls.Add(btnWorkerStop);

            Button btnWorkerStart = new Button();
            btnWorkerStart.Width = 50;
            btnWorkerStart.Height = 25;
            btnWorkerStart.Margin = new Padding(-5, -5, 0, 0);
            btnWorkerStart.Text = "Start";
            btnWorkerStart.Tag = id;
            btnWorkerStart.Click += StartEvent;
            btnWorkerStart.Visible = true;
            layoutWorker.Controls.Add(btnWorkerStart);

            Button btnWorkerRemove = new Button();
            btnWorkerRemove.Width = 55;
            btnWorkerRemove.Height = 25;
            btnWorkerRemove.Margin = new Padding(-5, -5, 0, 0);
            btnWorkerRemove.Text = "Remove";
            btnWorkerRemove.Tag = id;
            btnWorkerRemove.Click += RemoveEvent;
            layoutWorker.Controls.Add(btnWorkerRemove);

        }

        private void StopEvent(object sender, EventArgs e)
        {
            int processId = (int)(sender as Button).Tag;

            if (!processes[processId].HasExited)
            {
                processes[processId].CloseMainWindow();
            }
        }

        private void StartEvent(object sender, EventArgs e)
        {
            int processId = (int)(sender as Button).Tag;

            if (processes[processId].HasExited)
                processes[processId].Start();
            else
                MessageBox.Show("Process is still running!");
        }

        private void RemoveEvent(object sender, EventArgs e)
        {
            int processId = (int)(sender as Button).Tag;

            if (processes[processId].HasExited)
                (sender as Button).Parent.Dispose();
            else
                MessageBox.Show("Process is still running!");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtUrl.Text != "")
                StartProcess();
            else
                MessageBox.Show("Enter URL!");
        }
    }
}
