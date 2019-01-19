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

        public ReleaseForm()
        {
            InitializeComponent();
            releases = new SortedList<string, Release>();
            fixpacks = new SortedList<string, Fixpack>();

            home = IniUtils.IniUtils.GetConfig("Credentials", "Home");
            CVSDBName = IniUtils.IniUtils.GetConfig("Credentials", "CVSDBName");

            Release.excel = excelApp;
            Application.Idle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs args)
        {
            BtAddFixpack.Enabled = LBoxReleases.SelectedIndex != -1;
        }

        SortedList<string, Release> releases;
        SortedList<string, Fixpack> fixpacks;

        private void AddRelease(string name)
        {
            if (Directory.Exists(home))
            {
                Release release = new Release(name);

                DirectoryInfo releaseDir = Directory.CreateDirectory(string.Join("\\", home, release.name));

                release.SetLocalDir(releaseDir);

                releases.Add(release.name, release);

                LBoxReleases.Items.Clear();
                foreach (string releaseName in releases.Keys)
                {
                    LBoxReleases.Items.Add(releaseName);
                }
            }

            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
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

        private void ReleaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            excelApp.Quit();
            Marshal.FinalReleaseComObject(excelApp);
            cvs.Close();
        }

        private void LBoxReleases_SelectedIndexChanged(object sender, EventArgs e)
        {
            LBoxFixpacks.Items.Clear();
            if (LBoxReleases.SelectedIndex != -1)
            {
                currRelease = releases[(string)LBoxReleases.SelectedItem];                
                foreach (string fpName in currRelease.fixpacks.Keys)
                {
                    LBoxFixpacks.Items.Add(fpName);
                }
            }
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
                currRelease.AddFixpack(addFixpackForm.Value);
            }
        }
    }
}
