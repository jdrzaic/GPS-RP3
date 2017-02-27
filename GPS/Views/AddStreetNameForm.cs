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
    public partial class AddStreetNameForm : Form
    {
        public GPSStreet street { get; set; }
        public AddStreetNameForm(GPSStreet street)
        {
            this.street = street;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Name field must not be empty");
                return;
            }
            street.Name = this.textBox1.Text;
            Close();
        }
    }
}
