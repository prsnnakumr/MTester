using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dasTech
{
    interface TestStatusDisplayer
    {
        int FileCount { get; }

        void setStatus(bool status);
        void update(int index);
    }
}
