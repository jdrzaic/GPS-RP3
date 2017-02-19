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
    public partial class NodeBasicForm : Form
    {
        public GPSNode node { get; set; }
        public NodeBasicForm()
        {
            InitializeComponent();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            if (node == null) return;
            this.label1.Text = node.Name;
            this.label2.Text = "" + node.Location.X;
            this.label3.Text = "" + node.Location.Y;
        }
    }
}
