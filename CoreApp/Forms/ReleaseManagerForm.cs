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

        TreeNode editing;

        Release currRelease { get => getReleaseFromTree(mainTree.SelectedNode); }
        CPatch  currCPatch  { get => getCPatchFromTree (mainTree.SelectedNode); }
        ZPatch  currZPatch  { get => getZPatchFromTree (mainTree.SelectedNode); }

        public ReleaseManagerForm()
        {
            InitializeComponent();            

            CbCPatchStatus.DataSource = Enum.GetValues(typeof(CPatchStatuses));
            CbZPatchStatus.DataSource = Enum.GetValues(typeof(ZPatchStatuses));

            rm = new ReleaseManager();

            CreateTree();
            Application.Idle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtAddCPatch.Enabled = 
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level >= 0;

            BtReleaseGraph.Enabled = 
            BtReleaseRename.Enabled =
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level >= 0;

            BtCPatchGraph.Enabled = 
            BtZPatchOrder.Enabled =
            BtCPatchRename.Enabled =
            BtCPatchMove.Enabled =
            BtCPatchDelete.Enabled =
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level >= 1;

            GbCPatch.Visible =
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 1;

            GbZPatch.Visible = 
            BtZPatchRename.Enabled =
            BtZPatchMove.Enabled =
            BtZPatchDelete.Enabled =
                mainTree.SelectedNode != null && mainTree.SelectedNode.Level == 2;

        }

        private void CreateTree()
        {
            foreach(Release release in rm.releases)
            {
                mainTree.Nodes.Add(release.releaseId.ToString(), release.releaseName);                
            }
        }

        private void DisplayCPatch()
        {
            CbCPatchStatus.SelectedItem = currCPatch.CPatchStatus;

            LboxCPatchDependenciesFrom.Items.Clear();
            foreach (var dep in currCPatch.dependenciesFrom)
            {
                LboxCPatchDependenciesFrom.Items.Add(dep);
            }

            LboxCPatchDependenciesTo.Items.Clear();
            foreach (var dep in currCPatch.dependenciesTo)
            {
                LboxCPatchDependenciesTo.Items.Add(dep);
            }
        }

        private void DisplayZPatch()
        {
            CbZPatchStatus.SelectedItem = currZPatch.ZPatchStatus;

            LboxZPatchDependenciesFrom.Items.Clear();
            foreach(var dep in currZPatch.dependenciesFrom)
            {
                LboxZPatchDependenciesFrom.Items.Add(dep);
            }

            LboxZPatchDependenciesTo.Items.Clear();
            foreach (var dep in currZPatch.dependenciesTo)
            {
                LboxZPatchDependenciesTo.Items.Add(dep);
            }
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
                CPatch newCPatch = currRelease.AddCPatch(currRelease, new FileInfo(ofd.FileName));
                TreeNode cpatchNode = mainTree.SelectedNode.Nodes.Add(newCPatch.CPatchId.ToString(), newCPatch.CPatchName);
                foreach(ZPatch zpatch in newCPatch.zpatches)
                {
                    cpatchNode.Nodes.Add(zpatch.ZPatchId.ToString(), zpatch.ZPatchName);
                }
            }
        }

        private TreeNode GetReleaseNode(TreeNode node)
        {
            switch (node.Level)
            {
                case 0:
                    return node;
                case 1:
                    return node.Parent;
                case 2:
                    return node.Parent.Parent;
            }
            throw new Exception("Нет такой ноды");
        }

        private TreeNode GetCPatchNode(TreeNode node)
        {
            switch (node.Level)
            {
                case 1:
                    return node;
                case 2:
                    return node.Parent;
            }
            throw new Exception("Нет такой ноды");
        }

        private TreeNode GetZPatchNode(TreeNode node)
        {
            switch (node.Level)
            {
                case 2:
                    return node;
            }
            throw new Exception("Нет такой ноды");
        }

        private Release getReleaseFromTree(TreeNode node)
        {
            return rm.releasesDict[int.Parse(GetReleaseNode(node).Name)];
        }

        private CPatch getCPatchFromTree(TreeNode node)
        {
            return getReleaseFromTree(GetReleaseNode(node)).CPatchesDict[int.Parse(GetCPatchNode(node).Name)];
        }

        private ZPatch getZPatchFromTree(TreeNode node)
        {
           return getCPatchFromTree(GetCPatchNode(node)).ZPatchesDict[int.Parse(GetZPatchNode(node).Name)];
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

        private void mainTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (mainTree.SelectedNode.Level)
            {
                case 1:
                    DisplayCPatch();
                    break;
                case 2:
                    DisplayZPatch();
                    break;
            }
            mainTree.LabelEdit = false;
        }

        private void BtCPatchStatus_Click(object sender, EventArgs e)
        {
            CPatchStatuses newStatus = (CPatchStatuses)CbCPatchStatus.SelectedItem;
            currCPatch.UpdateStatus(newStatus);
        }

        private void BtZPatchStatus_Click(object sender, EventArgs e)
        {
            ZPatchStatuses newStatus = (ZPatchStatuses)CbZPatchStatus.SelectedItem;
            currZPatch.UpdateStatus(newStatus);
        }

        private void mainTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            switch (editing.Level)
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
            ReleasesListForm rlf = new ReleasesListForm(
                rm.releases
                .Where(x => x != currRelease)
                .OrderBy(x => x.releaseName));

            if(rlf.ShowDialog() == DialogResult.OK)
            {
                TreeNode nodeToMove = mainTree.SelectedNode;
                currCPatch.Move(rlf.release);

                mainTree.Nodes.Remove(nodeToMove);

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
                ZPatch zpatchFrom, zpatchTo;
                if (CPatch.CanDeleteCPatchDependency(clf.cpatch, currCPatch, out zpatchFrom, out zpatchTo))
                {
                    currCPatch.DeleteDependencyFrom(clf.cpatch);
                    LboxCPatchDependenciesFrom.Items.Remove(clf.cpatch);
                }
                else
                {
                    MessageBox.Show($"Существует зависимость по Z-патчам: {zpatchFrom} -> {zpatchTo}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtCPatchAddDependenciesFrom_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currRelease.cpatches.Where(x => x != currCPatch));
            if (clf.ShowDialog() == DialogResult.OK)
            {
                //если нет обратной зависимости
                if (!CPatch.HaveTransitiveDependency(currCPatch, clf.cpatch))
                {
                    currCPatch.AddDependencyFrom(clf.cpatch);
                    LboxCPatchDependenciesFrom.Items.Add(clf.cpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {currCPatch} -> {clf.cpatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtCPatchDeleteDependenciesTo_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currCPatch.dependenciesTo);
            if (clf.ShowDialog() == DialogResult.OK)
            {
                ZPatch zpatchFrom, zpatchTo;
                if (CPatch.CanDeleteCPatchDependency(currCPatch, clf.cpatch, out zpatchFrom, out zpatchTo))
                {
                    currCPatch.DeleteDependencyTo(clf.cpatch);
                    LboxCPatchDependenciesTo.Items.Remove(clf.cpatch);
                }
                else
                {
                    MessageBox.Show($"Существует зависимость по Z-патчам: {zpatchFrom} -> {zpatchTo}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtCPatchAddDependenciesTo_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(currRelease.cpatches.Where(x => x != currCPatch));
            if (clf.ShowDialog() == DialogResult.OK)
            {
                //если нет обратной зависимости
                if (!CPatch.HaveTransitiveDependency(clf.cpatch, currCPatch))
                {
                    currCPatch.AddDependencyTo(clf.cpatch);
                    LboxCPatchDependenciesTo.Items.Add(clf.cpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {clf.cpatch} -> {currCPatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            ZPatchesListForm zlf = new ZPatchesListForm(
                currRelease.cpatches
                .SelectMany(x => x.zpatches)
                .Where(x => x != currZPatch)
                .OrderBy(x => x.ZPatchName));
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                //если нет обратной зависимости
                if (!ZPatch.HaveTransitiveDependency(currZPatch, zlf.zpatch))
                {
                    currZPatch.AddDependencyFrom(zlf.zpatch);
                    LboxZPatchDependenciesFrom.Items.Add(zlf.zpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {currZPatch} -> {zlf.zpatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            ZPatchesListForm zlf = new ZPatchesListForm(
                currRelease.cpatches
                .SelectMany(x => x.zpatches)
                .Where(x => x != currZPatch)
                .OrderBy(x => x.ZPatchName));

            if (zlf.ShowDialog() == DialogResult.OK)
            {                
                //если нет обратной зависимости
                if (!ZPatch.HaveTransitiveDependency(zlf.zpatch, currZPatch))
                {
                    currZPatch.AddDependencyTo(zlf.zpatch);
                    LboxZPatchDependenciesTo.Items.Add(zlf.zpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {zlf.zpatch} -> {currZPatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtReleaseRename_Click(object sender, EventArgs e)
        {
            mainTree.LabelEdit = true;
            editing = GetReleaseNode(mainTree.SelectedNode);
            editing.BeginEdit();
        }

        private void BtCPatchRename_Click(object sender, EventArgs e)
        {
            mainTree.LabelEdit = true;
            editing = GetCPatchNode(mainTree.SelectedNode);
            editing.BeginEdit();
        }

        private void BtZPatchRename_Click(object sender, EventArgs e)
        {
            mainTree.LabelEdit = true;
            editing = GetZPatchNode(mainTree.SelectedNode);
            editing.BeginEdit();
        }
    }
}
