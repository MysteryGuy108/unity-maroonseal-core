using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MaroonSeal.Maths.DataStructures.NodeGraphs {
    public class NodeGraphTreeWeighted<TData> : NodeGraphWeighted<TData>
    {
        public NodeGraphTreeWeighted() : base() { }

        public NodeGraphTreeWeighted(NodeGraphWeighted<TData> _inputGraph) : base() {

            // Copying nodes.
            for(int i = 0; i < _inputGraph.NodeCount; i++) { PushNode(_inputGraph[i]); }

            // Adding edges.
            for(int i = 0; i < _inputGraph.NodeCount; i++) {
                foreach(int neighbourIndex in _inputGraph.GetAdjacentNodes(i)) {
                    SetEdgeWeight(i, neighbourIndex, _inputGraph.GetEdgeWeight(i, neighbourIndex));
                }
            }
        }

        public override void SetEdgeWeight(int _nodeA, int _nodeB, float _weight) {
            float curWeight = adjacencyMatrix[_nodeA][_nodeB];
            base.SetEdgeWeight(_nodeA, _nodeB, _weight);

            if (_weight >= Mathf.Infinity) { return; }

            if (CheckIsCyclic()) {
                base.SetEdgeWeight(_nodeA, _nodeB, curWeight);
            }
        }

        private bool CheckIsCyclic() {
            bool[] visited = new bool[NodeCount];

            for (int i = 0; i < NodeCount; i++) {
                if (!visited[i]) {
                    if (FindCycles(i, visited, -1)) {
                        return true;
                    } 
                }
            }
    
            return false;
        }

        private bool FindCycles(int _node, bool[] _visited, int _parent) {
            _visited[_node] = true;

            List<int> adjacentNodes = GetAdjacentNodes(_node);

            foreach(int i in adjacentNodes) {

                if (!_visited[i]) {
                    if (FindCycles(i, _visited, _node))
                        return true;
                }
                else if (i != _parent)
                    return true;
            }
            return false;
        }
    }
}