using CoreApp.ReleaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreApp
{
    public partial class ReleasesListForm : Form
    {
        IEnumerable<Release> releases;
        public Release release { get; private set; }

        public ReleasesListForm(IEnumerable<Release> releases)
        {
            this.releases = releases;
            InitializeComponent();
            Application.Idle += OnIdle;

            foreach (Release release in releases)
            {
                LboxReleases.Items.Add(release);
            }
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtConfirm.Enabled = LboxReleases.SelectedItem != null;
        }

        private void BtConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            release = (Release)LboxReleases.SelectedItem;
        }


    }
}
