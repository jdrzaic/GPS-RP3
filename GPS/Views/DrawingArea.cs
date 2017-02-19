using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPS.Views
{
    public partial class DrawingArea : UserControl
    {
        public DrawingArea()
        {
            InitializeComponent();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            this.Size = new Size(1000, 1000);
        }

        public void SetSize(int width, int height)
        {
            this.Size = new Size(width, height);
            this.Refresh();
        }
    }
}
