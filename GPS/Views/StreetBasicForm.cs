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
    public partial class StreetBasicForm : Form
    {
        public GPSGraph.Node node { get; set; }
        public StreetBasicForm(GPSGraph.Node node)
        {
            this.node = node;
            InitializeComponent();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            if (node == null) return;
            this.textBox1.Text = node.Data.Name;
            this.textBox1.Enabled = false;
            this.textBox2.Text = "" + node.Data.Location.X;
            this.textBox2.Enabled = false;
            this.textBox3.Text = "" + node.Data.Location.Y;
            this.textBox3.Enabled = false;
        }
    }
}
