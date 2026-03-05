using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.Generic
{
    public class Edge<TValue>
    {
        public TValue Next { get; }

        public Edge(TValue _next)
        {
            Next = _next;
        }
    }

    public class WeightedEdge<TValue> : Edge<TValue>
    {
        public float Weight { get; }
        
        public WeightedEdge(TValue _next, float _weight) : base(_next)
        {
            Weight = _weight;
        }
    }
}
