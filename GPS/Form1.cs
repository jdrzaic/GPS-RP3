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

namespace GPS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var graph = Program.DbContext.Graphs.First();
            var tbjNode = graph.Nodes.First();
            var kvatricNode = graph.Nodes.Skip(1).First();
            //var graph = new GPSGraph();

            //var tbj = new GPSNode { Name = "TBJ" };
            //var kvatric = new GPSNode { Name = "Kvatric" };
            //var vlaska = new GPSStreet { Name = "Vlaska" };
            //var tbjNode = graph.NewNode(tbj);
            //var kvatricNode = graph.NewNode(kvatric);
            //tbjNode.ConnectBothWays(vlaska, 10, kvatricNode);

            var preds = new List<Predicate<GPSNode>>();
            preds.Add(g => g.Name == "TBJ");
            var path = tbjNode.GetBestPath(kvatricNode, preds);
            foreach (var n in path) Debug.WriteLine(n);

            //Program.DbContext.Graphs.Add(graph);
            //Program.DbContext.SaveChanges();

        }
    }
}
