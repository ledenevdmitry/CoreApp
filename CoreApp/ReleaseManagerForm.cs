using CoreApp.FixpackObjects;
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

            ResizeForm();

            CbStatus.DataSource = Enum.GetValues(typeof(CPatchStatuses));

            rm = new ReleaseManager();
            CreateTree();
            Application.Idle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtAddFixpack.Enabled = mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 0;
            GbCPatch.Visible = mainTree.SelectedNode.Level == 1;
        }

        private void CreateTree()
        {
            foreach(Release release in rm.releases)
            {
                var currReleaseNode = mainTree.Nodes.Add(release.releaseId.ToString(), release.releaseName);
            }
        }

        private void ResizeForm()
        {
            mainTree.Width = SCMain.Panel1.Width;
            mainTree.Height = SCMain.Panel1.Height;

            GbCPatch.Width = SCMain.Panel2.Width;
            GbCPatch.Height = SCMain.Panel2.Height;
        }

        private void mainSplitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeForm();
        }

        private void DisplayCPatch(CPatch cpatch)
        {
            CbStatus.SelectedItem = cpatch.CPatchStatus;

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
                rm.AddRelease(addForm.Value);
            }
        }

        private void BtAddFixpack_Click(object sender, EventArgs e)
        {
            Release currRelease = getReleaseFromTree(mainTree.SelectedNode);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                currRelease.AddCPatch(currRelease, new FileInfo(ofd.FileName));
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
            switch(mainTree.SelectedNode.Level)
            {
                case 0:
                    Release currRelease = getReleaseFromTree(mainTree.SelectedNode);
                    currRelease.InitCPatches();
                    mainTree.SelectedNode.Nodes.Clear();
                    foreach (CPatch cPatch in currRelease.CPatches)
                    {
                        mainTree.SelectedNode.Nodes.Add(cPatch.CPatchId.ToString(), cPatch.CPatchName);
                    }
                    break;
                case 1:
                    CPatch currCPatch = getCPatchFromTree(mainTree.SelectedNode);
                    currCPatch.InitZPatches();
                    mainTree.SelectedNode.Nodes.Clear();
                    foreach (ZPatch zPatch in currCPatch.ZPatches)
                    {
                        mainTree.SelectedNode.Nodes.Add(zPatch.ZPatchId.ToString(), zPatch.ZPatchName);
                    }
                    break;
            }
            mainTree.SelectedNode.Expand();
        }

        private void CbStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index == actualCPatchStatusIndex)
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
                    actualCPatchStatusIndex = CbStatus.Items.IndexOf(currCPatch.CPatchStatus);
                    break;
                case 2:
                    currZPatch = getZPatchFromTree(mainTree.SelectedNode);
                    break;
            }
        }

        private void BtStatus_Click(object sender, EventArgs e)
        {
            CPatchStatuses newStatus = (CPatchStatuses)CbStatus.SelectedItem;
            if (newStatus != currCPatch.CPatchStatus)
            {
                currCPatch.UpdateStatus(newStatus);
                actualCPatchStatusIndex = CbStatus.Items.IndexOf(newStatus);
            }
        }
    }
}
