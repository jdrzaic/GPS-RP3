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
            //this.graph = Program.DbContext.Graphs   uncommment when database fixed
            //    .Where(g => g.GraphId == GPSGraphId)
            //    .FirstOrDefault();
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
            var middleX = (int)(node1.Data.Location.X + node2.Data.Location.X) / 2 + 8;
            var middleY = (int)(node1.Data.Location.Y + node2.Data.Location.Y) / 2 + 8; 
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
            var cosn1 = d12 * d12 + d13 * d13 - d23 * d23;
            cosn1 /= 2 * d12 * d13;
            var sinn1 = Math.Sqrt(1 - cosn1 * cosn1);
            var tann1 = sinn1 / cosn1;
            var cosn2 = d12 * d12 + d23 * d23 - d13 * d13;
            cosn2 /= 2 * d12 * d23;
            var sinn2 = Math.Sqrt(1 - cosn2 * cosn2);
            if (Math.Abs(sinn2 - sinn1) < 0.0000000001) sinn2 = -sinn2;
            var tann2 = sinn2 / cosn2;
            var C1 = n1.Data.Location.Y - tann1 * n1.Data.Location.X;
            var C2 = n2.Data.Location.Y - tann2 * n2.Data.Location.X;
            var A1 = tann1;
            var A2 = tann2;
            var B1 = -1.0;
            var B2 = -1.0;
            double delta = A1 * B2 - A2 * B1;
            double x = (B2 * C1 - B1 * C2) / delta;
            double y = (A1 * C2 - A2 * C1) / delta;
            Debug.WriteLine("" + x + " " + y);
            NodeCreated(n1.Data.Name + " + " + n2.Data.Name, (float)x, (float)y);
        }

        public void CalculateShortestPathWithCriteria(GPSGraph.Node node, Tuple<List<String>, List<String>> criteria)
        {
            var criteriaPredicates = new List<Predicate<GPSNode>>();
            var types = criteria.Item1;
            var names = criteria.Item2;
            foreach (var name in names) {
                Predicate<GPSNode> namePredicate = delegate (GPSNode g)
                {
                    foreach (var c in g.Characteristics)
                    {
                        if (c.Name == name) return true;
                    }
                    return false;
                };
                criteriaPredicates.Add(namePredicate);
            }
            foreach (var type in types)
            {
                Predicate<GPSNode> typePredicate = delegate (GPSNode g)
                {
                    foreach (var c in g.Characteristics)
                    {
                        if (c.NodeType.ToString() == type) return true;
                    }
                    return false;
                };
                criteriaPredicates.Add(typePredicate);
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
    }
}
