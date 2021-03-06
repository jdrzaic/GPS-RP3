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
    public partial class StreetBasicForm : Form
    {
        public GPSStreet street { get; set; }
        public StreetBasicForm(GPSStreet street)
        {
            this.street = street;
            InitializeComponent();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            if (street == null) return;
            this.textBox1.Text = street.Name;
            this.textBox1.Enabled = false;
        }
    }
}
