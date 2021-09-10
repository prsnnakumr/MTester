using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dasTech
{
    class ListTestManager : TestManager
    {
        private IList<TestInfo> _testData;
        private int index;
        private Form _form;
        //private event EventHandler onTestItemDataLoaded;

        public ListTestManager(Form form ,ICollection<TestInfo> data)
        {
            _testData = data as IList<TestInfo>;
            _form = form;
            index = 0;
        }

        public void StartTest()
        {
            beginTest(index);
        }

        private void beginTest(int _index)
        {
            Tester tester = new ConsoleTester(_form, this, _testData[0]);
            startTester(tester);
        }
        private void startTester(object sender)
        {
            Tester tester = sender as Tester;
            tester.Begin();
            //EventArgs e = new EventArgs();
            //onTestCompleted(tester, e);
        }

        public void testCompleted(object sender, EventArgs e)
        {
            var tester = sender as Tester;
            _form.Controls.Remove(_form.Controls.Find("TestCasesGridView", false)[0]);
            _form.Controls.Remove(_form.Controls.Find("TestStatusProgressBar", false)[0]);
            _form.Controls.Remove(_form.Controls.Find("TestName", false)[0]);
            _form.Controls.Find("ListDataTableLayoutPanel", false)[0].Controls[index].ForeColor = System.Drawing.Color.DarkBlue;

            //if (_testData.Count > 0)
            //    _testData.RemoveAt(0);

            if (_testData.Count > ++index) beginTest(index);
            else
                _form.Controls.Find("startBtn", false)[0].Enabled = true;
        }

        public bool QueueTest(ICollection<TestInfo> data)
        {
            _testData = _testData.Concat(data as IList<TestInfo>).ToList();
            return true;
        }
    }
}
