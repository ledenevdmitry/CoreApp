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
    public partial class ZPatchesListForm : Form
    {
        IEnumerable<ZPatch> zpatches;
        public ZPatch zpatch { get; private set; }

        public ZPatchesListForm(IEnumerable<ZPatch> zpatches)
        {
            this.zpatches = zpatches;
            InitializeComponent();
            Application.Idle += OnIdle;

            foreach (ZPatch zpatch in zpatches)
            {
                LboxZPatches.Items.Add(zpatch);
            }
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtConfirm.Enabled = LboxZPatches.SelectedItem != null;
        }

        private void BtConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            zpatch = (ZPatch)LboxZPatches.SelectedItem;
        }

    }
}
