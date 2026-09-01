using System;
using UnityEngine;

namespace MaroonSeal.DataStructures.Graphs
{
    public readonly struct GraphEdge<TNode, TEdge> : IEquatable<GraphEdge<TNode, TEdge>>
    {
        public TNode From { get; }
        public TNode To  { get; }
        public TEdge Value  { get; }

        public GraphEdge(TNode _from, TNode _to, TEdge _value)
        {
            From = _from;
            To = _to;
            Value = _value;
        }

        public bool Equals(GraphEdge<TNode, TEdge> _other) =>
            this.From.Equals(_other.From) && 
            this.To.Equals(_other.To) && 
            this.Value.Equals(_other.Value);
    }
}
