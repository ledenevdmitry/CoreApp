using CoreApp.Forms;
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

        Release CurrRelease { get => GetReleaseFromTree(mainTree.SelectedNode); }
        CPatch  CurrCPatch  { get => GetCPatchFromTree (mainTree.SelectedNode); }
        ZPatch  CurrZPatch  { get => GetZPatchFromTree (mainTree.SelectedNode); }

        CVS.CVS cvs;

        public ReleaseManagerForm()
        {
            InitializeComponent();

            CbCPatchStatus.Items.AddRange(CPatch.cpatchStatuses.ToArray());
            CbEnvCode.Items.AddRange(CPatch.cpatchEnvCodes.ToArray());

            cvs = new CVS.VSS(
                IniUtils.IniUtils.GetConfig("CVS", "location"),
                Environment.UserName);

            Release.cvs = CPatch.cvs = ZPatch.cvs = cvs;

            CbZPatchStatus.Items.AddRange(ZPatch.zpatchStatuses.ToArray());

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
            BtCPatchCreateScenario.Enabled =
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
                mainTree.Nodes.Add(release.ReleaseId.ToString(), release.ReleaseName);                
            }
        }

        private void DisplayCPatch()
        {
            CbCPatchStatus.SelectedItem = CurrCPatch.CPatchStatus;
            CbEnvCode.SelectedItem = CurrCPatch.KodSredy;

            LboxCPatchDependenciesFrom.Items.Clear();
            foreach (var dep in CurrCPatch.DependenciesFrom)
            {
                LboxCPatchDependenciesFrom.Items.Add(dep);
            }

            LboxCPatchDependenciesTo.Items.Clear();
            foreach (var dep in CurrCPatch.DependenciesTo)
            {
                LboxCPatchDependenciesTo.Items.Add(dep);
            }
        }

        private void DisplayZPatch()
        {
            CbZPatchStatus.SelectedItem = CurrZPatch.ZPatchStatus;

            LboxZPatchDependenciesFrom.Items.Clear();
            foreach(var dep in CurrZPatch.DependenciesFrom)
            {
                LboxZPatchDependenciesFrom.Items.Add(dep);
            }

            LboxZPatchDependenciesTo.Items.Clear();
            foreach (var dep in CurrZPatch.DependenciesTo)
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
                    mainTree.Nodes.Add(newRelease.ReleaseId.ToString(), newRelease.ReleaseName);
                }
                
            }
        }

        private void BtAddCPatch_Click(object sender, EventArgs e)
        {
            Release currRelease = GetReleaseFromTree(mainTree.SelectedNode);
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CPatch newCPatch = currRelease.AddCPatch(currRelease, new FileInfo(ofd.FileName));
                TreeNode cpatchNode = mainTree.SelectedNode.Nodes.Add(newCPatch.CPatchId.ToString(), newCPatch.CPatchName);
                foreach(ZPatch zpatch in newCPatch.ZPatches)
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

        private Release GetReleaseFromTree(TreeNode node)
        {
            return rm.releasesDict[int.Parse(GetReleaseNode(node).Name)];
        }

        private CPatch GetCPatchFromTree(TreeNode node)
        {
            return GetReleaseFromTree(GetReleaseNode(node)).CPatchesDict[int.Parse(GetCPatchNode(node).Name)];
        }

        private ZPatch GetZPatchFromTree(TreeNode node)
        {
           return GetCPatchFromTree(GetCPatchNode(node)).ZPatchesDict[int.Parse(GetZPatchNode(node).Name)];
        }

        private void MainTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(mainTree.SelectedNode.Level == 0)
            {
                if(rm.homeDir == null)
                {
                    MessageBox.Show("Задайте домашнюю папку перед продолжением работы", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Release currRelease = GetReleaseFromTree(mainTree.SelectedNode);
                currRelease.InitCPatches();
                mainTree.SelectedNode.Nodes.Clear();

                foreach (CPatch cPatch in currRelease.CPatches)
                {
                    TreeNode newNode = mainTree.SelectedNode.Nodes.Add(cPatch.CPatchId.ToString(), cPatch.CPatchName);

                    foreach (ZPatch zPatch in cPatch.ZPatches)
                    {
                        newNode.Nodes.Add(zPatch.ZPatchId.ToString(), zPatch.ZPatchName);
                    }
                }
            }
            mainTree.SelectedNode.Expand();
        }

        private void MainTree_AfterSelect(object sender, TreeViewEventArgs e)
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
            string newStatus = (string)CbCPatchStatus.SelectedItem;
            CurrCPatch.UpdateStatus(newStatus);
        }

        private void BtZPatchStatus_Click(object sender, EventArgs e)
        {
            string newStatus = (string)CbZPatchStatus.SelectedItem;
            CurrZPatch.UpdateStatus(newStatus);
        }

        private void MainTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            switch (editing.Level)
            {
                case 0:
                    GetReleaseFromTree(mainTree.SelectedNode).Rename(e.Label);
                    break;
                case 1:
                    GetCPatchFromTree(mainTree.SelectedNode).Rename(e.Label);
                    break;
                case 2:
                    GetZPatchFromTree(mainTree.SelectedNode).Rename(e.Label);
                    break;
            }
        }

        private void BtReleaseGraph_Click(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph releaseGraph = CurrRelease.DrawGraph();
            GraphForm rgf = new GraphForm(releaseGraph);
            rgf.ShowDialog();
        }

        private void BtCPatchGraph_Click(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph cpatchGraph = CurrCPatch.DrawGraph();
            GraphForm rgf = new GraphForm(cpatchGraph);
            rgf.ShowDialog();
        }

        private void BtZPatchOrder_Click(object sender, EventArgs e)
        {
            ZPatchOrderForm zpof = new ZPatchOrderForm(CurrCPatch);
            zpof.ShowDialog();
        }

        private void BtDeleteRelease_Click(object sender, EventArgs e)
        {
            CurrRelease.Delete();
            mainTree.SelectedNode.Remove();
        }

        private void BtCPatchDelete_Click(object sender, EventArgs e)
        {
            CurrCPatch.Delete();
            mainTree.SelectedNode.Remove();
        }

        private void BtZPatchDelete_Click(object sender, EventArgs e)
        {
            CurrZPatch.Delete();
            mainTree.SelectedNode.Remove();
        }

        private void BtMoveCPatch_Click(object sender, EventArgs e)
        {
            ReleasesListForm rlf = new ReleasesListForm(
                rm.releases
                .Where(x => x != CurrRelease)
                .OrderBy(x => x.ReleaseName));

            if(rlf.ShowDialog() == DialogResult.OK)
            {
                TreeNode nodeToMove = mainTree.SelectedNode;
                CurrCPatch.Move(rlf.release);

                mainTree.Nodes.Remove(nodeToMove);

                mainTree.Nodes[rlf.release.ReleaseId.ToString()].Nodes.Add(nodeToMove);
            }

        }

        private void BtZPatchMove_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(CurrRelease.CPatches);
            if (clf.ShowDialog() == DialogResult.OK)
            {
                TreeNode nodeToMove = mainTree.SelectedNode;
                mainTree.Nodes.Remove(nodeToMove);

                CurrZPatch.Move(clf.cpatch);

                mainTree.Nodes[clf.cpatch.CPatchId.ToString()].Nodes.Add(nodeToMove);
            }
        }

        private void BtCPatchDeleteDependenciesFrom_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(CurrCPatch.DependenciesFrom);
            if(clf.ShowDialog() == DialogResult.OK)
            {
                if (CPatch.CanDeleteCPatchDependency(clf.cpatch, CurrCPatch, out ZPatch zpatchFrom, out ZPatch zpatchTo))
                {
                    CurrCPatch.DeleteDependencyFrom(clf.cpatch);
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
            CPatchesListForm clf = new CPatchesListForm(CurrRelease.CPatches.Where(x => x != CurrCPatch));
            if (clf.ShowDialog() == DialogResult.OK)
            {
                //если нет обратной зависимости
                if (!CPatch.HaveTransitiveDependency(CurrCPatch, clf.cpatch))
                {
                    CurrCPatch.AddDependencyFrom(clf.cpatch);
                    LboxCPatchDependenciesFrom.Items.Add(clf.cpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {CurrCPatch} -> {clf.cpatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtCPatchDeleteDependenciesTo_Click(object sender, EventArgs e)
        {
            CPatchesListForm clf = new CPatchesListForm(CurrCPatch.DependenciesTo);
            if (clf.ShowDialog() == DialogResult.OK)
            {
                if (CPatch.CanDeleteCPatchDependency(CurrCPatch, clf.cpatch, out ZPatch zpatchFrom, out ZPatch zpatchTo))
                {
                    CurrCPatch.DeleteDependencyTo(clf.cpatch);
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
            CPatchesListForm clf = new CPatchesListForm(CurrRelease.CPatches.Where(x => x != CurrCPatch));
            if (clf.ShowDialog() == DialogResult.OK)
            {
                //если нет обратной зависимости
                if (!CPatch.HaveTransitiveDependency(clf.cpatch, CurrCPatch))
                {
                    CurrCPatch.AddDependencyTo(clf.cpatch);
                    LboxCPatchDependenciesTo.Items.Add(clf.cpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {clf.cpatch} -> {CurrCPatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtZPatchDeleteDependenciesFrom_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(CurrZPatch.DependenciesFrom);
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                CurrZPatch.DeleteDependencyFrom(zlf.zpatch);
                LboxZPatchDependenciesFrom.Items.Remove(zlf.zpatch);
            }
        }

        private void BtZPatchAddDependenciesFrom_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(
                CurrRelease.CPatches
                .SelectMany(x => x.ZPatches)
                .Where(x => x != CurrZPatch)
                .OrderBy(x => x.ZPatchName));
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                //если нет обратной зависимости
                if (!ZPatch.HaveTransitiveDependency(CurrZPatch, zlf.zpatch))
                {
                    CurrZPatch.AddDependencyFrom(zlf.zpatch);
                    LboxZPatchDependenciesFrom.Items.Add(zlf.zpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {CurrZPatch} -> {zlf.zpatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtZPatchDeleteDependenciesTo_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(CurrZPatch.DependenciesTo);
            if (zlf.ShowDialog() == DialogResult.OK)
            {
                CurrZPatch.DeleteDependencyTo(zlf.zpatch);
                LboxZPatchDependenciesTo.Items.Remove(zlf.zpatch);
            }
        }

        private void BtZPatchAddDependenciesTo_Click(object sender, EventArgs e)
        {
            ZPatchesListForm zlf = new ZPatchesListForm(
                CurrRelease.CPatches
                .SelectMany(x => x.ZPatches)
                .Where(x => x != CurrZPatch)
                .OrderBy(x => x.ZPatchName));

            if (zlf.ShowDialog() == DialogResult.OK)
            {                
                //если нет обратной зависимости
                if (!ZPatch.HaveTransitiveDependency(zlf.zpatch, CurrZPatch))
                {
                    CurrZPatch.AddDependencyTo(zlf.zpatch);
                    LboxZPatchDependenciesTo.Items.Add(zlf.zpatch);
                }
                else
                {
                    MessageBox.Show($"Есть зависимость {zlf.zpatch} -> {CurrZPatch} (возможно, транзитивная)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtCreateScenario_Click(object sender, EventArgs e)
        {
            try
            {
                var scenario = CurrCPatch.CreateScenario();
                ScenarioForm sf = new ScenarioForm(scenario);
                sf.ShowDialog();
            }
            catch(DirectoryNotFoundException exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(IOException exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtEnvCode_Click(object sender, EventArgs e)
        {
            string newEnvCode = (string)CbEnvCode.SelectedItem;
            CurrCPatch.UpdateEnvCode(newEnvCode);
        }
    }
}
