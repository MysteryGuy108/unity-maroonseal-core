using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.Generic
{
    public class Graph<TValue, TEdge> : IEnumerable where TEdge : Edge<TValue>
    {
        Dictionary<TValue, Node> nodes;

        public class Node
        {
            public TValue Value { get; }
            public HashSet<TEdge> Edges { get; }

            #region Constructor/Destructor
            public Node(TValue _value)
            {
                Value = _value;
                Edges = new();
            }

            ~Node() { Edges.Clear(); }
            #endregion

            #region Edges
            public void AddEdge(TEdge _edge)
            {
                Edges.Add(_edge);
            }

            public void ClearEdges() { Edges.Clear(); }
            #endregion

            #region Nodes
            public void RemoveEdge(TEdge _edge)
            {
                Edges.Remove(_edge);
            }

            public void RemoveNode(TValue _node)
            {
                Edges.RemoveWhere(x => x.Next.Equals(_node));
            }
            #endregion
        }

        #region Constructor/Destructor
        public Graph() 
        {
            nodes = new();
        }

        ~Graph() {
            foreach(KeyValuePair<TValue, Node> nodePair in nodes)
            {
                nodePair.Value.ClearEdges();
            }

            nodes.Clear();
        }
        #endregion

        #region Getting
        protected Node GetNode(TValue _nodeValue) => nodes[_nodeValue]; 

        public List<TEdge> GetNodeEdges(TValue _nodeValue)
        {
            if (!nodes.ContainsKey(_nodeValue)) { return null; }
            return nodes[_nodeValue].Edges.ToList();
        }

        public List<TEdge> GetAllEdges()
        {
            List<TEdge> edges = new();
            foreach(KeyValuePair<TValue, Node> node in nodes)
            {
                edges.AddRange(node.Value.Edges);
            }
            return edges;
        }
        #endregion

        #region Adding
        public void AddNode(TValue _nodeValue)
        {
            if (_nodeValue == null) { return; }
            if (nodes.ContainsKey(_nodeValue)) { return; }
            nodes.Add(_nodeValue, new Node(_nodeValue));
        }

        public void AddEdge(TValue _nodeValue, TEdge _edge)
        {
            if (_nodeValue == null || _edge.Next == null) { return; }

            AddNode(_nodeValue);
            AddNode(_edge.Next);

            nodes[_nodeValue].AddEdge(_edge);
        }
        #endregion

        #region Removing
        public void RemoveNode(TValue _nodeValue)
        {
            if (_nodeValue == null) { return; }
            if (!nodes.ContainsKey(_nodeValue)) { return; }

            Node node = nodes[_nodeValue];

            nodes.Remove(_nodeValue);
            node.ClearEdges();

            foreach(KeyValuePair<TValue, Node> nodePair in nodes)
            {
                nodePair.Value.RemoveNode(_nodeValue);
            }
        }

        public void RemoveEdge(TValue _nodeValue, TEdge _edge)
        {
            Node node = nodes.GetValueOrDefault(_nodeValue);
            if (node == null) { return; }
            node.RemoveEdge(_edge);
        }

        public void RemoveAllEdges(TValue _from, TValue _to)
        {
            Node from = nodes.GetValueOrDefault(_from);
            if (from == null) { return; }
            from.RemoveNode(_to);
        }
        #endregion

        #region IEnumerator
        public IEnumerator GetEnumerator()
        {
            return nodes.GetEnumerator();
        }
        #endregion
    }

    public class Graph<TValue> : Graph<TValue, Edge<TValue>>
    {
        public void AddEdge(TValue _value, TValue _next)
        {
            Edge<TValue> newEdge = new(_next);
            AddEdge(_value, newEdge);
        }
    }
}
