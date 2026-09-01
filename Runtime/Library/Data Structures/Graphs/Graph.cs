using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MaroonSeal.DataStructures.Graphs
{
    /// <summary>
    /// A class used to define a simple generic graph data structure.
    /// </summary>
    /// <typeparam name="TNode"> The value type to be stored in each node. </typeparam>
    /// <typeparam name="TEdge"> The type of edge used to connect each node. </typeparam>
    public class Graph<TNode, TEdge> : IGraph<TNode, TEdge>
    {
        readonly Dictionary<TNode, Dictionary<TNode, TEdge>> adjacency;

        public int NodeCount => adjacency.Count;

        public TEdge this[TNode _from, TNode _to]
        {
            get => adjacency[_from][_to];
            set => adjacency[_from][_to] = value;
        }

        #region Constructor/Destructor
        public Graph() => adjacency = new();

        ~Graph() => Clear();
        #endregion

        #region Nodes
        public IEnumerable<TNode> Nodes => adjacency.Keys;

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
        #endregion

        #region Edges
        public void AddEdge(TNode _from, TNode _to, TEdge _edge)
        {
            AddNode(_from);
            AddNode(_to);
            adjacency[_from][_to] = _edge;
        }
        public void AddEdge(GraphEdge<TNode, TEdge> _edge) => AddEdge(_edge.From, _edge.To, _edge.Value);

        public void RemoveEdge(TNode _from, TNode _to) => adjacency[_from].Remove(_to);

        public IEnumerable<GraphEdge<TNode, TEdge>> GetEdges(TNode _node)
        {
            adjacency.TryGetValue(_node, out Dictionary<TNode, TEdge> neighbours);
            if (neighbours == null) { yield break; }
            foreach(KeyValuePair<TNode, TEdge> neighbour in neighbours)
            {
                yield return new(_node, neighbour.Key, neighbour.Value);
            }
        }

        public IEnumerable<GraphEdge<TNode, TEdge>> GetAllEdges()
        {
            foreach(TNode node in Nodes) {
                foreach(GraphEdge<TNode, TEdge> edge in GetEdges(node)) {
                    yield return edge;
                }
            }
        }
        #endregion

        #region Contains
        public bool ContainsNode(TNode _node) => adjacency.ContainsKey(_node);
        public bool ContainsEdge(TNode _from, TNode _to) {
            if (!adjacency.TryGetValue(_from, out Dictionary<TNode, TEdge> neighbors)) { return false; }
            return neighbors.ContainsKey(_to);
        }
        #endregion

        #region Clear
        public void Clear()
        {
            ClearEdges();
            adjacency.Clear();
        }

        public void ClearEdges(TNode _from) => adjacency[_from].Clear();

        public void ClearEdges() { foreach(TNode node in Nodes) { ClearEdges(node); } }
        #endregion
    }
}
