using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace dasTech
{
    class TableLayoutTestItemPopulator : TestItemPopulator
    {
        private TableLayoutPanel testItemTblLytPanel;

        public TableLayoutTestItemPopulator(Form form)
        {
            testItemTblLytPanel = new TableLayoutPanel();
            testItemTblLytPanel.Size = new System.Drawing.Size(200,100);
            testItemTblLytPanel.Location = new System.Drawing.Point(10, form.Height - 200);
            testItemTblLytPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
            testItemTblLytPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            testItemTblLytPanel.AutoScroll = true;
            testItemTblLytPanel.Name = "testItemTable";

            form.Controls.Add(testItemTblLytPanel);
        }

        public void populateTestItems(Form form, List<TestInfo> res)
        {
            form.Controls.Add(testItemTblLytPanel);

            if (testItemTblLytPanel.RowCount < res.Count)
            {
                Label itemInfo = new Label();
                itemInfo.Text = res.Count.ToString() + ". " + res[res.Count - 1]._testName;
                testItemTblLytPanel.SetRow(itemInfo, res.Count);
                
            }
        }

        public void TestDataUpdated(object sender, DataUpdatedEventArgs e)
        {
            Form currentForm = sender as Form;
            
            Label itemInfo = new Label();
            itemInfo.Text = e.testInfo._testName;
            itemInfo.Enabled = true;
            testItemTblLytPanel.Controls.Add(itemInfo);
        }
    }
}
