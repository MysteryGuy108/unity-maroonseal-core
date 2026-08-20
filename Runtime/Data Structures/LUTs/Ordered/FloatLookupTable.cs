using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs {
    [System.Serializable]
    public class FloatLookupTable<TValue> : OrderedLookupTableBase<float, TValue>
    {
        protected override int CompareKeys(float _a, float _b) => _a.CompareTo(_b);
        public (TValue, TValue) EvaluateKeyNeighbours(float _key, out float _time) {
            (int, int) indices = SearchIndices(_key, (i) => this[i].Key, CompareKeys);
            _time = Mathf.InverseLerp(this[indices.Item1].Key, this[indices.Item2].Key, _key);

            return (this[indices.Item1].Value, this[indices.Item2].Value);
        }
    }
}