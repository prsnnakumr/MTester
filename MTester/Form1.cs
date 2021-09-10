using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace dasTech
{
    public delegate void TestStatusDisplayEventHandler(object sender, ProcessCompletedEventArgs e);


    public partial class Form1 : Form
    {
        private DataCollector dataCollector;
        private TestManager testManager;
        internal static ICollection<TestInfo> data;

        public static event DataCollectedEventHandler onNewTestAdded;

        //private Tester tester;

        public Form1()
        {
            InitializeComponent();
        }
        private void oDxSWTlStripMenuItem_Click(object sender, EventArgs e)
        {
            dataCollector = new FormDataCollector(this);
        }

        public static void dataReceived(object sender, DataCollectedEventArgs e)
        {
            TableLayoutPanel testItemsList;
            var listDataControl = e.TargetForm.Controls.Find("ListDataTableLayoutPanel", false);
            if (listDataControl.Count() > 0)
            {
                foreach (var item in e.List_Data.Controls)
                    listDataControl[0].Controls.Add(item as Control);
                Form1.onNewTestAdded(sender, e);
            }
            else
            {
                testItemsList = e.List_Data;
                testItemsList.Name = "ListDataTableLayoutPanel";
                testItemsList.Location = new Point(500, 40);
                testItemsList.Size = new Size(200, 100);
                e.TargetForm.Controls.Add(testItemsList);
                var btn = e.TargetForm.Controls.Find("startBtn", false)[0];
                btn.Enabled = true;
            }
           
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            data = dataCollector.getData;
            startBtn.Enabled = false;
            testManager = new ListTestManager(this, data);
            onNewTestAdded += new DataCollectedEventHandler(NewTestAdded);
            testManager.StartTest();
            
        }
        public void NewTestAdded(object sender, DataCollectedEventArgs e)
        {
            testManager.QueueTest(dataCollector.getData);
        }
    }
}
