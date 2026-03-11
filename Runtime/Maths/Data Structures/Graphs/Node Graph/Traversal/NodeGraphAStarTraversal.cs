using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.NodeGraphs.Traversal {
    public class DistanceTable {
        readonly public PriorityQueue<int> vertexQueue;
        readonly public HashSet<int> visitedVertices;
        readonly public Dictionary<int, int> cameFromLUT;
        readonly public Dictionary<int, float> distanceLUT;

        public DistanceTable(List<int> _visitedVertices) {
            vertexQueue = new PriorityQueue<int>();
            visitedVertices = new HashSet<int>();
            cameFromLUT = new Dictionary<int, int>();
            distanceLUT = new Dictionary<int, float>();

            if (_visitedVertices == null) { return; }
            foreach(int vertex in _visitedVertices) { AddVisitedVertex(vertex); }
        }

        public void AddVertex(int _node) {
            if (visitedVertices.Contains(_node)) { return; }
            if (cameFromLUT.ContainsKey(_node)) { return; }

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
    
        public List<int> TracePath(int _start, int _end, out float _distance) {

            int current = _end;

            List<int> path = new();
            _distance = 0.0f;

            int count = 0;
            do {
                path.Insert(0, current);
                _distance += distanceLUT[current];
                if (current == _start) { return path; }
                
                current = this.cameFromLUT[current];
                count++;

            } while(count <= this.visitedVertices.Count);

            return null;
        }
    }
    
    static public class NodeGraphAStarTraversal
    {
        #region Traversal
        static public List<TData> FindNodePath<TData>(NodeGraphDirectionalWeighted<TData> _graph, TData _startNode, TData _endNode,
                                                    Func<TData, TData, float> _heuristic, List<TData> _visitedVertices = null) {

            return FindNodePath(_graph, _startNode, _endNode, _heuristic, out float distance, _visitedVertices);
        }

        static public List<TData> FindNodePath<TData>(NodeGraphDirectionalWeighted<TData> _graph, TData _startNode, TData _endNode,
                                                    Func<TData, TData, float> _heuristic, out float _distance, List<TData> _visitedVertices = null) {
            
            List<int> visistedVertexIndices = ConvertDataListToIndexList(_graph, _visitedVertices);
            List<int> pathIndexes = FindPath(_graph, _graph[_startNode], _graph[_endNode], _heuristic, out _distance, visistedVertexIndices);
            

            if (pathIndexes == null) { return null; }

            List<TData> dataPath = ConvertIndexListToDataList(_graph, pathIndexes);

            visistedVertexIndices?.Clear();
            pathIndexes?.Clear();

            return dataPath;
        }

        static public List<int> FindPath<TData>(NodeGraphDirectionalWeighted<TData> _graph, int _startNode, int _endNode,
                                                Func<TData, TData, float> _heuristic, List<int> _visitedVertices = null) {

            return FindPath(_graph, _startNode, _endNode, _heuristic, out float distance, _visitedVertices);
        }

        static public List<int> FindPath<TData>(NodeGraphDirectionalWeighted<TData> _graph, int _startNode, int _endNode,
                                                Func<TData, TData, float> _heuristic, out float _distance, List<int> _visitedVertices = null) {
            _distance = 0.0f;
            if (_startNode == _endNode) { return new List<int>() { _startNode };}
            
            DistanceTable table = new(_visitedVertices);

            _distance = Mathf.Infinity;
            table.AddVertex(_startNode);
            table.distanceLUT[_startNode] = 0.0f;

            while(table.vertexQueue.Count() > 0) {
                int current = table.vertexQueue.Dequeue();

                if (current == _endNode) {
                    List<int> nodePath = table.TracePath(_startNode, _endNode, out _distance);
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
                        float cost = weight + table.distanceLUT[current] + _heuristic(_graph[current], _graph[_endNode]);
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
        #endregion

        static public List<TData> ConvertIndexListToDataList<TData>(NodeGraphDirectionalWeighted<TData> _graph, List<int> _indexList) {
            if (_indexList == null) { return null; }
            List<TData> dataList = new();
            foreach(int index in _indexList) { dataList.Add(_graph[index]); }
            return dataList;
        }

        static public List<int> ConvertDataListToIndexList<TData>(NodeGraphDirectionalWeighted<TData> _graph, List<TData> _dataList) {
            if (_dataList == null) { return null; }
            List<int> indexList = new();
            foreach(TData _data in _dataList) { indexList.Add(_graph[_data]); }
            return indexList;
        }
    }
}