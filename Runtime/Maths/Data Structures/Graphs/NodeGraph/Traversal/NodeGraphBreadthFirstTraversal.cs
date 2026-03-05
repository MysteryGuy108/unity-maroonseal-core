using System;
using System.Collections;
using System.Collections.Generic;

namespace MaroonSeal.Maths.DataStructures.NodeGraphs.Traversal {
    static public class NodeGraphBreadthFirstTraversal {

        #region Path to Node that meets condition
        static public List<TData> FindNodePath<TData, TConditionData>(NodeGraphDirectionalWeighted<TData> _graph, TData _startNode, 
            TConditionData _endConditionData, Func<TData, TConditionData, bool> _endCondition) {

            HashSet<TData> visitedNodes = new();

            List<TData> nodePath = SearchNode(_graph, _startNode, _endConditionData, _endCondition, visitedNodes);
            visitedNodes.Clear();

            return nodePath;
        }

        static private List<TData> SearchNode<TData, TConditionData>(NodeGraphDirectionalWeighted<TData> _graph, TData _node, 
                TConditionData _endConditionData, Func<TData, TConditionData, bool> _endCondition, HashSet<TData> _visitedNodes) {

            if (_visitedNodes.Contains(_node)) { return null; }
            _visitedNodes.Add(_node);

            List<TData> adjacentNodes = _graph.GetAdjacentNodes(_node);
            
            foreach(TData neighbour in adjacentNodes) {
                if (_endCondition.Invoke(neighbour, _endConditionData)) { 
                    return new List<TData>(2) { _node, neighbour }; 
                }
            }

            foreach(TData neighbour in adjacentNodes) {
                List<TData> neighbourSearch = SearchNode(_graph, neighbour, _endConditionData, _endCondition, _visitedNodes);
                if (neighbourSearch == null) { continue; }
                neighbourSearch.Insert(0, _node);
                return neighbourSearch;
            }

            return null;
        }
        #endregion

        #region Path Between Nodes
        static public List<TData> FindNodePath<TData>(NodeGraphDirectionalWeighted<TData> _graph, TData _startNode, HashSet<TData> _endNodes) {
            return FindNodePath(_graph, _startNode, _endNodes, IsNodeInSet);
        }

        static public List<TData> FindNodePath<TData>(NodeGraphDirectionalWeighted<TData> _graph, TData _startNode, TData _endNode) {
            return FindNodePath(_graph, _startNode, new HashSet<TData>() { _endNode }, IsNodeInSet);
        }

        static private bool IsNodeInSet<TData>(TData _node, HashSet<TData> _nodeEndSet) {
            return _nodeEndSet.Contains(_node);
        }
        #endregion
    }
}