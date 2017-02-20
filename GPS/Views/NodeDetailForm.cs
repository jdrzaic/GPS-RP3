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
    public partial class NodeDetailForm : Form
    {
        public GPSGraph.Node node { get; set; }
        public NodeDetailForm(GPSGraph.Node node) : base()
        {
            this.node = node;
            InitializeComponent();
            InitializeStaticProperties();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            if (node == null) return;
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Name         ");
            this.listView1.Columns.Add("Type         ");
            this.listView1.Columns.Add("Description                                                                             ");
            this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.textBox1.Text = node.Data.Name;
            this.textBox1.Enabled = false;
            this.textBox2.Text = "" + node.Data.Location.X;
            this.textBox2.Enabled = false;
            this.textBox3.Text = "" + node.Data.Location.Y;
            this.textBox3.Enabled = false;
            foreach (var characteristic in node.Data.Characteristics)
            {
                // change to get type 
                var rowToAdd = new string[] { characteristic.Name, "", characteristic.Description };
                this.listView1.Items.Add(new ListViewItem(rowToAdd));
            }
        }

        public void InitializeStaticProperties()
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
