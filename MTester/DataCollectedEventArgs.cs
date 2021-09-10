using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dasTech
{
    public class DataCollectedEventArgs: EventArgs
    {
        private TableLayoutPanel _testItems;
        private Form _concerndForm;
        internal TableLayoutPanel List_Data
        {
            get { return _testItems; }
            set {_testItems = value; }
        }

        internal Form TargetForm
        {
            get { return _concerndForm; }
            set { _concerndForm = value; }
        }
    }
}
