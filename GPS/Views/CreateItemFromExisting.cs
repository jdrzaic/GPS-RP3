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
    public partial class CreateItemFromExisting : Form
    {
        private LocationNodeButton creator;
        public CreateItemFromExisting(LocationNodeButton creator)
        {
            this.creator = creator;
            InitializeComponent();
            CustomizeComponent();
        }

        private void CustomizeComponent()
        {
            this.numericUpDown1.Minimum = 0;
            this.numericUpDown2.Minimum = 0;
            this.numericUpDown1.Maximum = decimal.MaxValue;
            this.numericUpDown2.Maximum = decimal.MaxValue;
            this.numericUpDown1.Increment = 1;
            this.numericUpDown2.Increment = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fromFirst = (float)this.numericUpDown1.Value;
            var fromSecond = (float)this.numericUpDown2.Value;
            this.creator.ItemFromExistingCallback(fromFirst, fromSecond);
            Close();
        }
    }
}
