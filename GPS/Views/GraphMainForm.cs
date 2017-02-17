using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPS.Views
{
    public partial class GraphMainForm: Form
    {
        public int graphId;

        public GraphMainForm()
        {
            InitializeComponent();
        }

        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addNodeForm = new AddNodeDialog();
            addNodeForm.ShowDialog();
        }
    }
}
