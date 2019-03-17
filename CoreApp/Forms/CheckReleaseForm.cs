using CoreApp.Dicts;
using CoreApp.FormUtils;
using CoreApp.Parsers;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreApp
{
    public partial class CheckReleaseForm : Form
    {
        public CheckReleaseForm(Release release)
        {
            InitializeComponent();
            Application.Idle += OnIdle;
            this.release = release;
            CheckRelease();
        }
        
        Thread checkThread;
        ETLParser etlparser;
        Release release;

        private void OnIdle(object sender, EventArgs e)
        {
            CheckAllCPatchesInDir.Enabled = checkThread == null || !checkThread.IsAlive;
            if(_checked)
            {
                _checked = false;

                FormUtil.InitObjDGV(dgvObjects);
                FormUtil.InitObjDGV(dgvIntersections);
                FormUtil.InitObjDGV(dgvAllDependencies);
                FormUtil.InitObjDGV(dgvLostDependencies);
                FormUtil.InitNotFoundFilesDGV(dgvNotFoundFiles);

                FormUtil.AddObjectsInDGV(dgvObjects, etlparser);
                FormUtil.AddIntersectionsInDGV(dgvIntersections, etlparser);
                FormUtil.AddAllDependenciesInDGV(dgvAllDependencies, etlparser);
                FormUtil.AddLostDependenciesInDGV(dgvLostDependencies, etlparser);
                FormUtil.AddNotFoundFiles(dgvNotFoundFiles, etlparser);
            }
        }

        bool _checked = false;

        private void TSMIFileScPrereq_Click(object sender, EventArgs e)
        {

        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as TabControl).SelectedTab.Controls[0] as DataGridView).AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }       

        private void CheckRelease()
        {
            etlparser = new ETLParser(release);

            checkThread = new Thread(() =>
            {
                etlparser.Check(UMEnabled);
                _checked = true;
            });
            checkThread.Start();

        }

        private bool UMEnabled = false;

        private void TSMIUmState_Click(object sender, EventArgs e)
        {
            if(UMEnabled)
            {
                UMEnabled = false;
                TSMIUmState.Text = "Учитывать УМ";
            }
            else
            {
                UMEnabled = true;
                TSMIUmState.Text = "Не учитывать УМ";
            }
        }

        private void CheckAllCPatchesInDir_Click(object sender, EventArgs e)
        {
            CheckRelease();
        }
    }
}
