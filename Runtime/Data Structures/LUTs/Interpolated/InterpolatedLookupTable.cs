using System;
using UnityEngine;

using MaroonSeal.Maths.Algorithms;

namespace MaroonSeal.DataStructures.LUTs
{
    abstract public class InterpolatedLookupTable<TValue> : FloatLookupTable<TValue>
    {   
        #region Lerp Evaluations
        public TValue EvaluateKeyTime(float _time) => EvaluateKey(Mathf.Lerp(items[0].Key, items[^1].Key, _time));

        public TValue EvaluateKey(float _key) =>
            LerpEvaluate(_key, (i)=>this[i].Key, (i)=>this[i].Value, InterpolateValue);

        protected TReturn LerpEvaluate<TReturn>(float _search,
            Func<int, float> _indexToSearch, 
            Func<int, TReturn> _indexToReturn, 
            Func<TReturn, TReturn, float, TReturn> _returnLerp)
        {
            (int, int) segment = SearchIndices(_search, _indexToSearch, CompareKeys);

            float lowerKey = _indexToSearch(segment.Item1);
            float upperKey = _indexToSearch(segment.Item2);

            float lerpTime = Mathf.InverseLerp(lowerKey, upperKey, _search);

            TReturn lowerValue = _indexToReturn(segment.Item1);
            TReturn upperValue = _indexToReturn(segment.Item2);

            return _returnLerp(lowerValue, upperValue, lerpTime);
        }

        #endregion

        protected abstract TValue InterpolateValue(TValue _from, TValue _to, float _t);


    }

    [System.Serializable]
    public class FloatLookupTable : InterpolatedLookupTable<float>
    {
        #region Evaluating Values
        public (float, float) EvaluateValueNeighbours(float _value) {
            (int, int) indices = SearchIndices(_value, (i) => this[i].Value, (a, b) => a.CompareTo(b));
            return (this[indices.Item1].Key, this[indices.Item2].Key);
        }
        public float EvaluateValueFloor(float _value) => EvaluateValueNeighbours(_value).Item1;
        public float EvaluateValueCeiling(float _value) => EvaluateValueNeighbours(_value).Item2;

        public float EvaluateValueTime(float _time) => EvaluateValue(Mathf.Lerp(items[0].Value, items[^1].Value, _time));
        public float EvaluateValue(float _value) => 
            LerpEvaluate(_value, (i)=>this[i].Value, (i)=>this[i].Key, InterpolateValue);
        #endregion

        protected override float InterpolateValue(float _from, float _to, float _t) => Mathf.Lerp(_from, _to, _t);
    }

    [System.Serializable]
    public class DoubleLookupTable : InterpolatedLookupTable<double>
    {
        protected override double InterpolateValue(double _from, double _to, float _t) => _from + (_to - _from) * _t;
    }

    [System.Serializable]
    public class Vector2LookupTable : InterpolatedLookupTable<Vector2>
    {
        protected override Vector2 InterpolateValue(Vector2 _from, Vector2 _to, float _t) => Vector2.Lerp(_from, _to, _t);
    }

    [System.Serializable]
    public class Vector3LookupTable : InterpolatedLookupTable<Vector3>
    {
        protected override Vector3 InterpolateValue(Vector3 _from, Vector3 _to, float _t) => Vector3.Lerp(_from, _to, _t);
    }

    [System.Serializable]
    public class ColourLookupTable : InterpolatedLookupTable<Color>
    {
        protected override Color InterpolateValue(Color _from, Color _to, float _t) => Color.Lerp(_from, _to, _t);
    }
}
