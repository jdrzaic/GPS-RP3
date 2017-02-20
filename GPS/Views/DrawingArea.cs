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
    public partial class DrawingArea : Panel
    {
        private GPSGraph graph;
        public GraphMainForm creator { get; set; }
        public DrawingArea(GPSGraph graph, Form creator) : base()
        {
            this.creator = (GraphMainForm)creator;
            this.graph = graph;
            InitializeComponent();
            CustomizeComponent();
        }

        public void GraphChanged() {
            this.Refresh();
        }

        public void GraphCleared()
        {
            this.Refresh();
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

        public LocationNodeButton addButtonForNode(float x, float y, GPSGraph.Node node)
        {
            //change size if more than current
            var newLocationButton = new LocationNodeButton();
            newLocationButton.Location = new Point((int)x,
                (int)y);
            newLocationButton.node = node;
            newLocationButton.creator = this.creator;
            this.Controls.Add(newLocationButton);
            return newLocationButton;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            // graphics.DrawLine(new Pen(Brushes.Aquamarine), new Point(10, 20), new Point(140, 150));
        }

        private void drawLineConnection(PointF point1, PointF point2)
        {

        }
    }
}
