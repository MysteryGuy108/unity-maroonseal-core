using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs
{
    public class DoubleLookupTable<TValue> : OrderedLookupTableBase<double, TValue>
    {
        protected override int CompareKeys(double _a, double _b) => _a.CompareTo(_b);
    }
}
