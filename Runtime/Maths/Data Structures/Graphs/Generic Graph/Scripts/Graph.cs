using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MaroonSeal.Maths.DataStructures.Graphs
{
    /// <summary>
    /// A class used to define a simple generic graph data structure.
    /// </summary>
    /// <typeparam name="TNode"> The value type to be stored in each node. </typeparam>
    /// <typeparam name="TEdge"> The type of edge used to connect each node. </typeparam>
    public class Graph<TNode, TEdge>
    {
        readonly Dictionary<TNode, Dictionary<TNode, TEdge>> adjacency;

        public int NodeCount => adjacency.Count;
        public int EdgeCount { get; private set; }

        public TEdge this[TNode _from, TNode _to]
        {
            get => adjacency[_from][_to];
            set => adjacency[_from][_to] = value;
        }

        #region Constructor/Destructor
        public Graph() { adjacency = new(); }

        ~Graph() => Clear();
        #endregion

        #region Nodes
        public void AddNode(TNode _node) {
            if (adjacency.ContainsKey(_node)) { return; }
            adjacency.Add(_node, new());
        }
        public bool RemoveNode(TNode _node)
        {
            if (!adjacency.Remove(_node)) { return false; }

            foreach(Dictionary<TNode, TEdge> neighbours in adjacency.Values)
            {
                neighbours.Remove(_node);
            }
            return true;
        }
        public void Clear()
        {
            foreach(TNode node in Nodes) { ClearEdges(node); }
            adjacency.Clear();
        }
        #endregion

        #region Edges
        public void AddEdge(TNode _from, TNode _to, TEdge _edge)
        {
            AddNode(_from);
            AddNode(_to);
            adjacency[_from][_to] = _edge;
        }
        public void RemoveEdge(TNode _from, TNode _to) => adjacency[_from].Remove(_to);
        public void ClearEdges(TNode _from) => adjacency[_from].Clear();
        #endregion

        #region Contains
        public bool ContainsNode(TNode _node) => adjacency.ContainsKey(_node);
        public bool ContainsEdge(TNode _from, TNode _to) {
            if (!adjacency.TryGetValue(_from, out Dictionary<TNode, TEdge> neighbors)) { return false; }
            return neighbors.ContainsKey(_to);
        }
        #endregion

        #region Neighbours
        public IReadOnlyDictionary<TNode, TEdge> GetNodeNeighbors(TNode _node)
        {
            if (adjacency.TryGetValue(_node, out var neighbors))
            {
                return neighbors;
            }

            return new Dictionary<TNode, TEdge>();
        }
        #endregion

        #region IEnumerable
        public IEnumerable<TNode> Nodes => adjacency.Keys;
        public IEnumerable<IReadOnlyDictionary<TNode, TEdge>> Edges => adjacency.Values;
        #endregion
    
        #region Conversions
        public List<TNode> NodesToList()
        {
            List<TNode> nodes = new();
            foreach(TNode node in Nodes) { nodes.Add(node); }
            return nodes;
        }
        #endregion
    }
}
