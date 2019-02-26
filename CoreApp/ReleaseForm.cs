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
    public partial class ReleaseForm : Form
    {
        ReleaseManager rm;
        public ReleaseForm()
        {
            InitializeComponent();
            mainTree.Width = mainSplitter.Panel1.Width;
            mainDGV.Width = mainSplitter.Panel2.Width;
            rm = new ReleaseManager();
            CreateTree();
        }

        private void CreateTree()
        {
            foreach(Release release in rm.releases)
            {
                var currReleaseNode = mainTree.Nodes.Add(release.releaseId.ToString(), release.releaseName);
                foreach(CPatch cPatch in release.CPatches)
                {
                    var currCPatchNode = currReleaseNode.Nodes.Add(cPatch.CPatchId.ToString(), cPatch.CPatchName);
                    foreach(ZPatch zPatch in cPatch.ZPatches)
                    {
                        currCPatchNode.Nodes.Add(zPatch.ZPatchId.ToString(), zPatch.ZPatchName);
                    }
                }
            }
        }

        private void mainSplitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            mainTree.Width = mainSplitter.Panel1.Width;
            mainDGV.Width = mainSplitter.Panel2.Width;
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
    }
}
