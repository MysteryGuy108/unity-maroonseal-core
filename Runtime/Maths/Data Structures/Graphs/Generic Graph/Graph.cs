using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MaroonSeal.Maths.DataStructures.Graphs
{
    /// <summary>
    /// A class used to define a simple generic graph data structure.
    /// </summary>
    /// <typeparam name="TValue"> The value to be stored in each node. </typeparam>
    /// <typeparam name="TEdge"> The type of edge used to connect each node. </typeparam>
    abstract public class Graph<TValue, TEdge> : IEnumerable where TEdge : Edge<TValue>
    {
        Dictionary<TValue, Node> valueToNodeLUT;

        /// <summary>
        /// Class used to define a node that stores a value and a list of edges
        /// to other nodes.
        /// </summary>
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

            #region Edge Setting
            /// <summary>
            /// Adds edge to node.
            /// </summary>
            /// <param name="_edge"> Edge to add. </param>
            public void AddEdge(TEdge _edge) => Edges.Add(_edge);

            /// <summary>
            /// Removes edge from node.
            /// </summary>
            /// <param name="_edge"> Edge to remove. </param>
            public void RemoveEdge(TEdge _edge) => Edges.Remove(_edge);

            /// <summary>
            /// Remove edge connected to value.
            /// </summary>
            /// <param name="_value"></param>
            public void RemoveEdges(TValue _value) => Edges.RemoveWhere(x => x.Next.Equals(_value));

            /// <summary>
            /// Removes all edges from node.
            /// </summary>
            public void ClearEdges() => Edges.Clear();
            #endregion

            #region Edge Getting
            public List<TEdge> GetEdges() => Edges.ToList();

            /// <summary>
            /// Finds edges in node that connects to a value.
            /// </summary>
            /// <param name="_value">Target node value</param>
            /// <returns>Edge list</returns>
            public List<TEdge> FindEdges(TValue _value) => Edges.Where(cntx => cntx.Next.Equals(_value)).ToList();

            /// <summary>
            /// Finds first edge in node that connects to a value.
            /// </summary>
            /// <param name="_value">Node value</param>
            /// <returns>Edge</returns>
            public TEdge FindEdge(TValue _value)
            {
                List<TEdge> edges = FindEdges(_value);
                if (edges.Count <= 0) { return default; }
                TEdge edge = edges[0];
                edges.Clear();
                return edge;
            }
            #endregion
        }

        #region Constructor/Destructor
        public Graph() { valueToNodeLUT = new(); }

        ~Graph() {
            foreach(KeyValuePair<TValue, Node> nodePair in valueToNodeLUT)
            {
                nodePair.Value.ClearEdges();
            }

            valueToNodeLUT.Clear();
        }
        #endregion

        #region Getting
        protected Node GetNode(TValue _nodeValue) => valueToNodeLUT[_nodeValue]; 

        public List<TEdge> GetNodeEdges(TValue _nodeValue)
        {
            if (!valueToNodeLUT.ContainsKey(_nodeValue)) { return null; }
            return valueToNodeLUT[_nodeValue].Edges.ToList();
        }
        
        public TEdge GetEdge(TValue _from, TValue _to)
        {
            Node node = GetNode(_from);
            if (node == null) { return null; }
            return node.FindEdge(_to);
        }

        public List<TEdge> GetAllEdges()
        {
            List<TEdge> edges = new();
            foreach(KeyValuePair<TValue, Node> node in valueToNodeLUT)
            {
                edges.AddRange(node.Value.Edges);
            }
            return edges;
        }
        #endregion

        #region Adding
        /// <summary>
        /// Adds a node to the graph containing value.
        /// </summary>
        /// <param name="_value">Node value</param>
        public void AddNode(TValue _value)
        {
            if (_value == null) { return; }
            if (valueToNodeLUT.ContainsKey(_value)) { return; }
            valueToNodeLUT.Add(_value, new Node(_value));
        }

        /// <summary>
        /// Adds and edge to a node containing value.
        /// </summary>
        /// <param name="_value">Node value</param>
        /// <param name="_edge">Edge</param>
        public void AddEdge(TValue _value, TEdge _edge)
        {
            if (_value == null || _edge.Next == null) { return; }

            AddNode(_value);
            AddNode(_edge.Next);

            valueToNodeLUT[_value].AddEdge(_edge);
        }
        #endregion

        #region Removing
        /// <summary>
        /// Remove node containing value.
        /// </summary>
        /// <param name="_value">Node value</param>
        public void RemoveNode(TValue _value)
        {
            if (_value == null) { return; }
            if (!valueToNodeLUT.ContainsKey(_value)) { return; }

            Node node = valueToNodeLUT[_value];

            valueToNodeLUT.Remove(_value);
            node.ClearEdges();

            foreach(KeyValuePair<TValue, Node> nodePair in valueToNodeLUT)
            {
                nodePair.Value.RemoveEdges(_value);
            }
        }

        /// <summary>
        /// Removes an edge from a node containing value.
        /// </summary>
        /// <param name="_value">Node value</param>
        /// <param name="_edge">Edge to remove</param>
        public void RemoveEdge(TValue _value, TEdge _edge)
        {
            Node node = valueToNodeLUT.GetValueOrDefault(_value);
            if (node == null) { return; }
            node.RemoveEdge(_edge);
        }

        /// <summary>
        /// Removes all edges between two nodes
        /// </summary>
        /// <param name="_from">Start node</param>
        /// <param name="_to">Target node</param>
        public void RemoveAllEdges(TValue _from, TValue _to)
        {
            Node from = valueToNodeLUT.GetValueOrDefault(_from);
            from?.RemoveEdges(_to);
        }
        #endregion

        #region IEnumerable
        public IEnumerator GetEnumerator()
        {
            foreach(KeyValuePair<TValue, Node> node in valueToNodeLUT)
            {
                yield return new KeyValuePair<TValue, List<TEdge>>(node.Key, node.Value.Edges.ToList());
            }
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
