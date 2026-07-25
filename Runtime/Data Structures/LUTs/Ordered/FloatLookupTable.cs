using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs {
    [System.Serializable]
    public class FloatLookupTable<TValue> : OrderedLookupTableBase<float, TValue>
    {
        protected override int CompareKeys(float _a, float _b) => _a.CompareTo(_b);
    }
}