using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dasTech
{
    interface TestManager
    {
        void StartTest();
        bool QueueTest(ICollection<TestInfo> data);
    }
}
