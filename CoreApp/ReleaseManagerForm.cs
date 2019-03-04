﻿using CoreApp.FixpackObjects;
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

            CbCPatchStatus.DataSource = Enum.GetValues(typeof(CPatchStatuses));

            rm = new ReleaseManager();
            CbCPatchRelease.Items.AddRange(rm.releases.ToArray());

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
            BtAddFixpack.Enabled = mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 0;
            GbCPatch.Visible = mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 1;
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

            if (GbCPatch.Visible)
            {
                GbCPatch.Width = SCMain.Panel2.Width;
                GbCPatch.Height = SCMain.Panel2.Height;

                LboxCPatchDependenciesFrom.Width = SCCPatchDependenciesFrom.Panel1.Width;
                LboxCPatchDependenciesFrom.Height = SCCPatchDependenciesFrom.Panel1.Height - LbCPatchDependenciesFrom.Height;

                LboxCPatchDependenciesTo.Width = SCCPatchDependenciesTo.Panel1.Width;
                LboxCPatchDependenciesTo.Height = SCCPatchDependenciesTo.Panel1.Height - LbCPatchDependenciesTo.Height;

                //LboxCPatchDependenciesFrom.Height = SCCPatchDependencies.Panel1.Height - LboxCPatchDependenciesFrom.Top - (SCCPatchDependencies.Panel1.Bottom - LboxCPatchDependenciesFrom.Bottom);
            }
        }

        private void mainSplitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeForm();
        }

        private void DisplayCPatch()
        {
            CbCPatchStatus.SelectedItem = currCPatch.CPatchStatus;
            CbCPatchRelease.SelectedItem = currCPatch.release;

            LboxCPatchDependenciesFrom.DataSource = currCPatch.dependenciesFrom.ToList();
            LboxCPatchDependenciesTo.DataSource = currCPatch.dependenciesTo.ToList();
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
                    CbCPatchRelease.Items.Add(newRelease);
                }
                
            }
        }

        private void BtAddFixpack_Click(object sender, EventArgs e)
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
                    actualCPatchStatusIndex = CbCPatchStatus.Items.IndexOf(currCPatch.CPatchStatus);
                    DisplayCPatch();
                    break;
                case 2:
                    currZPatch = getZPatchFromTree(mainTree.SelectedNode);
                    break;
            }
            mainTree.LabelEdit = false;
        }

        private void BtStatus_Click(object sender, EventArgs e)
        {
            CPatchStatuses newStatus = (CPatchStatuses)CbCPatchStatus.SelectedItem;
            if (newStatus != currCPatch.CPatchStatus)
            {
                currCPatch.UpdateStatus(newStatus);
                actualCPatchStatusIndex = CbCPatchStatus.Items.IndexOf(newStatus);
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

        private void BtCPatchRelease_Click(object sender, EventArgs e)
        {
            TreeNode cpatchNodeToMove = mainTree.SelectedNode;

            //удаляем старую ноду из дерева
            mainTree.Nodes.Remove(cpatchNodeToMove);

            Release newRelease = (Release)CbCPatchRelease.SelectedItem;
            currCPatch.ChangeRelease(newRelease);

            //добавляем ноду к новому релизу
            mainTree.Nodes[newRelease.releaseId.ToString()].Nodes.Add(cpatchNodeToMove);
            mainTree.SelectedNode = cpatchNodeToMove;
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
    }
}
