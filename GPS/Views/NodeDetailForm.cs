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
        public GPSNode node { get; set; }
        public NodeDetailForm()
        {
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
            this.label1.Text = node.Name;
            this.label2.Text = "" + node.Location.X;
            this.label3.Text = "" + node.Location.Y;
            // adding characteristics - requires changes in model
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
