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
    public partial class ZPatchOrderForm : Form
    {
        CPatch cpatch;
        public ZPatchOrderForm(CPatch cpatch)
        {
            this.cpatch = cpatch;
            InitializeComponent();
            Application.Idle += OnIdle;

            foreach(var zpatch in cpatch.ZPatchOrder.Values)
            {
                LboxZPatchOrder.Items.Add(zpatch);
            }
        }

        private void OnIdle(object sender, EventArgs e)
        {
            BtUp.Enabled   = LboxZPatchOrder.SelectedItem != null && LboxZPatchOrder.SelectedIndex != 0;
            BtDown.Enabled = LboxZPatchOrder.SelectedItem != null && LboxZPatchOrder.SelectedIndex != LboxZPatchOrder.Items.Count - 1;
        }
        

        private void BtUp_Click(object sender, EventArgs e)
        {
            ZPatch currPatch = (ZPatch)LboxZPatchOrder.SelectedItem;

            int currPatchIndex = LboxZPatchOrder.SelectedIndex;
            ZPatch prevPatch = (ZPatch)LboxZPatchOrder.Items[currPatchIndex - 1];

            if(prevPatch.dependenciesTo.Contains(currPatch) || currPatch.dependenciesFrom.Contains(prevPatch))
            {
                MessageBox.Show($"Патч {prevPatch} влияет на {currPatch}, удалите зависимость и попробуйте снова", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LboxZPatchOrder.Items[currPatchIndex] = prevPatch;
            LboxZPatchOrder.Items[currPatchIndex - 1] = currPatch;

            LboxZPatchOrder.SelectedIndex--;
        }

        private void BtDown_Click(object sender, EventArgs e)
        {
            ZPatch currPatch = (ZPatch)LboxZPatchOrder.SelectedItem;

            int currPatchIndex = LboxZPatchOrder.SelectedIndex;
            ZPatch nextPatch = (ZPatch)LboxZPatchOrder.Items[currPatchIndex + 1];

            if (nextPatch.dependenciesFrom.Contains(currPatch) || currPatch.dependenciesTo.Contains(nextPatch))
            {
                MessageBox.Show($"Патч {nextPatch} зависит от {currPatch}, удалите зависимость и попробуйте снова", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LboxZPatchOrder.Items[currPatchIndex] = nextPatch;
            LboxZPatchOrder.Items[currPatchIndex + 1] = currPatch;

            LboxZPatchOrder.SelectedIndex++;
        }

        private void BtConfirm_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < LboxZPatchOrder.Items.Count; ++i)
            {
                cpatch.UpdateZPatchOrder((ZPatch)LboxZPatchOrder.Items[i], i);
            }
        }
    }
}
