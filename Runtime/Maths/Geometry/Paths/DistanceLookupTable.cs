using UnityEngine;

using MaroonSeal.DataStructures.LUTs;

namespace MaroonSeal.Maths.Geometry.Paths
{
    public class DistanceLookupTable
    {
        [SerializeField] private FloatLookupTable lookupTable;
        
        public bool isDirty;

        public LUTItem<float, float> this[int _index]
        {
            get => lookupTable[_index];
            set => lookupTable[_index] = value;
        }

        public int Count => lookupTable.Count;

        public void Add(float _distance)
        {
            isDirty = true;
            
        }

        public void Clear() => lookupTable.Clear();
    }
}
