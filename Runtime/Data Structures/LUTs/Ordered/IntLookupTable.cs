using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs {
    [System.Serializable]
    public class IntLookupTable<TValue> : OrderedLookupTableBase<int, TValue>
    {
        protected override int CompareKeys(int _a, int _b) => _a.CompareTo(_b);
    }

    [System.Serializable]
    public class IntLookupTable : IntLookupTable<int> {}
}