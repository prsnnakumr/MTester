using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dasTech
{
    public class DataUpdatedEventArgs : EventArgs
    {
        internal TestInfo _testInfo;
        internal TestInfo testInfo
        {
            get
            { return _testInfo; }
            set
            { _testInfo = value; }
        }
    }
}
