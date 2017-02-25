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
        private Dictionary<string, int> GraphsInfo = new Dictionary<string, int>();

        public LandingForm()
        {
            InitializeComponent();
            CustomizeComponent();
            addGraphsToListView();
            InitializeStaticProperties();
        }

        public void addGraphsToListView()
        {
            this.GraphsInfo = new Dictionary<string, int>();
            var entities = Program.DbContext.Graphs;
            foreach (var entity in entities)
            {
                var name = entity.graphName;
                if (name == null || name.Trim() == "") continue;
                if (this.GraphsInfo.ContainsKey(name)) continue;
                this.listBox1.Items.Add(name);
                this.GraphsInfo.Add(name, entity.GPSGraphId);
            }   
        }

        public void InitializeStaticProperties()
        {
            this.splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            this.listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            this.listBox1.DrawItem += new DrawItemEventHandler(ListBox1_DrawItem);
            this.listBox1.MeasureItem += new MeasureItemEventHandler(ListBox1_MeasureItem);
            this.listBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index == ListBox.NoMatches) return;
            var graphName = this.listBox1.Items[index].ToString();
            var graphForm = new GraphMainForm(this.GraphsInfo[graphName]);
            graphForm.ShowDialog();
        }

        private void ListBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Cast the sender object back to ListBox type.
            e.ItemHeight = 22;
        }

        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
            }
            else
            {
                // Otherwise, draw the rectangle filled in beige.
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }

            // Draw the text in the item.
            var font = new Font("Cambria", 11);
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(),
                font, Brushes.DarkGray, e.Bounds.X, e.Bounds.Y);

            // Draw the focus rectangle around the selected item.
            e.DrawFocusRectangle();

        }

        public void CustomizeComponent()
        {
            // this.listBox1.View = View.Details;
            this.listBox1.Size = new Size(this.splitContainer1.Panel1.ClientRectangle.Width - 2 * OFFSET_LIST,
                this.splitContainer1.Panel1.ClientRectangle.Height - 2 * OFFSET_LIST);
            this.label2.Location = new Point(this.splitContainer1.Panel2.Width / 2 - this.label2.Width / 2, this.splitContainer1.ClientRectangle.Height / 2 - 50 - this.label2.Height - this.label2.Height / 2);
            this.richTextBox1.Location = new Point(this.splitContainer1.Panel2.Width / 2 - richTextBox1.ClientRectangle.Width / 2, this.splitContainer1.Height / 2 - 50);
            this.button1.Location = new Point(this.splitContainer1.Panel2.Width / 2 - this.button1.Width / 2, this.splitContainer1.ClientRectangle.Height / 2 + this.richTextBox1.ClientRectangle.Height + this.button1.Height / 2 - 50);
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
            var graph = new GPSGraph();
            var name = this.richTextBox1.Text;
            if (name.Trim() != "")
            {
                graph.graphName = name.Trim();
                Program.DbContext.Graphs.Add(graph);
                Program.DbContext.SaveChanges();
                this.listBox1.Hide();
                listBox1.Items.Clear();
                addGraphsToListView();
                this.listBox1.Show();
            }
            else
            {
                var caption = "Error in input";
                var message = "Graph name is empty, please enter graph name and try again.";
                MessageBox.Show(message, caption);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
