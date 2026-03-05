using System;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    public class LerpList<TData> : SerializableListBase<TData> 
    {
        public float GetTimeAtIndex(int _index) { return _index/(this.Count-1.0f); }

        public int GetIndexAtTime(float _time) { return Mathf.FloorToInt(_time * (this.Count-1)); }
        public int GetIndexAtTimeCeil(float _time) { return Mathf.CeilToInt(_time * (this.Count-1)); }
        public int GetIndexAtTimeRounded(float _time) { 
            int floorIndex = GetIndexAtTime(_time);
            int ceilIndex = GetIndexAtTimeCeil(_time);

            float floor = GetTimeAtIndex(floorIndex);
            float ceil = GetTimeAtIndex(ceilIndex);
            float mid = Mathf.InverseLerp(floor, ceil, _time);
            return mid < 0.5f ? floorIndex : ceilIndex;
        }

        public float GetTimeAtItem(TData _item) { return GetTimeAtIndex(this.IndexOf(_item)); }

        public TData GetItemAtTime(float _time) { return this[GetIndexAtTime(_time)]; }
        public TData GetItemAtTimeCeil(float _time) { return this[GetIndexAtTimeCeil(_time)]; }
        public TData GetItemAtTimeRounded(float _time) { return this[GetIndexAtTimeRounded(_time)]; }
    }
}

