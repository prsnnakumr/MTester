using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace dasTech
{
    public delegate void ProcessCompletedEventHandler(object sender, ProcessCompletedEventArgs e);

    class ConsoleTester : Tester
    {
        private DataGridView testDataGrid;
        private Label _testName;
        private TestStatusDisplayer testStatusDisplayer;
        private ProgressBar testStatusProgressBar;
        private TestInfo _t;
        private BackgroundWorker bgWorker;
        private Thread thread1;
        private delegate void SafeCallDelegate(ProcessCompletedEventArgs e);
        public event EventHandler onTestCompleted;
        private ListTestManager _qTestMgr;
        public event TestStatusDisplayEventHandler onProcessCompleted;

        public ConsoleTester(Form form, ListTestManager queueMgr, TestInfo t)
        {
            _testName = new Label();
            _testName.Text = t._testName;
            _testName.Name = "TestName";
            _testName.Location = new System.Drawing.Point(10, form.Height/2 + 30);
            form.Controls.Add(_testName);

            testStatusDisplayer = new DataGridViewStatusDisplayer(form, t);
            testStatusProgressBar = new ProgressBar();
            _qTestMgr = queueMgr;

            testStatusProgressBar.Name = "TestStatusProgressBar";
            onProcessCompleted += new TestStatusDisplayEventHandler(upDateTestItemStatus);
            onTestCompleted += new EventHandler(_qTestMgr.testCompleted);

            testStatusProgressBar.Location = new System.Drawing.Point(50, form.Height-100);
            form.Controls.Add(testStatusProgressBar);
            testDataGrid = form.Controls.Find("TestCasesGridView", false)[0] as DataGridView;
            _t = t;
        }

        public void Begin()
        {
            bgWorker = new BackgroundWorker();

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Current_Domain_Unhandled_Exception);

            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(testerFinished);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_doWork);
            bgWorker.RunWorkerAsync();
        }

        public static void Current_Domain_Unhandled_Exception(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.Write(1);
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Debug.Write(1);
        }

        private void bgWorker_doWork(object sender, DoWorkEventArgs e)
        {
            int index = 1;
            string fileName;
            var worker = sender as BackgroundWorker;
            foreach (DataGridViewRow item in testDataGrid.Rows)
            {
                item.Selected = true;
                fileName = item.Cells[1].Value as string;
                if (fileName == null )
                    continue;

                runProcess(_t, fileName);
                worker.ReportProgress(index++ * 100 / testStatusDisplayer.FileCount);
                item.Selected = false;

                ProcessCompletedEventArgs e_process = new ProcessCompletedEventArgs();
                e_process.ProcessCompletionStatus = true;
                e_process.ProcessIndex = item.Index;
                onProcessCompleted(testDataGrid, e_process);
            }
        }

        private void testerFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            _qTestMgr.testCompleted(this, e);

        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            testStatusProgressBar.Value = e.ProgressPercentage;
            testStatusProgressBar.Update();
        }

        private void runProcess(TestInfo _t, string fileName)
        {
            Process testProcess = new Process();
            testProcess.StartInfo.FileName = _t._processName;
            testProcess.StartInfo.Arguments = fileName + " " + _t._logLocation + " " + _t._processOption;
            testProcess.StartInfo.RedirectStandardOutput = true;
            testProcess.StartInfo.UseShellExecute = false;
            testProcess.StartInfo.CreateNoWindow = true;
            testProcess.EnableRaisingEvents = true;
            testProcess.Start();
            //testProcess.WaitForInputIdle();
            testProcess.WaitForExit();
        }

        private void upDateTestItemStatus(object sender, ProcessCompletedEventArgs e)
        {
            thread1 = new Thread(new ParameterizedThreadStart(updataDataGridView));
            thread1.Start(e);
            //Thread.Sleep(1000);
           
        }

        private void updataDataGridView(object e_process)
        {
            ProcessCompletedEventArgs e = e_process as ProcessCompletedEventArgs;

            if (testDataGrid.InvokeRequired)
            {
                var d = new SafeCallDelegate(updataDataGridView);
                testDataGrid.Invoke(d, new object[] { e_process });
            }
            else
            {
                testDataGrid.FirstDisplayedScrollingRowIndex = e.ProcessIndex;
                testDataGrid.Rows[e.ProcessIndex].Cells[2].Value = e.ProcessCompletionStatus == true ? "PASS" : "FAIL";
            }
        }
    }
}
