using CoreApp.Dicts;
using CoreApp.FormUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
            fileScs = new List<FileInfo>();
        }

        List<FileInfo> fileScs;
        Thread checkThread;
        InfaParser infaParser;
        SqlParser sqlParser;
        InfaObjectDict infaDict;
        OraObjectDict oraDict;

        private void OnIdle(object sender, EventArgs e)
        {
            CheckAllFixpacksInDir.Enabled = checkThread == null || !checkThread.IsAlive;
            if(_checked)
            {
                _checked = false;

                FormUtil.InitObjDGV(dgvObjects);
                FormUtil.InitObjDGV(dgvIntersections);
                FormUtil.InitObjDGV(dgvWrongOrder);
                FormUtil.InitObjDGV(dgvNotFound);
                FormUtil.InitNotFoundFilesDGV(dgvNotFoundFiles);

                FormUtil.AddOraObjectsInDGV(dgvObjects, oraDict);
                FormUtil.AddInfaObjectsInDGV(dgvObjects, infaDict);
                FormUtil.AddOraIntersectionsInDGV(dgvIntersections, oraDict);
                FormUtil.AddInfaIntersectionsInDGV(dgvIntersections, infaDict);
                FormUtil.AddInfaWrongOrderInDGV(dgvWrongOrder, infaDict);
                FormUtil.AddInfaNotFoundObjectsInDGV(dgvNotFound, infaDict);
                FormUtil.AddNotFoundFiles(dgvNotFoundFiles, oraDict);
                FormUtil.AddNotFoundFiles(dgvNotFoundFiles, infaDict);
            }
        }

        bool _checked = false;

        private void PrepareInfaParser()
        {
            infaParser.StartOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = true));            
            infaParser.ProgressChanged += () => PBChecks.Invoke(new Action(() => PBChecks.Value++));            
            infaParser.EndOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = false));
        }

        private void PrepareOraParser()
        {
            sqlParser.StartOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = true));
            sqlParser.ProgressChanged += () => PBChecks.Invoke(new Action(() => PBChecks.Value++));
            sqlParser.EndOfCheck += () => PBChecks.Invoke(new Action(() => PBChecks.Visible = false));
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

            dgvWrongOrder.Width = mainTabControl.DisplayRectangle.Width  - BORDER;
            dgvWrongOrder.Height = mainTabControl.DisplayRectangle.Height - BORDER;

            dgvNotFound.Width = mainTabControl.DisplayRectangle.Width  - BORDER;
            dgvNotFound.Height = mainTabControl.DisplayRectangle.Height - BORDER;

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

        private void TSMIAddScIntoList_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы сценария (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileScs.Add(new FileInfo(ofd.FileName));
            }
        }

        private void CheckAllFixpacksInDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                List<FileInfo> files = FileScUtils.GetFilesFromMainDir(new DirectoryInfo(fbd.SelectedPath));
                infaDict = new InfaObjectDict();
                oraDict = new OraObjectDict();

                infaParser = new InfaParser(files, infaDict);
                sqlParser = new SqlParser();

                PBChecks.Value = 0;
                PBChecks.Maximum = infaParser.WorkAmount(infaDict) + sqlParser.WorkAmoumt(files);

                PrepareParsers();

                checkThread = new Thread(() =>
                {
                    sqlParser.RetrieveObjectsFromFile(files, oraDict, UMEnabled);
                    infaParser.RetrieveObjectsFromFiles(files, infaDict);
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
