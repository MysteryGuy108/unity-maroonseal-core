using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.DataStructures.Graphs
{
    public interface IGraph<TNode, TEdge>
    {
        public IEnumerable<TNode> Nodes { get; }
        public IEnumerable<GraphEdge<TNode, TEdge>> GetEdges(TNode _node);
    }
}
