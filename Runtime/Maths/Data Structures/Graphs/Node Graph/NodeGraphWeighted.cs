using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.NodeGraphs {
    [Obsolete("Use Generic Graph Instead")]
    public class NodeGraphWeighted<TData> : NodeGraphDirectionalWeighted<TData>
    {

        public NodeGraphWeighted() : base() { }
        public NodeGraphWeighted(NodeGraphWeighted<TData> _baseGraph) : base(_baseGraph) { }

        override public void SetEdgeWeight(int _nodeA, int _nodeB, float _weight) {
            adjacencyMatrix[_nodeA][_nodeB] = _weight;
            adjacencyMatrix[_nodeB][_nodeA] = _weight;
        }
    }
}