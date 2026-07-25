using System;
using UnityEngine;

using MaroonSeal.Maths.Algorithms;

namespace MaroonSeal.DataStructures.LUTs
{
    abstract public class InterpolatedLookupTable<TValue> : FloatLookupTable<TValue>
    {
        public TValue Evaluate(float _time) => EvaluateKey(items[^1].Key * _time);

        public TValue EvaluateKey(float _time) => EvaluateKey(_time, (cntx) => items[cntx].Key, (cntx) => items[cntx].Value);

        protected TValue EvaluateKey(float _time, Func<int, float> _keyIndex, Func<int, TValue> _valueIndex)
        {
            EnsureSorted();

            if (items.Count == 0) { throw new InvalidOperationException("Interpolated table has no points."); }
            if (items.Count == 1) { return _valueIndex.Invoke(0); }

            (int, int) segment = SearchAlgorithms.BinarySearch(_time, items.Count, _keyIndex);

            float lowerKey = _keyIndex.Invoke(segment.Item1);
            float upperKey = _keyIndex.Invoke(segment.Item2);

            float lerpTime = Mathf.InverseLerp(lowerKey, upperKey, _time);

            TValue lowerValue = _valueIndex.Invoke(segment.Item1);
            TValue upperValue = _valueIndex.Invoke(segment.Item2);

            return InterpolateValue(lowerValue, upperValue, lerpTime);
        }

        protected abstract TValue InterpolateValue(TValue _from, TValue _to, float _t);
    }

    [System.Serializable]
    public class FloatLookupTable : InterpolatedLookupTable<float>
    {
        public float EvaluateInverse(float _time) => EvaluateKey(items[^1].Value * _time);
        public float EvaluateValue(float _value) => EvaluateKey(_value, (cntx) => items[cntx].Value, (cntx) => items[cntx].Key);

        protected override float InterpolateValue(float _from, float _to, float _t) => Mathf.Lerp(_from, _to, _t);
    }

    [System.Serializable]
    public class DoubleLookupTable : InterpolatedLookupTable<double>
    {
        public double EvaluateInverse(float _time) => EvaluateKey((float)items[^1].Value * _time);
        public double EvaluateValue(float _value) => EvaluateKey(_value, (cntx) => (float)items[cntx].Value, (cntx) => items[cntx].Key);

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
