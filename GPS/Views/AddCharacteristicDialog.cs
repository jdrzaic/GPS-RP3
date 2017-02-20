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
    public partial class AddCharacteristicDialog : Form
    {
        public GPSGraph.Node node { get; set; }
        public AddCharacteristicDialog(GPSGraph.Node node)
        {
            this.node = node;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) {}

        private void button1_Click(object sender, EventArgs e)
        {
            //store node
        }
    }
}
