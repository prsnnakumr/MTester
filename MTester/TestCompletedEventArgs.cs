using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dasTech
{
    public class TestCompletedEventArgs : EventArgs
    {
        private Form _form;

        internal Form TargetForm { get { return _form; } set { _form = value; } }
    }
}
