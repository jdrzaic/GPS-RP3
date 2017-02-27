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
    public partial class AddNodeCharacteristicDialog : Form
    {
        public GPSGraph.Node node { get; set; }
        public AddNodeCharacteristicDialog(GPSGraph.Node node)
        {
            this.node = node;
            InitializeComponent();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            var typeValues = Enum.GetValues(typeof(NodeType));
            foreach (var value in typeValues)
            {
                this.comboBox1.Items.Add(value.ToString());
            }
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void label1_Click(object sender, EventArgs e) {}

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Name field must not be empty");
                return;
            }
            var characteristic = new GPSCharacteristic();
            characteristic.NodeType = (NodeType)Enum.Parse(typeof(NodeType), this.comboBox1.Text);
            characteristic.Name = this.textBox1.Text;
            characteristic.Description = this.richTextBox1.Text;
            node.Data.Characteristics.Add(characteristic);
            Close();
        }
    }
}
