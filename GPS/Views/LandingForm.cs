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
    public partial class LandingForm : Form
    {
        private static int OFFSET_LIST = 10;

        public LandingForm()
        {
            InitializeComponent();
            CustomizeComponent();
            InitializeStaticProperties();
            addGraphsToListView();
        }

        public void addGraphsToListView()
        {
            var entities = Program.DbContext.Graphs;
            foreach (var entity in entities)
            {
                var name = entity.graphName;
                if (name == null || name.Trim() == "") continue;
                this.listBox1.Items.Add(name);
            }   
        }

        public void InitializeStaticProperties()
        {
            this.splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            this.listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            this.listBox1.MeasureItem += ListBox1_MeasureItem;
        }

        private void ListBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {

        }

        public void CustomizeComponent()
        {
            // this.listBox1.View = View.Details;
            this.listBox1.Size = new Size(this.splitContainer1.Panel1.ClientRectangle.Width - 2 * OFFSET_LIST,
                this.splitContainer1.Panel1.ClientRectangle.Height - 2 * OFFSET_LIST);
            this.label2.Location = new Point(this.splitContainer1.Panel2.Width / 2 - this.label2.Width / 2, this.splitContainer1.ClientRectangle.Height / 2 - this.label2.Height);
            this.richTextBox1.Location = new Point(this.splitContainer1.Panel2.Width / 2 - richTextBox1.ClientRectangle.Width / 2, this.splitContainer1.Height / 2);
            this.button1.Location = new Point(this.splitContainer1.Panel2.Width / 2 - this.button1.Width / 2, this.splitContainer1.ClientRectangle.Height / 2 + this.richTextBox1.ClientRectangle.Height + this.button1.Height / 2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            CustomizeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CustomizeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
