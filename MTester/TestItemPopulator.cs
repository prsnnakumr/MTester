using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading.Tasks;

namespace dasTech
{
    interface TestItemPopulator
    {
        void populateTestItems(Form form , List<TestInfo> res);
        void TestDataUpdated(object sender, DataUpdatedEventArgs e);
    }
}
