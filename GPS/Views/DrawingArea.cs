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
        public Tuple<List<string>, List<string>> itemsToShow = null;
        public IEnumerable<Tuple<GPSStreet, GPSGraph.Node>> highlighted { get; set; }
        public GraphMainForm creator { get; set; }
        public DrawingArea(GPSGraph graph, Form creator) : base()
        {
            this.creator = (GraphMainForm)creator;
            this.graph = graph;
            this.highlighted = new List<Tuple<GPSStreet, GPSGraph.Node>>();
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
            pen.Color = Color.Red;
            if (this.creator.calculatingPath) HandleHighlighted(pen, graphics);
            if (this.itemsToShow != null) HandleItemsToShow(pen, graphics);
        }

        private void HandleItemsToShow(Pen p, Graphics g)
        {
            HandleNodesToShow();
            HandleEdgesToShow(p, g);
        }

        private void HandleNodesToShow()
        {
            foreach (var node in graph.Nodes)
            {
                if (CheckSatisfiesCriteria(node.Data.Characteristics))
                {
                    var button = (LocationNodeButton)node.Data.AssociatedControl;
                    button.InnerColor = Color.Green;
                    button.OuterColor = Color.ForestGreen;
                    button.Refresh();
                }
            }
        }

        private void HandleEdgesToShow(Pen p, Graphics g)
        {
            p.Color = Color.Green;
            foreach (var node in graph.Nodes)
            {
                foreach (var connection in node.Connections)
                {
                    var street = connection.Item2;
                    if (CheckSatisfiesCriteria(street.Characteristics))
                    {
                        var origin = node;
                        var dest = connection.Item1;
                        var origX = origin.Data.Location.X + origin.Data.AssociatedControl.Height / 2;
                        var origY = origin.Data.Location.Y + origin.Data.AssociatedControl.Height / 2;
                        var destX = dest.Data.Location.X + dest.Data.AssociatedControl.Height / 2;
                        var destY = dest.Data.Location.Y + dest.Data.AssociatedControl.Height / 2;
                        DrawArrowLine(origX, origY, destX, destY, g, p);
                    }
                }
            }
            p.Color = Color.Red;
        }

        private bool CheckSatisfiesCriteria(List<GPSCharacteristic> characteristics)
        {;
            var types = itemsToShow.Item1;
            var names = itemsToShow.Item2;
            var satisfied = new List<GPSGraph.Node>();
            foreach (var type in types)
            {
                var foundType = false;
                foreach (var characteristic in characteristics)
                {
                    if (characteristic.NodeType == (NodeType)Enum.Parse(
                        typeof(NodeType), type))
                    {
                        foundType = true;
                        break;
                    }
                }
                if (!foundType) return false;
            }
            foreach (var name in names)
            {
                var foundName = false;
                foreach (var characteristic in characteristics)
                {
                    if (characteristic.Name == name)
                    {
                        foundName = true;
                        break;
                    }
                }
                if (!foundName) return false;
            }
            return true;
        }

        private void HandleHighlighted(Pen p, Graphics g)
        {
            var current = this.creator.lastSelectedNode;
            var nodeSizeOffset = current.Data.AssociatedControl.Height / 2;
            foreach (var connection in this.highlighted)
            {
                var originX = current.Data.Location.X + nodeSizeOffset;
                var originY = current.Data.Location.Y + nodeSizeOffset;
                var otherNode = connection.Item2;
                var destX = otherNode.Data.Location.X + nodeSizeOffset;
                var destY = otherNode.Data.Location.Y + nodeSizeOffset;
                DrawArrowLine(originX, originY, destX, destY, g, p);
                current = otherNode;
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
                DrawArrowLine(originX, originY, destX, destY, g, p);
            }
        }

        private void DrawArrowLine(float originX, float originY, float destX, float destY,
            Graphics g, Pen p) {
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

