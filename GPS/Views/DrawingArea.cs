using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GPS.Views
{
    public partial class DrawingArea : Panel
    {
        private GPSGraph graph;
        public IEnumerable<Tuple<GPSStreet, GPSGraph.Node>> highlighted { get; set; }
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
            var newLocationButton = new LocationNodeButton(Color.Blue, Color.DarkBlue);
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
            Pen pen = new Pen(Color.DarkBlue, 3);
            var nodes = this.graph.Nodes;
            foreach (var node in nodes)
            {
                DrawConnectionsForNode(node, pen, graphics);
            }
            HandleHighlighted();
        }

        private void HandleHighlighted()
        {
            var current = this.creator.lastSelectedNode;
            foreach (var connection in this.highlighted)
            {

            }
        }

        private void DrawConnectionsForNode(GPSGraph.Node node, Pen p, Graphics g)
        {
            var connections = node.Connections;
            var nodeSizeOffset = node.Data.AssociatedControl.Height / 2;
            var originX = node.Data.Location.X + nodeSizeOffset;
            var originY = node.Data.Location.Y + nodeSizeOffset;
            foreach (var connection in connections)
            {
                var otherNode = connection.Item1;
                var destX = otherNode.Data.Location.X + nodeSizeOffset;
                var destY = otherNode.Data.Location.Y + nodeSizeOffset;
                g.DrawLine(p, originX, originY, destX, destY);
                var middlePoint = new Point((int)((destX + originX) / 2),
                    (int)((destY + originY) / 2));
                var twoThirdsPoint = new Point((int)((3 * destX + originX) / 4),
                    (int)((3 * destY + originY) / 4));
                p.CustomEndCap = new AdjustableArrowCap(4, 4);
                g.DrawLine(p, middlePoint, twoThirdsPoint);
                p.EndCap = LineCap.Flat;
            }
        }
    }
}
