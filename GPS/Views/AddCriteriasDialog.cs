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
    public partial class AddCriteriasDialog : Form
    {
        private List<Tuple<string, string>> criteria;
        LocationNodeButton creator;

        public AddCriteriasDialog(LocationNodeButton creator)
        {
            this.creator = creator;
            criteria = new List<Tuple<string, string>>();
            InitializeComponent();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("Type         ");
            this.listView1.Columns.Add("Name         ");
            this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            var typeValues = Enum.GetValues(typeof(NodeType));
            foreach (var value in typeValues)
            {
                this.comboBox1.Items.Add(value.ToString());
            }
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {}

        private void AddCriteriasDialog_Load(object sender, EventArgs e) {}

        private void button2_Click(object sender, EventArgs e)
        {
            var type = this.comboBox1.Text;
            var name = this.textBox2.Text;
            if (type != "" && name != "") this.criteria.Add(new Tuple<string, string>(type, name));
            string[] row = new string[] { type, name};
            this.listView1.Items.Add(new ListViewItem(row));
            this.comboBox1.Text = "";
            this.textBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.creator.ItemFindShortestWithCriteriaCallback(this.criteria);
            Close();
        }
    }
}

