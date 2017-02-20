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

        public GraphMainForm()
        {
            this.graph = new GPSGraph();
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
        }

        public void CreateOneWayConnection(GPSGraph.Node node)
        {
            GPSGraph.Node originNode = this.lastSelectedNode;
            if(originNode.IsConnectedTo(node))
            {
                MessageBox.Show("Nodes are already connected");
                return;
            } 
            originNode.ConnectTo(new GPSStreet(), node);
            this.area.GraphChanged();
            this.nodeSelected = false;
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
            if (street == null) street = new GPSStreet();
            originNode.ConnectBothWays(street, node);
            this.area.GraphChanged();
            this.nodeSelected = false;
        }

        public void SelectNode(GPSGraph.Node node)
        {
            this.nodeSelected = true;
            this.lastSelectedNode = node;
        }
    }
}
