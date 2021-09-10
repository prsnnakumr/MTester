using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace dasTech
{
    public delegate void DataUpdatedEventHandler(object sender, DataUpdatedEventArgs e);
    //public delegate void DataCollectedEventHandler(object sender, DataCollectedEventArgs e);

    class FormDataCollector : DataCollector
    {
        private OpenFileDialog openfileDialog;
        private TextBox processTbox;
        private TextBox testNameTbox;
        private TextBox testsuiteTbox;
        private TextBox logTbox;
        private TextBox optionTbox;
        private Form collectionAgent;
        private Form parentForm;
        private Button acceptBtn;
        private List<TestInfo> _collection_t;
        private Button closeDataFormBtn;
        private TestItemPopulator testItemPopulator;

        public event DataCollectedEventHandler OnDataCollected;
        private event DataUpdatedEventHandler OnDataUpDated;

        public ICollection<TestInfo> getData
        {
            get
            {
                return _collection_t;
            }
        }

        public FormDataCollector(Form form)
        {
            _collection_t = new List<TestInfo>();
            OnDataCollected += new DataCollectedEventHandler(Form1.dataReceived);
            parentForm = form;
            collectionAgent = new Form();
            collectionAgent.Size = new Size(650, 350);
            collectionAgent.ControlBox = false;
            collectionAgent.FormBorderStyle = FormBorderStyle.FixedSingle;
            collectionAgent.Text = "Test Process Data";
            collectionAgent.StartPosition = FormStartPosition.CenterScreen;

            testItemPopulator = new TableLayoutTestItemPopulator(collectionAgent);
            OnDataUpDated += new DataUpdatedEventHandler(testItemPopulator.TestDataUpdated);

            var testNameLbl = createLabelControl("Test Name", new Point(10, 10), new Size(100, 13));
            collectionAgent.Controls.Add(testNameLbl);
            testNameTbox = createTextBoxControl("Type Test Name", new Point(110, 8),
                                                new Size(400, 12));
            testNameTbox.Name = "TestNameTextBox";
            collectionAgent.Controls.Add(testNameTbox);

            var pNameLbl = createLabelControl("Process Name", new Point(10, 26), new Size(100, 13));
            collectionAgent.Controls.Add(pNameLbl);
            processTbox = createTextBoxControl("Click Here to Add File", new Point(110, 26),
                                                new Size(400, 12));
            processTbox.Enabled = true;
            processTbox.Name = "ProcessTextBox";

            collectionAgent.Controls.Add(processTbox);

            var tsNameLbl = createLabelControl("TestSuite ", new Point(10, 42), new Size(100, 13));
            collectionAgent.Controls.Add(tsNameLbl);
            testsuiteTbox = createTextBoxControl("Click Here to Add File", new Point(110, 42),
                                                 new Size(400, 12));
            collectionAgent.Controls.Add(testsuiteTbox);
            testsuiteTbox.Name = "TestSuiteTextBox";

            var logNameLbl = createLabelControl("Log Location ", new Point(10, 58), new Size(100, 13));
            collectionAgent.Controls.Add(logNameLbl);
            logTbox = createTextBoxControl("Click Here to Add Folder", new Point(110, 58),
                                                 new Size(400, 12));
            collectionAgent.Controls.Add(logTbox);
            logTbox.Name = "LogTextBox";

            var optionNameLbl = createLabelControl("Option ", new Point(10, 70), new Size(100, 13));
            collectionAgent.Controls.Add(optionNameLbl);
            optionTbox = createTextBoxControl("Type the Option String", new Point(110, 76),
                                                 new Size(400, 12));
            collectionAgent.Controls.Add(optionTbox);
            optionTbox.Name = "OptionTextBox";

            openfileDialog = createOpenFileDialogControl();
            

            processTbox.Click += new EventHandler(this.ProcessTbox_Click);
            testsuiteTbox.Click += new EventHandler(this.ProcessTbox_Click);
            logTbox.Click += new EventHandler(this.logTBox_clicked);
            optionTbox.TextChanged += new EventHandler(this.optionTBox_TextChanged);

            collectionAgent.Enabled = true;
            collectionAgent.Visible = true;
            collectionAgent.BringToFront();

            acceptBtn = createButtonControl("Accept", new Point(collectionAgent.Width / 2 - 16,
                                            collectionAgent.Height - 100), new Size(84, 20));
            collectionAgent.Controls.Add(acceptBtn);
            acceptBtn.Click += new System.EventHandler(acceptBtn_Clicked);

            closeDataFormBtn = createButtonControl("Done ", new Point(collectionAgent.Width / 2 + 60, collectionAgent.Height - 100),
                                                   new Size(84, 20));
            collectionAgent.Controls.Add(closeDataFormBtn);
            closeDataFormBtn.Click += new System.EventHandler(closeDataForm_Clicked);

            testsuiteTbox.Enabled = false;
            logTbox.Enabled = false;
            optionTbox.Enabled = false;
            acceptBtn.Enabled = false;
            if (_collection_t.Count == 0)
                closeDataFormBtn.Enabled = false;

        }

        private FlowLayoutPanel createNewFlowLayoutPanelControl(string text, Point location, Size size)
        {
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Size = size;
            flowLayoutPanel.Location = location;
            flowLayoutPanel.Name = text;
            return flowLayoutPanel;

        }

        private void closeDataForm_Clicked(object sender, EventArgs e)
        {
            DataCollectedEventArgs e_args = new DataCollectedEventArgs();
            TableLayoutPanel listItems_copied = new TableLayoutPanel();
            listItems_copied = collectionAgent.Controls.Find("testItemTable", false)[0] as TableLayoutPanel ;
            e_args.List_Data = listItems_copied;
            collectionAgent.Controls.Remove(listItems_copied);
            collectionAgent.Close();
            e_args.TargetForm = parentForm;
            OnDataCollected(this, e_args);
        }

        private void resetField()
        {
            processTbox.Text = "Click Here to Add File";
            testsuiteTbox.Text = "Click Here to Add File";
            logTbox.Text = "Click Here to Add Folder";
            optionTbox.Text = "Type Option String";
            testNameTbox.Text = "Type Test Name";
            acceptBtn.Enabled = false;
            testsuiteTbox.Enabled = false;
            logTbox.Enabled = false;
            optionTbox.Enabled = false;

        }

        private void optionTBox_TextChanged(object sender, EventArgs e) => acceptBtn.Enabled = true;

        private void logTBox_clicked(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextBox temp = sender as TextBox;
                temp.Text = folderBrowserDialog.SelectedPath;
                optionTbox.Enabled = true;
                optionTbox.Focus();
            }
        } 

        private void acceptBtn_Clicked(object sender, EventArgs e)
        {
            TestInfo t = new TestInfo();
            t._testName = testNameTbox.Text;
            t._processName = processTbox.Text;
            t._testSuite = testsuiteTbox.Text;
            t._logLocation = logTbox.Text;
            t._processOption = optionTbox.Text;
            _collection_t.Add(t);
            if (_collection_t.Count > 0)
                closeDataFormBtn.Enabled = true;

            resetField();

            DataUpdatedEventArgs dataUpdateEventArg = new DataUpdatedEventArgs();
            dataUpdateEventArg.testInfo = t;
            OnDataUpDated(collectionAgent, dataUpdateEventArg);

        }
        

        private Button createButtonControl(string text, Point location, Size size)
        {
            Button btn = new Button();
            btn.Location = location;
            btn.Size = size;
            btn.Text = text;

            return btn;
        }
        private void ProcessTbox_Click(object sender, EventArgs e)
        {
            TextBox received = sender as TextBox;

            if (received.Name == "TestSuiteTextBox")
            {
                openfileDialog.Filter = "Text files (*.txt) | *.txt";
                openfileDialog.Title = "Browse Text Files";
            }

            else if (received.Name == "ProcessTextBox")
            {
                openfileDialog.Title = "Browse Exe Files";
                openfileDialog.Filter = "Exe Files (*.exe)|*.exe";
            }

            if (openfileDialog.ShowDialog() == DialogResult.OK)
            {
                received.Text = openfileDialog.FileName;

                if (received.Name == "ProcessTextBox")
                {
                    testsuiteTbox.Enabled = true;
                }
                else if (received.Name == "TestSuiteTextBox")
                { logTbox.Enabled = true; }
            }
        }
        private OpenFileDialog createOpenFileDialogControl()
        {
            OpenFileDialog fOpenDialog = new OpenFileDialog();
            return fOpenDialog;
        }
        private Label createLabelControl(string text, Point location, Size size)
        {
            Label userLabel = new Label();
            userLabel.Text = text;
            userLabel.Location = location;
            userLabel.Size = size;

            return userLabel;
        }
        private TextBox createTextBoxControl(string text, Point location, Size size)
        {
            TextBox processTextBox = new TextBox();
            processTextBox.Location = location;
            processTextBox.Size = size;
            processTextBox.Text = text;

            return processTextBox;
        }
    }

}
