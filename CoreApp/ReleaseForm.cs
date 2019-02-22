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
        string home;
        string CVSDBName;
        CVS.VSS cvs;

        Microsoft.Office.Interop.Excel.Application excelApp;
        Release currRelease;
        SortedList<string, Release> releases;

        public ReleaseForm()
        {
            InitializeComponent();
            releases = new SortedList<string, Release>();

            home = IniUtils.IniUtils.GetConfig("Credentials", "Home");

            //CVSDBName = IniUtils.IniUtils.GetConfig("Credentials", "CVSDBName");

            Release.excel = excelApp;
            Application.Idle += OnIdle;
        }

        private bool CheckHomeDir()
        {
            return Directory.Exists(home);
        }

        private bool CheckCVSDBName()
        {
            try
            {
                cvs = new CVS.VSS(CVSDBName, Environment.UserName);
                cvs.Connect();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool InitHomeDir()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Задать домашнюю папку";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                home = fbd.SelectedPath;
                return AutoInitHomeDir();
            }
            else
            {
                return false;
            }
        }

        private bool AutoInitHomeDir()
        {
            if (CheckHomeDir())
            {
                IniUtils.IniUtils.SetConfig("Credentials", "Home", home);
                return true;
            }
            else
            {
                return InitHomeDir();
            }
        }

        private bool InitCVSDBName()
        {
            AddForm setCVSPath = new AddForm();
            setCVSPath.Text = "Задать адрес базы всс";
            if (setCVSPath.ShowDialog() == DialogResult.OK)
            {
                CVSDBName = setCVSPath.Value;
                return AutoInitCVSDBName();
            }
            else
            {
                return false;
            }
        }

        private bool AutoInitCVSDBName()
        {
            if (CheckCVSDBName())
            {
                IniUtils.IniUtils.SetConfig("Credentials", "CVSDBName", CVSDBName);
                return true;
            }
            else
            {
                return InitCVSDBName();
            }
        }

        private void UpdateReleasesBox()
        {
            LBoxFixpacks.Items.Clear();
            LBoxReleases.Items.Clear();

            foreach (string releaseName in releases.Keys)
            {
                LBoxReleases.Items.Add(releaseName);
            }
        }

        private void UpdateFixpacksBox()
        {
            LBoxFixpacks.Items.Clear();
            if (LBoxReleases.SelectedIndex != -1)
            {
                currRelease = releases[(string)LBoxReleases.SelectedItem];
                foreach (string fpName in currRelease.CPatches.Keys)
                {
                    LBoxFixpacks.Items.Add(fpName);
                }
            }
        }

        private void InitFromLocal()
        {
            if (AutoInitHomeDir())
            {
                DirectoryInfo homeDir = new DirectoryInfo(home);
                foreach(var subdir in homeDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    Release currRelease = new Release(subdir);
                    releases.Add(currRelease.name, currRelease);
                }
                UpdateReleasesBox();
            }
        }

        private void OnIdle(object sender, EventArgs args)
        {
            BtDeleteRelease.Enabled =
            BtCheckRelease.Enabled = 
            BtAddFixpack.Enabled = LBoxReleases.SelectedIndex != -1;            
        }


        private void AddRelease(string name)
        {
            if (Directory.Exists(home))
            {
                Release release = new Release(name);

                DirectoryInfo releaseDir = Directory.CreateDirectory(string.Join("\\", home, release.name));

                release.SetLocalDir(releaseDir);

                releases.Add(release.name, release);

                UpdateReleasesBox();

            }

            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Домашняя папка";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    home = fbd.SelectedPath;
                    IniUtils.IniUtils.SetConfig("Credentials", "Home", home);

                    AddRelease(name);
                }
            }
        }

        private void BtAddRelease_Click(object sender, EventArgs e)
        {
            AddForm addReleaseForm = new AddForm();

            if (addReleaseForm.ShowDialog() == DialogResult.OK)
            {
                AddRelease(addReleaseForm.Value);
            }
        }

        private void LBoxReleases_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFixpacksBox();
        }

        private void ConnectToCVS()
        {
            if (cvs == null)
            {
                try
                {
                    cvs = new CVS.VSS(CVSDBName, Environment.UserName);
                    cvs.Connect();
                    IniUtils.IniUtils.SetConfig("Credentials", "CVSDBName", CVSDBName);
                }
                catch
                {
                    AddForm addCVSFrom = new AddForm();
                    if (addCVSFrom.ShowDialog() == DialogResult.OK)
                    {
                        CVSDBName = addCVSFrom.Value;
                        ConnectToCVS();
                    }
                }
            }
        }

        private void BtAddFixpack_Click(object sender, EventArgs e)
        {
            AddForm addFixpackForm = new AddForm();

            if (addFixpackForm.ShowDialog() == DialogResult.OK)
            {
                ConnectToCVS();
                currRelease.SetCVS(cvs);
                currRelease.LoadFixpackFromCVS(addFixpackForm.Value);
                UpdateFixpacksBox();
            }
        }


        private void ReleaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (excelApp != null)
            {
                excelApp.Quit();
                Marshal.FinalReleaseComObject(excelApp);
            }
            if (cvs != null)
            {
                cvs.Close();
            }
        }

        private const int BORDER = 5;

        private void ReleaseForm_Resize(object sender, EventArgs e)
        {
            LBoxReleases.Height = ClientSize.Height - LBoxReleases.Top - BORDER;

            LBoxFixpacks.Width = ClientSize.Width - LBoxFixpacks.Left - BORDER;
            LBoxFixpacks.Height = ClientSize.Height - LBoxFixpacks.Top - BORDER;
        }

        private void BtLoadFromCVS_Click(object sender, EventArgs e)
        {
            AddForm releaseNameForm = new AddForm();
            releaseNameForm.Text = "Введите название релиза";
            if (releaseNameForm.ShowDialog() == DialogResult.OK)
            {
                string strRegex = string.Concat(".*", Regex.Escape(releaseNameForm.Value), ".*");
                Regex regex = new Regex(strRegex);
                ConnectToCVS();
                DirectoryInfo releaseDir = Directory.CreateDirectory(string.Join("\\", home, releaseNameForm.Value));
                Release release = new Release(releaseNameForm.Value, releaseDir, cvs, regex);
                releases.Add(release.name, release);
                UpdateReleasesBox();
            }
        }

        private void BtSetHomePath_Click(object sender, EventArgs e)
        {
            InitHomeDir();
        }

        private void BtSetCVSPath_Click(object sender, EventArgs e)
        {
            InitCVSDBName();
        }

        private void BtLoadFromLocal_Click(object sender, EventArgs e)
        {
            InitFromLocal();
        }

        private void BtCheckRelease_Click(object sender, EventArgs e)
        {
            if(excelApp == null)
            { 
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                Release.excel = excelApp;
            }

            CheckReleaseForm checkReleaseForm = new CheckReleaseForm(currRelease);
            currRelease.SetAllDependencies();
            checkReleaseForm.ShowDialog();
        }

        private void BtDeleteRelease_Click(object sender, EventArgs e)
        {
            currRelease.DeleteLocal();
            releases.Remove(currRelease.name);
            UpdateReleasesBox();
        }
    }
}
