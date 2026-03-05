using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.NodeGraphs.Traversal {
    abstract public class NodeGraphDijkstraTraversal
    {
        public class DistanceTable {
            readonly public PriorityQueue<int> vertexQueue;
            readonly public HashSet<int> visitedVertices;
            readonly public Dictionary<int, int> cameFromLUT;
            readonly public Dictionary<int, float> distanceLUT;

            public DistanceTable() {
                vertexQueue = new PriorityQueue<int>();
                visitedVertices = new HashSet<int>();
                cameFromLUT = new Dictionary<int, int>();
                distanceLUT = new Dictionary<int, float>();
            }

            public void AddVertex(int _node) {
                if (visitedVertices.Contains(_node)) {return;}
                if (cameFromLUT.ContainsKey(_node)) {return;}

                cameFromLUT.Add(_node, -1);
                distanceLUT.Add(_node, Mathf.Infinity);
                vertexQueue.Enqueue(_node, Mathf.Infinity);
            }

            public void AddVisitedVertex(int _node) {
                if (visitedVertices.Contains(_node)) {return;}
                visitedVertices.Add(_node);
            }

            public void Clear() {
                vertexQueue.Clear();
                visitedVertices.Clear();
                cameFromLUT.Clear();
                distanceLUT.Clear();
            }
        }

        static public List<TData> FindNodePath<TData>(NodeGraphDirectionalWeighted<TData> _graph, TData _startNode, TData _endNode) {
            return FindNodePath(_graph, _startNode, _endNode, out float distance);
        }

        static public List<TData> FindNodePath<TData>(NodeGraphDirectionalWeighted<TData> _graph, TData _startNode, TData _endNode, out float _distance) {
            List<int> pathIndexes = FindPath(_graph, _graph[_startNode], _graph[_endNode], out _distance);

            if (pathIndexes == null) { return null; }

            List<TData> path = new();
            foreach(int index in pathIndexes) { path.Add(_graph[index]); }
            pathIndexes.Clear();
            return path;
        }

        static public List<int> FindPath<TData>(NodeGraphDirectionalWeighted<TData> _graph, int _startNode, int _endNode) {
            return FindPath(_graph, _startNode, _endNode, out float distance);
        }

        static public List<int> FindPath<TData>(NodeGraphDirectionalWeighted<TData> _graph, int _startNode, int _endNode, out float _distance) {
            DistanceTable table = new();
            _distance = Mathf.Infinity;
            table.AddVertex(_startNode);
            table.distanceLUT[_startNode] = 0.0f;

            while(table.vertexQueue.Count() > 0) {
                int current = table.vertexQueue.Dequeue();

                if (current == _endNode) {
                    List<int> nodePath = TracePathThroughTable(table, _startNode, _endNode, out _distance);
                    table.Clear();
                    return nodePath; 
                }

                List<int> currentNeighbours = _graph.GetAdjacentNodes(current);

                for(int i = 0; i < currentNeighbours.Count; i++) {
                    int neighbour = currentNeighbours[i];
                    if (table.visitedVertices.Contains(neighbour)) { continue; }
                    table.AddVertex(neighbour);

                    float weight = _graph.GetEdgeWeight(current, neighbour);

                    if (weight < table.distanceLUT[neighbour]) {
                        float cost = weight + table.distanceLUT[current];
                        table.distanceLUT[neighbour] = cost;
                        table.cameFromLUT[neighbour] = current;
                        table.vertexQueue.Enqueue(neighbour, cost);
                    }
                }

                table.AddVisitedVertex(current);
                currentNeighbours.Clear();
            }

            table.Clear();
            return null;    
        }
    
        static private List<int> TracePathThroughTable(DistanceTable _lookupTable, int _start, int _end, out float _distance) {

            int current = _end;

            List<int> path = new();
            _distance = 0.0f;

            int count = 0;
            do {
                path.Insert(0, current);
                _distance += _lookupTable.distanceLUT[current];
                if (current == _start) { return path; }
                
                current = _lookupTable.cameFromLUT[current];
                count++;

            } while(count <= _lookupTable.visitedVertices.Count);

            return null;
        }
    }
}