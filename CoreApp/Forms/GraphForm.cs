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
    public partial class GraphForm : Form
    {
        public GraphForm(Microsoft.Msagl.Drawing.Graph graph)
        {
            InitializeComponent();
            gViewer.Graph = graph;
        }

        private void GraphForm_Resize(object sender, EventArgs e)
        {
            gViewer.Top = 0;
            gViewer.Left = 0;
            gViewer.Width = ClientRectangle.Width;
            gViewer.Height = ClientRectangle.Height;
        }
    }
}
