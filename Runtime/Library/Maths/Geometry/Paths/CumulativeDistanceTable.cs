using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Algorithms;

namespace MaroonSeal.Maths.Geometry.Paths
{
    [System.Serializable]
    internal class CumulativeDistanceTable
    {
        [SerializeField] private List<float> table;
        public int Count { get; private set; }
        public float TotalLength => table.Count == 0 ? 0f : table[^1];

        public void Clear() => table.Clear();

        #region Constructor/Destructor
        public CumulativeDistanceTable(int _count) => table = new(_count);
        public CumulativeDistanceTable() : this(0) {}
        #endregion    
        
        #region Table Building
        public void RebuildSegments(int _segmentCount, Func<int, float> _lengthOf)
        {
            Count = _segmentCount;

            table.Clear();
            table.Add(0f);

            for (int i = 0; i < _segmentCount; i++)
            {
                table.Add(table[i] + _lengthOf(i));
            }
        }

        public void RebuildSamples(int _sampleCount, Func<int, int, float> _distanceBetween)
            => RebuildSegments(_sampleCount, (index) => _distanceBetween(index, index+1));
        #endregion

        #region Index Searching
        public float TimeAt(int _index) => _index / (float)Count;    
        public float DistanceAt(int index) => table[index];
        #endregion

        #region Table Evaluating
        public float EvaluateTime(float _time) => EvaluateValue(_time, TimeAt, DistanceAt);
        public float EvaluateDistance(float _distance) => EvaluateValue(_distance, DistanceAt, TimeAt);
        private float EvaluateValue(float _search, Func<int, float> _indexToSearch, Func<int, float> _indexToReturn) {

            (int, int) segment = BinarySearch.Search(_search, table.Count, _indexToSearch, (a, b) => a.CompareTo(b));

            float lowerKey = _indexToSearch(segment.Item1);
            float upperKey = _indexToSearch(segment.Item2);

            float blend = Mathf.InverseLerp(lowerKey, upperKey, _search);

            float lowerValue = _indexToReturn(segment.Item1);
            float upperValue = _indexToReturn(segment.Item2);

            return Mathf.Lerp(lowerValue, upperValue, blend);
        }
        #endregion
    }
}
