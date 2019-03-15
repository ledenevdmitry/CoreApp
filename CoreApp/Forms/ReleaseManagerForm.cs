using CoreApp.ReleaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreApp
{
    public partial class ReleaseManagerForm : Form
    {
        ReleaseManager rm;

        Release currRelease;
        CPatch currCPatch;
        ZPatch currZPatch;

        int actualCPatchStatusIndex = -1;
        int actualZPatchStatusIndex = -1;

        public ReleaseManagerForm()
        {
            InitializeComponent();            

            CbCPatchStatus.DataSource = Enum.GetValues(typeof(CPatchStatuses));
            CbZPatchStatus.DataSource = Enum.GetValues(typeof(ZPatchStatuses));

            rm = new ReleaseManager();

            CreateTree();
            Application.Idle += OnIdle;

            TSMIRename.Click += RenameStart;
        }

        private void RenameStart(object sender, EventArgs e)
        {
            mainTree.LabelEdit = true;
            mainTree.SelectedNode.BeginEdit();
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtAddCPatch.Enabled = mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 0;

            BtReleaseGraph.Enabled = 
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 0;

            GbCPatch.Visible = 
            BtCPatchGraph.Enabled = 
            BtZPatchOrder.Enabled =
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 1;

            GbZPatch.Visible = 
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 2;
        }

        private void CreateTree()
        {
            foreach(Release release in rm.releases)
            {
                var currReleaseNode = mainTree.Nodes.Add(release.releaseId.ToString(), release.releaseName);                
            }
        }

        private void DisplayCPatch()
        {
            CbCPatchStatus.SelectedItem = currCPatch.CPatchStatus;

            LboxCPatchDependenciesFrom.DataSource = currCPatch.dependenciesFrom.ToList();
            LboxCPatchDependenciesTo.DataSource = currCPatch.dependenciesTo.ToList();
        }

        private void DisplayZPatch()
        {
            CbZPatchStatus.SelectedItem = currZPatch.ZPatchStatus;
            CbZPatchCPatch.SelectedItem = currZPatch.cpatch;

            LboxZPatchDependenciesFrom.DataSource = currZPatch.dependenciesFrom.ToList();
            LboxZPatchDependenciesTo.DataSource = currZPatch.dependenciesTo.ToList();
        }

        private void BtSetHomePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                rm.homeDir = new DirectoryInfo(fbd.SelectedPath);
                IniUtils.IniUtils.SetConfig("Local", "Home", fbd.SelectedPath);
            }
        }

        private void BtAddRelease_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm();
            if(addForm.ShowDialog() == DialogResult.OK)
            {
                Release newRelease = rm.AddRelease(addForm.Value);
                if (newRelease != null)
                {
                    mainTree.Nodes.Add(newRelease.releaseId.ToString(), newRelease.releaseName);
                }
                
            }
        }

        private void BtAddCPatch_Click(object sender, EventArgs e)
        {
            Release currRelease = getReleaseFromTree(mainTree.SelectedNode);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                CPatch newCpatch = currRelease.AddCPatch(currRelease, new FileInfo(ofd.FileName));
                mainTree.SelectedNode.Nodes.Add(newCpatch.CPatchId.ToString(), newCpatch.CPatchName);
                mainTree.SelectedNode.Expand();
            }
        }

        private Release getReleaseFromTree(TreeNode node)
        {
            return rm.releasesDict[int.Parse(node.Name)];
        }

        private CPatch getCPatchFromTree(TreeNode node)
        {
            currRelease = getReleaseFromTree(node.Parent);
            return currRelease.CPatchesDict[int.Parse(node.Name)];
        }

        private ZPatch getZPatchFromTree(TreeNode node)
        {
            currCPatch = getCPatchFromTree(node.Parent);
            return currCPatch.ZPatchesDict[int.Parse(node.Name)];
        }

        private void mainTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(mainTree.SelectedNode.Level == 0)
            {
                if(rm.homeDir == null)
                {
                    MessageBox.Show("Задайте домашнюю папку перед продолжением работы", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Release currRelease = getReleaseFromTree(mainTree.SelectedNode);
                currRelease.InitCPatches();
                mainTree.SelectedNode.Nodes.Clear();
                foreach (CPatch cPatch in currRelease.cpatches)
                {
                    TreeNode newNode = mainTree.SelectedNode.Nodes.Add(cPatch.CPatchId.ToString(), cPatch.CPatchName);

                    foreach (ZPatch zPatch in cPatch.zpatches)
                    {
                        newNode.Nodes.Add(zPatch.ZPatchId.ToString(), zPatch.ZPatchName);
                    }
                }
            }
            mainTree.SelectedNode.Expand();
        }

        private void CbCPatchStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index == actualCPatchStatusIndex)
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            }
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, Brushes.Black, new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private void CbZPatchStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index == actualZPatchStatusIndex)
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            }
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, Brushes.Black, new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private void mainTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (mainTree.SelectedNode.Level)
            {
                case 0:
                    currRelease = getReleaseFromTree(mainTree.SelectedNode);
                    break;
                case 1:
                    currCPatch = getCPatchFromTree(mainTree.SelectedNode);
                    actualCPatchStatusIndex = CbCPatchStatus.Items.IndexOf(currCPatch.CPatchStatus);
                    DisplayCPatch();
                    break;
                case 2:
                    currZPatch = getZPatchFromTree(mainTree.SelectedNode);
                    actualZPatchStatusIndex = CbZPatchStatus.Items.IndexOf(currZPatch.ZPatchStatus);
                    DisplayZPatch();
                    break;
            }
            mainTree.LabelEdit = false;
        }

        private void BtCPatchStatus_Click(object sender, EventArgs e)
        {
            CPatchStatuses newStatus = (CPatchStatuses)CbCPatchStatus.SelectedItem;
            if (newStatus != currCPatch.CPatchStatus)
            {
                currCPatch.UpdateStatus(newStatus);
                actualCPatchStatusIndex = CbCPatchStatus.Items.IndexOf(newStatus);
            }
        }

        private void BtZPatchStatus_Click(object sender, EventArgs e)
        {
            ZPatchStatuses newStatus = (ZPatchStatuses)CbZPatchStatus.SelectedItem;
            if (newStatus != currZPatch.ZPatchStatus)
            {
                currZPatch.UpdateStatus(newStatus);
                actualZPatchStatusIndex = CbZPatchStatus.Items.IndexOf(newStatus);
            }
        }

        private void mainTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                e.Node.ContextMenuStrip = mainTreeContextMenu;
            }
        }

        private void mainTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {

            switch (mainTree.SelectedNode.Level)
            {
                case 0:
                    getReleaseFromTree(mainTree.SelectedNode).Rename(e.Label);
                    break;
                case 1:
                    getCPatchFromTree(mainTree.SelectedNode).Rename(e.Label);
                    break;
                case 2:
                    getZPatchFromTree(mainTree.SelectedNode).Rename(e.Label);
                    break;

            }
        }

        private void CbCPatchRelease_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index == ((ComboBox)sender).Items.IndexOf(currCPatch.release))
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            }
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, Brushes.Black, new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private void CbZPatchCPatch_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index == ((ComboBox)sender).Items.IndexOf(currZPatch.cpatch))
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            }
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, Brushes.Black, new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private void BtReleaseGraph_Click(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph releaseGraph = currRelease.DrawGraph();
            GraphForm rgf = new GraphForm(releaseGraph);
            rgf.ShowDialog();
        }

        private void BtCPatchGraph_Click(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph cpatchGraph = currCPatch.DrawGraph();
            GraphForm rgf = new GraphForm(cpatchGraph);
            rgf.ShowDialog();
        }

        private void BtZPatchOrder_Click(object sender, EventArgs e)
        {
            ZPatchOrderForm zpof = new ZPatchOrderForm(currCPatch);
            zpof.ShowDialog();
        }

        private void BtDeleteRelease_Click(object sender, EventArgs e)
        {
            currRelease.Delete();
            mainTree.SelectedNode.Remove();
        }

        private void BtCPatchDelete_Click(object sender, EventArgs e)
        {
            currCPatch.Delete();
            mainTree.SelectedNode.Remove();
        }

        private void BtZPatchDelete_Click(object sender, EventArgs e)
        {
            currZPatch.Delete();
            mainTree.SelectedNode.Remove();
        }

        private void BtMoveCPatch_Click(object sender, EventArgs e)
        {
            ReleasesListForm rlf = new ReleasesListForm(rm.releases);
            if(rlf.ShowDialog() == DialogResult.OK)
            {
                TreeNode nodeToMove = mainTree.SelectedNode;
                mainTree.Nodes.Remove(nodeToMove);

                currCPatch.Move(rlf.release);

                mainTree.Nodes[rlf.release.releaseId.ToString()].Nodes.Add(nodeToMove);
            }

        }

        private void BtZPatchMove_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currRelease.cpatches);
            if (clf.ShowDialog() == DialogResult.OK)
            {
                TreeNode nodeToMove = mainTree.SelectedNode;
                mainTree.Nodes.Remove(nodeToMove);

                currZPatch.Move(clf.cpatch);

                mainTree.Nodes[clf.cpatch.CPatchId.ToString()].Nodes.Add(nodeToMove);
            }
        }

        private void BtCPatchDeleteDependenciesFrom_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currCPatch.dependenciesFrom);
            if(clf.ShowDialog() == DialogResult.OK)
            {
                currCPatch.DeleteDependencyFrom(clf.cpatch);
                LboxCPatchDependenciesFrom.Items.Remove(clf.cpatch);
            }
        }

        private void BtCPatchAddDependenciesFrom_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currCPatch.dependenciesFrom);
            if (clf.ShowDialog() == DialogResult.OK)
            {
                currCPatch.AddDependencyFrom(clf.cpatch);
                LboxCPatchDependenciesFrom.Items.Add(clf.cpatch);
            }
        }

        private void BtCPatchDeleteDependenciesTo_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currCPatch.dependenciesTo);
            if (clf.ShowDialog() == DialogResult.OK)
            {
                currCPatch.DeleteDependencyTo(clf.cpatch);
                LboxCPatchDependenciesTo.Items.Remove(clf.cpatch);
            }
        }

        private void BtCPatchAddDependenciesTo_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currCPatch.dependenciesTo);
            if (clf.ShowDialog() == DialogResult.OK)
            {
                currCPatch.AddDependencyTo(clf.cpatch);
                LboxCPatchDependenciesTo.Items.Add(clf.cpatch);
            }
        }

        private void BtZPatchDeleteDependenciesFrom_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(currZPatch.dependenciesFrom);
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                currZPatch.DeleteDependencyFrom(zlf.zpatch);
                LboxZPatchDependenciesFrom.Items.Remove(zlf.zpatch);
            }
        }

        private void BtZPatchAddDependenciesFrom_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(currZPatch.dependenciesFrom);
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                currZPatch.AddDependencyFrom(zlf.zpatch);
                LboxZPatchDependenciesFrom.Items.Add(zlf.zpatch);
            }
        }

        private void BtZPatchDeleteDependenciesTo_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(currZPatch.dependenciesTo);
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                currZPatch.DeleteDependencyTo(zlf.zpatch);
                LboxZPatchDependenciesTo.Items.Remove(zlf.zpatch);
            }
        }

        private void BtZPatchAddDependenciesTo_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(currZPatch.dependenciesTo);
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                currZPatch.AddDependencyTo(zlf.zpatch);
                LboxZPatchDependenciesTo.Items.Add(zlf.zpatch);
            }
        }
    }
}
