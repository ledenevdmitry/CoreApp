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
            BtUp.Enabled   = LboxZPatchOrder.SelectedItem != null && LboxZPatchOrder.SelectedIndices.IndexOf(0) == -1;
            BtDown.Enabled = LboxZPatchOrder.SelectedItem != null && LboxZPatchOrder.SelectedIndices.IndexOf(LboxZPatchOrder.Items.Count - 1) == -1;
        }
        

        private void BtUp_Click(object sender, EventArgs e)
        {
            var itemsToMove = LboxZPatchOrder.SelectedItems;
            for(int i = 0; i < itemsToMove.Count; ++i)
            {
                var selectedItem = itemsToMove[i];
                int oldIndex = LboxZPatchOrder.Items.IndexOf(selectedItem);
                int newIndex = oldIndex - 1;

                ZPatch curr = (ZPatch)LboxZPatchOrder.Items[oldIndex];
                ZPatch prev = (ZPatch)LboxZPatchOrder.Items[newIndex];

                if (prev.DependenciesTo.Contains(curr) || curr.DependenciesFrom.Contains(prev))
                {
                    MessageBox.Show($"Патч {prev} влияет на {curr}, удалите зависимость и попробуйте снова", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LboxZPatchOrder.Items.Remove(selectedItem);
                LboxZPatchOrder.Items.Insert(newIndex, selectedItem);
                LboxZPatchOrder.SetSelected(newIndex, true);
            }
        }

        private void BtDown_Click(object sender, EventArgs e)
        {
            var itemsToMove = LboxZPatchOrder.SelectedItems;
            for (int i = itemsToMove.Count - 1; i >= 0; --i)
            {
                var selectedItem = itemsToMove[i];
                int oldIndex = LboxZPatchOrder.Items.IndexOf(selectedItem);
                int newIndex = oldIndex + 1;

                ZPatch curr = (ZPatch)LboxZPatchOrder.Items[oldIndex];
                ZPatch next = (ZPatch)LboxZPatchOrder.Items[newIndex];

                if (curr.DependenciesTo.Contains(next) || next.DependenciesFrom.Contains(curr))
                {
                    MessageBox.Show($"Патч {curr} влияет на {next}, удалите зависимость и попробуйте снова", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LboxZPatchOrder.Items.Remove(selectedItem);
                LboxZPatchOrder.Items.Insert(newIndex, selectedItem);
                LboxZPatchOrder.SetSelected(newIndex, true);
            }
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
