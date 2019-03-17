using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CoreApp.Scenario.Scenario;

namespace CoreApp.Forms
{
    public partial class ScenarioForm : Form
    {
        IEnumerable<Tuple<LineState, string>> scenario;

        public ScenarioForm(IEnumerable<Tuple<LineState, string>> scenario)
        {
            InitializeComponent();

            mainColumn.Width = LViewScenarioLines.Width;

            this.scenario = scenario;

            int i = 0;
            foreach(var item in scenario)
            {
                LViewScenarioLines.Items.Add(item.Item2);

                switch(item.Item1)
                {
                    case LineState.normal:
                        LViewScenarioLines.Items[i++].BackColor = Color.LightGreen;
                        break;
                    case LineState.notInFiles:
                        LViewScenarioLines.Items[i++].BackColor = Color.Yellow;
                        break;
                    case LineState.notInScenario:
                        LViewScenarioLines.Items[i++].BackColor = Color.OrangeRed;
                        break;
                }

            }

            PbNormal.BackColor = Color.LightGreen;
            PbNotInFiles.BackColor = Color.Yellow;
            PbNotInScenario.BackColor = Color.OrangeRed;

        }

        private void BtUp_Click(object sender, EventArgs e)
        {

        }

        private void ScenarioForm_Resize(object sender, EventArgs e)
        {
            mainColumn.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
