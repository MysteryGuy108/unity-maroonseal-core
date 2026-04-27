using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs.Generic
{
    /// <summary>
    /// A class used to define a graph edge to be used in the Graph class.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Edge<TValue>
    {
        public TValue Next { get; }

        public Edge(TValue _next)
        {
            Next = _next;
        }
    }

    /// <summary>
    /// A class used to define a weighted graph edge to be used in the Graph and WeightedGraph classes.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class WeightedEdge<TValue> : Edge<TValue>
    {
        public float Weight { get; }
        
        public WeightedEdge(TValue _next, float _weight) : base(_next)
        {
            Weight = _weight;
        }
    }
}
