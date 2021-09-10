using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dasTech
{
    public class ProcessCompletedEventArgs : EventArgs
    {

        private int index;
        private bool _status;
        internal bool ProcessCompletionStatus { get { return _status; } set { _status = value; } }
        internal int ProcessIndex { get { return index; } set { index = value; } }
    }
}
