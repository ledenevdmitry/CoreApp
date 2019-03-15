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
    public partial class CPatchesListForm : Form
    {
        IEnumerable<CPatch> cpatches;
        public CPatch cpatch { get; private set; }

        public CPatchesListForm(IEnumerable<CPatch> cpatches)
        {
            this.cpatches = cpatches;
            InitializeComponent();
            Application.Idle += OnIdle;

            foreach (CPatch cpatch in cpatches)
            {
                LboxCPatches.Items.Add(cpatch);
            }
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtConfirm.Enabled = LboxCPatches.SelectedItem != null;
        }

        private void BtConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            cpatch = (CPatch)LboxCPatches.SelectedItem;
        }

    }
}
