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
    public partial class AddForm : Form
    {
        public string Value { get; private set; }

        public AddForm()
        {
            InitializeComponent();
            Application.Idle += OnIdle;
        }

        private void OnIdle(object sender, EventArgs args)
        {
            BtSubmit.Enabled = !string.IsNullOrWhiteSpace(TbReleaseName.Text);
        }

        private void BtSubmit_Click(object sender, EventArgs e)
        {
            Value = TbReleaseName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void TbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                BtSubmit_Click(sender, e);
            }
        }
    }
}
