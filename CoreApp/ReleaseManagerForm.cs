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
        public ReleaseManagerForm()
        {
            InitializeComponent();
            mainTree.Width = mainSplitter.Panel1.Width;
            mainDGV.Width = mainSplitter.Panel2.Width;
            rm = new ReleaseManager();
            CreateTree();
            Application.Idle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtAddFixpack.Enabled = mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 0;
        }

        private void CreateTree()
        {
            foreach(Release release in rm.releases)
            {
                var currReleaseNode = mainTree.Nodes.Add(release.releaseId.ToString(), release.releaseName);
            }
        }

        private void mainSplitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            mainTree.Width = mainSplitter.Panel1.Width;
            mainTree.Height = mainSplitter.Panel1.Height;

            mainDGV.Width = mainSplitter.Panel2.Width;
            mainTree.Height = mainSplitter.Panel2.Height;
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
            Release currRelease = getReleaseFromTree(node.Parent);
            return currRelease.CPatchesDict[int.Parse(node.Name)];
        }

        private void mainTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch(mainTree.SelectedNode.Level)
            {
                case 0:
                    Release currRelease = getReleaseFromTree(mainTree.SelectedNode);
                    currRelease.InitCPatches();
                    foreach (CPatch cPatch in currRelease.CPatches)
                    {
                        mainTree.SelectedNode.Nodes.Clear();
                        mainTree.SelectedNode.Nodes.Add(cPatch.CPatchId.ToString(), cPatch.CPatchName);
                    }
                    break;
                case 1:
                    CPatch currCPatch = getCPatchFromTree(mainTree.SelectedNode);
                    currCPatch.InitZPatches();
                    foreach (ZPatch zPatch in currCPatch.ZPatches)
                    {
                        mainTree.SelectedNode.Nodes.Clear();
                        mainTree.SelectedNode.Nodes.Add(zPatch.ZPatchId.ToString(), zPatch.ZPatchName);
                    }
                    break;
            }
        }
    }
}
