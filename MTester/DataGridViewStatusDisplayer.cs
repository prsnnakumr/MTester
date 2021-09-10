using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace dasTech
{
    class DataGridViewStatusDisplayer : TestStatusDisplayer
    {
        private DataGridView testDataGridView;
        private Dictionary<int, string> testCases;
        private int fileCount;

        public DataGridViewStatusDisplayer(Form form, TestInfo t)
        {
            testDataGridView = new DataGridView();
            testDataGridView.Name = "TestCasesGridView";
            testCases = new Dictionary<int, string>(6);

            testDataGridView.RowHeadersVisible = false;
            testDataGridView.ColumnHeadersVisible = true;
            testDataGridView.Location = new System.Drawing.Point(50, form.Height / 2);
            testDataGridView.Size = new System.Drawing.Size(form.Width - 300, form.Height / 2 - 150);
            //testDataGridView.ColumnCount = 3;

            popualateTestCases(t);

            //testDataGridView.DataSource = testCases;

            testDataGridView.Columns.Add("SNo", "SNO");
            testDataGridView.Columns.Add("Input File", "INPUT_FILE");
            testDataGridView.Columns.Add("Status", "STATUS");

            foreach (var item in testCases)
                testDataGridView.Rows.Add(item.Key, item.Value);

            form.Controls.Add(testDataGridView);
        }

        private void popualateTestCases(TestInfo t)
        {
            StreamReader str = new StreamReader(t._testSuite);
            int index = 1;
            while (!str.EndOfStream)
                testCases.Add(index++, str.ReadLine());
            fileCount = index - 1;
            str.Close();
        }

        public void setStatus(bool status)
        {
        }

        public void update(int index)
        {
        }

        public int FileCount
        {
            get
            {
                return fileCount;
            }
        }
    }
}
