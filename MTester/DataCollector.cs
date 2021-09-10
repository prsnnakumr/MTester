using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dasTech
{
    public delegate void DataCollectedEventHandler(object sender , DataCollectedEventArgs e);

    struct TestInfo
    {
        public string _testName;
        public string _processName;
        public string _testSuite;
        public string _logLocation;
        public string _processOption;
       
    }

    interface DataCollector
    {
        event DataCollectedEventHandler OnDataCollected;
        //event EventHandler OnDataUpDated;
        ICollection<TestInfo> getData { get; }
        //void getData()
        //DialogResult getData(TestInfo t);
    }
}
