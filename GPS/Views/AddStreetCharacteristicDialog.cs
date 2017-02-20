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
    public partial class AddStreetCharacteristicDialog : Form
    {
        public GPSStreet street { get; set; }
        public AddStreetCharacteristicDialog(GPSStreet street)
        {
            this.street = street;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) {}

        private void button1_Click(object sender, EventArgs e)
        {
            var characteristic = new GPSCharacteristic();
            characteristic.Name = this.textBox2.Text;
            characteristic.Description = this.richTextBox1.Text;
            street.Characteristics.Add(characteristic);
            Close();
        }
    }
}
