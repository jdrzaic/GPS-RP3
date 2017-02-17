using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Priority_Queue;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace GPS
{
    /// <summary>
    /// Represents a generic graph. Contains nodes along with weighted, directed edges between them.
    /// 
    /// The API is implemented via the Node struct.
    /// </summary>
    /// <typeparam name="TNode">Data that's associated with each node.</typeparam>
    /// <typeparam name="TEdge">Data associated with edges.</typeparam>
    [Table("Graph")]
    public class Graph<TNode, TEdge>
    {
        [Key]
        public int GraphId;
        public void LoadFromDb() { throw new NotImplementedException(); }
        public void SaveToDb() { throw new NotImplementedException(); }

        private Dictionary<int, Dictionary<int, Tuple<TEdge, double>>> connections;
        private Dictionary<int, TNode> data;
        private SortedSet<int> usedIds;
        private SortedSet<int> deletedIds;

        public IEnumerable<Node> Nodes => data.Keys.Select(NodeFromIndex);

        private static Func<int, Graph<TNode, TEdge>, Node> makeNode;
        private Node NodeFromIndex(int idx) => makeNode(idx, this);

        static Graph()
        {
            RuntimeHelpers.RunClassConstructor(typeof(Node).TypeHandle);
        }

        public Graph()
        {
            connections = new Dictionary<int, Dictionary<int, Tuple<TEdge, double>>>();
            data = new Dictionary<int, TNode>();
            usedIds = new SortedSet<int>();
            deletedIds = new SortedSet<int>();
        }

        /// <summary>
        /// Make a new Node given the associated data.
        /// </summary>
        /// <param name="data">Associated data</param>
        /// <returns>A fresh Node</returns>
        public Node NewNode(TNode data)
        {
            int id = usedIds.Count == 0 ? 1 : usedIds.Max() + 1;
            usedIds.Add(id);
            connections.Add(id, new Dictionary<int, Tuple<TEdge, double>>());
            this.data.Add(id, data);
            return NodeFromIndex(id);
        }

        /// <summary>
        /// Represents a single node in a graph. The operations on this value mutate the graph which
        /// holds this node.
        /// </summary>
        public struct Node
        {
            static Node()
            {
                makeNode = (idx, graph) => new Node(idx, graph);
            }

            /// <summary>
            /// Get the associated data.
            /// </summary>
            public TNode Data => graph.data[idx];

            /// <summary>
            /// The the list of edges going from this node.
            /// </summary>
            public IEnumerable<Tuple<Node, TEdge, double>> Connections
            {
                get
                {
                    var _graph = graph;
                    return graph.connections[idx].Select(t => Tuple.Create(_graph.NodeFromIndex(t.Key), t.Value.Item1, t.Value.Item2));
                }
            }

            private int idx;
            private Graph<TNode, TEdge> graph;

            private Node(int idx, Graph<TNode, TEdge> graph) { this.idx = idx; this.graph = graph; }

            /// <summary>
            /// Connect two nodes. They must belong to the same graph.
            /// </summary>
            /// <param name="distance"></param>
            /// <param name="other"></param>
            public void ConnectTo(TEdge data, double distance, Node other)
            {
                if (graph != other.graph) throw new InvalidOperationException("Cannot connect nodes that don't belong to the same graph");
                if (graph.deletedIds.Contains(idx)) throw new InvalidOperationException("Node is deleted");
                graph.connections[idx][other.idx] = Tuple.Create(data, distance);
            }

            public void ConnectBothWays(TEdge data, double distance, Node other)
            {
                ConnectTo(data, distance, other);
                other.ConnectTo(data, distance, this);
            }

            /// <summary>
            /// Disconnect two nodes. They must belong to the same graph.
            /// </summary>
            /// <param name="other"></param>
            public void DisconnectFrom(Node other)
            {
                if (graph != other.graph) throw new InvalidOperationException("Cannot disconnect nodes that don't belong to the same graph");
                if (graph.deletedIds.Contains(idx)) throw new InvalidOperationException("Node is deleted");
                graph.connections[idx].Remove(other.idx);
            }

            /// <summary>
            /// Check if two nodes are connected.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool IsConnectedTo(Node other) => graph.connections[idx].ContainsKey(other.idx);

            /// <summary>
            /// Delete a given node from the graph. This removes all edges to and from this node.
            /// Using this node, or any other Node object that represents the same node in the graph throws an exception.
            /// </summary>
            public void Delete()
            {
                Node _this = this;
                graph.Nodes.Where(n => n.IsConnectedTo(_this)).ForEach(n => n.DisconnectFrom(_this));
                graph.deletedIds.Add(idx);
                graph.connections.Remove(idx);
                graph.data.Remove(idx);
            }

            public static bool operator ==(Node node1, Node node2)
            {
                return node1.graph == node2.graph && node1.idx == node2.idx;
            }

            public static bool operator !=(Node node1, Node node2) => !(node1 == node2);

            public override bool Equals(object obj)
            {
                if (obj.GetType() != typeof(Node)) return false;
                return this == (Node)obj;
            }

            public override int GetHashCode()
            {
                return graph.GetHashCode() ^ idx.GetHashCode();
            }

            public override string ToString()
            {
                return idx.ToString();
            }

            public Tuple<List<Tuple<TEdge, Node>>, double> GetShortestPath(ISet<Node> to)
            {
                var q = new SimplePriorityQueue<Node, double>();
                q.Enqueue(this, 0);
                var distances = new Dictionary<Node, double>();
                distances[this] = 0;
                var previous = new Dictionary<Node, Tuple<TEdge, Node>>();
                while (q.Count > 0)
                {
                    var u = q.Dequeue();
                    foreach (var v in u.Connections)
                    {
                        double alt = distances[u] + v.Item3;
                        if (!distances.ContainsKey(v.Item1) || alt < distances[v.Item1])
                        {
                            distances[v.Item1] = alt;
                            previous[v.Item1] = Tuple.Create(v.Item2, u);
                            q.Enqueue(v.Item1, alt);
                        }
                    }
                }
                var actualTarget = to.OrderBy(n => distances.ContainsKey(n) ? distances[n] : double.PositiveInfinity)
                                     .First();
                var path = new List<Tuple<TEdge, Node>>();
                if (!previous.ContainsKey(actualTarget)) return Tuple.Create(path, double.PositiveInfinity);
                Node curr = actualTarget;
                while (curr != this)
                {
                    path.Add(Tuple.Create(previous[curr].Item1, curr));
                    curr = previous[curr].Item2;
                }
                path.Reverse();
                return Tuple.Create(path, distances[actualTarget]);
            }

            /// <summary>
            /// Get the shortest path such that all the given predicates are satisfied by some node along the path.
            /// </summary>
            /// <param name="to">The destination.</param>
            /// <param name="pathCriteria">A collection of node criteria.</param>
            /// <returns></returns>
            public IEnumerable<Tuple<TEdge, Node>> GetBestPath(Node to, IEnumerable<Predicate<TNode>> pathCriteria)
            {
                var graph = this.graph;
                var groups = pathCriteria.Select(p => graph.Nodes.Where(n => p(n.Data))).ToList();
                IEnumerable<Tuple<TEdge, Node>> bestPath = null;
                double bestDistance = double.PositiveInfinity;
                foreach (var perm in groups.Permutations())
                {
                    IEnumerable<Tuple<TEdge, Node>> path = new List<Tuple<TEdge, Node>>();
                    var pathPartStart = this;
                    double dist = 0;
                    foreach (var group in perm.Concat(new[] { new[] { to } }))
                    {
                        var pathPart = pathPartStart.GetShortestPath(group.ToImmutableSortedSet());
                        if (pathPart.Item1.Count > 0)
                        {
                            pathPartStart = pathPart.Item1.Last().Item2;
                            path = path.Concat(pathPart.Item1);
                            dist += pathPart.Item2;
                        }
                    }
                    if (dist < bestDistance)
                    {
                        dist = bestDistance;
                        bestPath = path;
                    }
                }
                return bestPath;
            }
        }
    }
}
