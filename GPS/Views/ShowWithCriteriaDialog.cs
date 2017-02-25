﻿using System;
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
    public partial class ShowWithCriteriaDialog : Form
    {
        private List<string> namesCriteria;
        private List<string> typesCriteria;
        GraphMainForm creator;

        public ShowWithCriteriaDialog(GraphMainForm creator)
        {
            this.creator = creator;
            namesCriteria = new List<string>();
            typesCriteria = new List<string>();
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
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {}

        private void AddCriteriasDialog_Load(object sender, EventArgs e) {}

        private void button2_Click(object sender, EventArgs e)
        {
            var type = this.textBox1.Text;
            var name = this.textBox2.Text;
            if (type != "") this.typesCriteria.Add(type);
            if (name != "") this.namesCriteria.Add(name);
            string[] row = new string[] { type, name};
            this.listView1.Items.Add(new ListViewItem(row));
            this.textBox1.Text = "";
            this.textBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.creator.ShowItemsWithCriteriaCallback(
                this.typesCriteria, this.namesCriteria);
            Close();
        }
    }
}
