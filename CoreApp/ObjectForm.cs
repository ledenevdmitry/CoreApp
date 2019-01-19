using CoreApp.Dicts;
using CoreApp.FixpackObjects;
using CoreApp.FormUtils;
using CoreApp.Parsers;
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
    public partial class ObjectForm : Form
    {
        public ObjectForm()
        {
            InitializeComponent();
            Application.Idle += OnIdle;
            FormResize();
            //fileScs = new List<FileInfo>();
        }
        
        //List<FileInfo> fileScs;
        Thread checkThread;
        ETLParser etlparser;

        private void OnIdle(object sender, EventArgs e)
        {
            CheckAllFixpacksInDir.Enabled = checkThread == null || !checkThread.IsAlive;
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

        private void PrepareInfaParser()
        {
            /*
            etlparser.infaParser.StartOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = true));
            etlparser.infaParser.ProgressChanged += () => PBChecks.Invoke(new Action(() => PBChecks.Value++));
            etlparser.infaParser.EndOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = false));
            */
        }

        private void PrepareOraParser()
        {
            /*
            etlparser.sqlParser.StartOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = true));
            etlparser.sqlParser.ProgressChanged += () => PBChecks.Invoke(new Action(() => PBChecks.Value++));
            etlparser.sqlParser.EndOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = false));
            */
        }

        private void PrepareParsers()
        {
            PrepareInfaParser();
            PrepareOraParser();
        }

        private void TSMIFileScPrereq_Click(object sender, EventArgs e)
        {

        }

        private const int BORDER = 5;

        private void FormResize()
        {
            PBChecks.Left = Width - PBChecks.Width;
            mainTabControl.Width = DisplayRectangle.Width;
            mainTabControl.Height = DisplayRectangle.Height - MainMenu.Height;

            dgvObjects.Width = mainTabControl.DisplayRectangle.Width - BORDER;
            dgvObjects.Height = mainTabControl.DisplayRectangle.Height - BORDER;

            dgvIntersections.Width = mainTabControl.DisplayRectangle.Width  - BORDER;
            dgvIntersections.Height = mainTabControl.DisplayRectangle.Height - BORDER;

            dgvAllDependencies.Width = mainTabControl.DisplayRectangle.Width  - BORDER;
            dgvAllDependencies.Height = mainTabControl.DisplayRectangle.Height - BORDER;

            dgvLostDependencies.Width = mainTabControl.DisplayRectangle.Width  - BORDER;
            dgvLostDependencies.Height = mainTabControl.DisplayRectangle.Height - BORDER;

            dgvNotFoundFiles.Width = mainTabControl.DisplayRectangle.Width - BORDER;
            dgvNotFoundFiles.Height = mainTabControl.DisplayRectangle.Height - BORDER;
        }
        
        private void Form1_Resize(object sender, EventArgs e)
        {
            FormResize();
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as TabControl).SelectedTab.Controls[0] as DataGridView).AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }       

        private void CheckAllFixpacksInDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                //List<Fixpack> fixpacks;
                //List<FileInfo> files = FileScUtils.GetFilesFromMainDir(new DirectoryInfo(fbd.SelectedPath), out fixpacks);
                etlparser = new ETLParser(new DirectoryInfo(fbd.SelectedPath), UMEnabled);

                PBChecks.Value = 0;
                PBChecks.Maximum = etlparser.infaParser.WorkAmount(etlparser.infaObjectDict) + etlparser.fileCount();

                PrepareParsers();

                checkThread = new Thread(() =>
                {
                    etlparser.Check(UMEnabled);
                    _checked = true;
                });
                checkThread.Start();
            }
        }

        private void TSMICheckScList_Click(object sender, EventArgs e)
        {

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
        
    }
}
