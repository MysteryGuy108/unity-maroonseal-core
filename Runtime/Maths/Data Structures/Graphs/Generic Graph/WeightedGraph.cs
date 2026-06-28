using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs
{
    abstract public class WeightedGraph<TValue, TWeightedEdge> : Graph<TValue, TWeightedEdge> where TWeightedEdge : WeightedEdge<TValue>
    {
        public float GetEdgeWeight(TValue _value, TValue _next)
        {
            TWeightedEdge edge = GetEdge(_value, _next);
            if (edge == null) { return Mathf.Infinity; }
            return edge.Weight;
        }
    }

    public class WeightedGraph<TValue> : WeightedGraph<TValue, WeightedEdge<TValue>>
    {
        public void AddEdge(TValue _value, TValue _next, float _weight = 0.0f)
        {
            WeightedEdge<TValue> newEdge = new(_next, _weight);
            AddEdge(_value, newEdge);
        }


    }
}
