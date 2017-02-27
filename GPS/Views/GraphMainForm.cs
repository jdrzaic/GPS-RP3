using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPS.Views
{
    public partial class GraphMainForm: Form
    {
        public int graphId;
        public bool nodeSelected = false;
        public GPSGraph.Node lastSelectedNode;
        private GPSGraph graph;
        public DrawingArea area;
        public bool calculatingPath = false;

        public GraphMainForm(int graphId)
        {
            this.graph = Program.DbContext.Graphs
                .Where(g => g.GPSGraphId == graphId)
                .FirstOrDefault();
            this.graph = new GPSGraph();  //comment this once database works
            InitializeComponent();
            CustomizeComponent();
        }

        public void CustomizeComponent()
        {
            this.area = new DrawingArea(this.graph, this);
            this.area.Location = new Point(0, this.menuStrip2.Height);
            this.Controls.Add(this.area);
        }

        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addNodeForm = new AddNodeForm();
            addNodeForm.creator = this;
;            addNodeForm.ShowDialog();
        }

        private void cleanGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.graph = new GPSGraph();
            this.area.GraphCleared();
        }

        public void NodeCreated(string name, float xCoordinate, float yCoodrinate) {
            if (graph == null) return;
            var newNode = new GPSNode();
            newNode.Location = new PointF(xCoordinate, yCoodrinate);
            newNode.Name = name;
            var node = graph.NewNode(newNode);
            var locationButton = this.area.addButtonForNode(xCoordinate, yCoodrinate, node);
            newNode.AssociatedControl = locationButton;
            this.area.SetSize(xCoordinate, yCoodrinate);
        }

        public void CreateOneWayConnection(GPSGraph.Node node)
        {
            GPSGraph.Node originNode = this.lastSelectedNode;
            if(originNode.IsConnectedTo(node))
            {
                MessageBox.Show("Nodes are already connected");
                return;
            }
            var street = new GPSStreet();
            originNode.ConnectTo(street, node);
            this.nodeSelected = false;
            this.area.GraphChanged();
            createStreetButton(originNode, node, street);
            System.Threading.Thread.Sleep(500);
            var addStreetNameForm = new AddStreetNameForm(street);
            addStreetNameForm.ShowDialog();
        }

        public void createStreetButton(GPSGraph.Node node1, GPSGraph.Node node2, GPSStreet street) 
        {
            var button = new LocationStreetButton(Color.Red, Color.OrangeRed, 16, 16);
            var middleX = (int)(node1.Data.Location.X + node2.Data.Location.X) / 2 + node1.Data.AssociatedControl.Height / 2 - 8;
            var middleY = (int)(node1.Data.Location.Y + node2.Data.Location.Y) / 2 + node1.Data.AssociatedControl.Height / 2 - 8; 
            button.Location = new Point(middleX, middleY);
            button.street = street;
            button.creator = this;
            this.area.Controls.Add(button);
        }

        public void CreateBothWayConnection(GPSGraph.Node node)
        {
            GPSGraph.Node originNode = this.lastSelectedNode;
            GPSStreet street = null;
            if (originNode.IsConnectedTo(node)) street = originNode.GetConnectionToNode(node);
            if (street != null && node.IsConnectedTo(originNode))
            {
                MessageBox.Show("Nodes are already connected both ways");
                return;
            }
            if (node.IsConnectedTo(originNode)) street = node.GetConnectionToNode(originNode);
            if (street == null)
            {
                street = new GPSStreet();
                createStreetButton(originNode, node, street);
            }
            originNode.ConnectBothWays(street, node);
            this.area.GraphChanged();
            System.Threading.Thread.Sleep(500);
            var addStreetNameForm = new AddStreetNameForm(street);
            addStreetNameForm.ShowDialog();
            this.nodeSelected = false;
        }

        public void SelectNode(GPSGraph.Node node)
        {
            this.nodeSelected = true;
            this.lastSelectedNode = node;
        }

        public void CalculateShortestPath(GPSGraph.Node node)
        {
            try
            { 
                this.area.highlighted = this.lastSelectedNode.GetBestPath(node,
                    new List<Predicate<GPSNode>>());
                if (this.area.highlighted.Count() == 0)
                {
                    MessageBox.Show("No available routes");
                }
                this.calculatingPath = true;
                this.area.GraphChanged();
                this.calculatingPath = false;
            } catch(Exception ex)
            {
                MessageBox.Show("No available routes");
            }
        }

        public void AddItemFromExisting(GPSGraph.Node n2, float d13, float d23)
        {
            var n1 = this.lastSelectedNode;
            var d12 = Math.Sqrt(Math.Pow(n2.Data.Location.X - n1.Data.Location.X, 2) + Math.Pow(
                n2.Data.Location.Y - n1.Data.Location.Y, 2));
            var cosn1 = (d12 * d12 + d13 * d13 - d23 * d23) / (2 * d12 * d13);
            var sinn1 = Math.Sqrt(1 - cosn1 * cosn1);
            var h = sinn1 * d13; // length of h
            var x12 = cosn1 * d13; // h part
            var x12ratio = x12 / d12;
            var hx = n1.Data.Location.X + x12ratio * (n2.Data.Location.X - n1.Data.Location.X);
            var hy = n1.Data.Location.Y + x12ratio * (n2.Data.Location.Y - n1.Data.Location.Y);
            var coefh = -1.0;
            if (n1.Data.Location.X == n2.Data.Location.X) coefh = 0.0;
            else
            {
                var tanbase = (n2.Data.Location.Y - this.lastSelectedNode.Data.Location.Y) /
                    (n2.Data.Location.X - this.lastSelectedNode.Data.Location.X);
                coefh = -1.0 / tanbase;
            }
            var all = 1 + coefh * coefh;
            var unit = 1 / all;
            var unitSqrt = Math.Sqrt(unit);
            var sinh = coefh * unitSqrt;
            var cosh = unitSqrt;
            var x = hx + h * sinh;
            var y = hy + h * cosh;
            if (x < 0 || y < 0)
            {
                x = hx - h * sinh;
                y = hy - h * cosh;

            }
            if (x < 0 || y < 0)
            {
                x = hx - h * sinh;
                y = hy + h * cosh;
            }
            if (x < 0 || y < 0)
            {
                x = hx + h * sinh;
                y = hy - h * cosh;
            }
            if (x < 0 || y < 0) return;
            NodeCreated(n1.Data.Name + " + " + n2.Data.Name, (float)x, (float)y);
        }

        public void CalculateShortestPathWithCriteria(GPSGraph.Node node, List<Tuple<string, string>> criteria)
        {
            var criteriaPredicates = new List<Predicate<GPSNode>>();
            foreach (var cr in criteria) {
                Predicate<GPSNode> namePredicate = delegate (GPSNode g)
                {
                    foreach (var c in g.Characteristics)
                    {
                        if (c.Name == cr.Item2 && c.NodeType.ToString() == cr.Item1) return true;
                    }
                    return false;
                };
                criteriaPredicates.Add(namePredicate);
            }
            try
            {
                this.area.highlighted = this.lastSelectedNode.GetBestPath(node, criteriaPredicates);
                this.calculatingPath = true;
                this.area.GraphChanged();
                this.calculatingPath = false;
            } catch (Exception ex)
            {
                MessageBox.Show("No available route");
            }
        }

        private void showItemsWithCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var showWithCriteriaDialog = new ShowWithCriteriaDialog(this);
            showWithCriteriaDialog.ShowDialog();
        }

        public void ShowItemsWithCriteriaCallback(List<string> types, List<string> names)
        {
            this.area.itemsToShow = new Tuple<List<string>, List<string>>(types, names);
            this.area.GraphChanged();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
