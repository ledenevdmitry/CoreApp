using CoreApp.ReleaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CoreApp.Scenario.Scenario;

namespace CoreApp.Forms
{
    public partial class ScenarioForm : Form
    {

        CPatch cpatch;
        CVS.CVS cvs;
        string cvsPath;
        string localPath;
        string scenarioFilePath;

        IEnumerable<Tuple<LineState, string>> scenario;

        public ScenarioForm(IEnumerable<Tuple<LineState, string>> scenario, CVS.CVS cvs, string localPath, string cvsPath)
        {
            InitializeComponent();

            this.cpatch = cpatch;
            this.cvs = cvs;
            this.cvsPath = cpatch.Download();
            this.localPath = cpatch.Dir.FullName;

            scenarioFilePath = Path.Combine(localPath, "file_sc.txt");

            Application.Idle += OnIdle;

            mainColumn.Width = LViewScenarioLines.Width;

            this.scenario = scenario;
            this.cvs = cvs;
            this.cvsPath = cvsPath;
            this.localPath = localPath;

            int i = 0;
            foreach (var item in scenario)
            {
                LViewScenarioLines.Items.Add(item.Item2);

                switch (item.Item1)
                {
                    case LineState.newScenarioNormal:
                        LViewScenarioLines.Items[i++].BackColor = Color.LightGreen;
                        break;
                    case LineState.oldScenario:
                        LViewScenarioLines.Items[i++].BackColor = Color.LightBlue;
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


        private void OnIdle(object sender, EventArgs e)
        {
            BtUp.Enabled = LViewScenarioLines.SelectedItems.Count > 0 && LViewScenarioLines.SelectedIndices.IndexOf(0) == -1;
            BtDown.Enabled = LViewScenarioLines.SelectedItems.Count > 0 && LViewScenarioLines.SelectedIndices.IndexOf(LViewScenarioLines.Items.Count - 1) == -1;
        }

        private void BtUp_Click(object sender, EventArgs e)
        {
            var itemsToMove = LViewScenarioLines.SelectedItems;
            for (int i = 0; i < itemsToMove.Count; ++i)
            {
                var selectedItem = itemsToMove[i];
                int oldIndex = LViewScenarioLines.Items.IndexOf(selectedItem);
                int newIndex = oldIndex - 1;

                var curr = LViewScenarioLines.Items[oldIndex];
                var prev = LViewScenarioLines.Items[newIndex];

                LViewScenarioLines.Items.Remove(selectedItem);
                LViewScenarioLines.Items.Insert(newIndex, selectedItem);
                LViewScenarioLines.Items[newIndex].Selected = true;
                LViewScenarioLines.Select();
            }
        }

        private void ScenarioForm_Resize(object sender, EventArgs e)
        {
            mainColumn.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void BtDown_Click(object sender, EventArgs e)
        {
            var itemsToMove = LViewScenarioLines.SelectedItems;
            for (int i = itemsToMove.Count - 1; i >= 0; --i)
            {
                var selectedItem = itemsToMove[i];
                int oldIndex = LViewScenarioLines.Items.IndexOf(selectedItem);
                int newIndex = oldIndex + 1;

                var curr = LViewScenarioLines.Items[oldIndex];
                var prev = LViewScenarioLines.Items[newIndex];

                LViewScenarioLines.Items.Remove(selectedItem);
                LViewScenarioLines.Items.Insert(newIndex, selectedItem);
                LViewScenarioLines.Items[newIndex].Selected = true;
                LViewScenarioLines.Select();
            }
        }

        private void BtDeleteLines_Click(object sender, EventArgs e)
        {
            var itemsToDelete = LViewScenarioLines.SelectedItems;
            int countAtStart = itemsToDelete.Count;

            for (int i = 0; i < countAtStart; ++i)
            {
                var selectedItem = itemsToDelete[0];
                LViewScenarioLines.Items.Remove(selectedItem);
            }
        }

        private string CreateFinalScenario()
        {
            string scenario = "";
            foreach (var item in LViewScenarioLines.Items)
            {
                scenario += ((ListViewItem)item).Text + Environment.NewLine;
            }
            return scenario;
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            string scenarioText = CreateFinalScenario();
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "file_sc.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.Write(scenarioText);
                }
            }
        }

        private void BtLoadToCVS_Click(object sender, EventArgs e)
        {
            string scenarioText = CreateFinalScenario();

            cvs.PrepareToPush(cvsPath, "file_sc.txt");

            if (File.Exists(scenarioFilePath))
            {
                File.SetAttributes(scenarioFilePath, FileAttributes.Normal);
            }

            using (StreamWriter sw = new StreamWriter(scenarioFilePath))
            {
                sw.Write(scenarioText);
            }

            cvs.Push(localPath, "file_sc.txt", cvsPath);
        }
    }
}





















