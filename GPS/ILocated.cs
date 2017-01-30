using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS
{
    /// <summary>
    /// Types that have a location.
    /// </summary>
    interface ILocated
    {
        PointF Location { get; }
    }

    static class LocatedGraphExtensions
    {
        /// <summary>
        /// If the node data has locations, nodes can be connected without explicitly providing their distance.
        /// </summary>
        /// <typeparam name="TNode">The type of the associated data.</typeparam>
        /// <typeparam name="TEdge">The type associated to edges.</typeparam>
        /// <param name="thisNode"></param>
        /// <param name="other"></param>
        public static void ConnectTo<TNode, TEdge>(this Graph<TNode, TEdge>.Node thisNode, TEdge data, Graph<TNode, TEdge>.Node other) where TNode : ILocated
        {
            thisNode.ConnectTo(
                data,
                Math.Sqrt(Math.Pow(other.Data.Location.X - thisNode.Data.Location.X, 2) +
                          Math.Pow(other.Data.Location.X - thisNode.Data.Location.X, 2)), 
                other);
        }
    }
}
