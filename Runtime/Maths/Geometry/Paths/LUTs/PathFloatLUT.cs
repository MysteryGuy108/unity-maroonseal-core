using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths.LUTs {
    [System.Serializable]
    sealed public class PathFloatLUT : PathLUT<float>
    {
        #region LUT
        override public void AddValue(float _value) {
            _value += values[^1];
            base.AddValue(_value);
        }

        public void AddPath(IShapePath _path, int _resolution) {
            Clear();
            
            for (int i = 1; i < _resolution; i++) {
                float time = i / (float)(_resolution - 1.0f);
                float previousTime = (i-1) / (float)(_resolution - 1.0f);

                float distance = Vector3.Distance(_path.GetPositionAtTime(time), _path.GetPositionAtTime(previousTime));
                AddValue(distance);
            }
        }

        override public void Clear() {
            base.Clear();
            values.Add(0.0f);
        }
        
        override public int GetIndexAtValue(float _value)
        {
            _value = Mathf.Clamp(_value, 0.0f, values[^1]);
            return BinarySearch(_value, 0, this.Resolution - 1, GetValueAtIndex);
        }
        
        override public float EvaulateValueAtTime(float _time)
        {
            int index = GetIndexAtTime(_time);
            if (index + 1 >= Resolution) { return values[^1]; }
            float lerpTime = Mathf.InverseLerp(GetTimeAtIndex(index), GetTimeAtIndex(index + 1), _time);
            return Mathf.Lerp(GetValueAtIndex(index), GetValueAtIndex(index + 1), lerpTime);
        }

        override public float EvaulateTimeAtValue(float _value) {
            int index = GetIndexAtValue(_value);
            if (index+1 >= Resolution) { return 0.0f; }
            float lerpTime = Mathf.InverseLerp(GetValueAtIndex(index), GetValueAtIndex(index+1), _value);
            return Mathf.Lerp(GetTimeAtIndex(index), GetTimeAtIndex(index+1), lerpTime);
        }
        #endregion

        #region Binary Search
        private int BinarySearch(float _value, int _low, int _high, Func<int, float> _LUT) {
            int mid = (_high + _low)/2;

            if (_low == _high) { return _low; }
            else if (_value >= _LUT(mid) && _value < _LUT(mid+1)) { return mid; }
            else if (_value < _LUT(mid)) { return BinarySearch(_value, _low, mid, _LUT); }
            else if (_value >= _LUT(mid+1)) { return BinarySearch(_value, mid+1, _high, _LUT); }

            return -1;
        }

        public float FindLocalMinimum(float _minT, float _maxT, Func<float, float> _timeWeightFunction, float _e = 0.0001f) {
            float min = _minT;
            float max = _maxT;
            float mid;
        
            do {
                mid = (max + min) / 2.0f;

                float minWeight = _timeWeightFunction(mid-_e);
                float maxWeight = _timeWeightFunction(mid+_e);

                if (minWeight < maxWeight) { max = mid; }
                else{ min = mid; }

            } while (max - min > _e);

            return mid;
        }

        public float FindLocalMinimum(float _minT, float _maxT, IShapePath _path, Vector3 _position, float _e = 0.0001f) {
            return this.FindLocalMinimum(_minT, _maxT, GetSqrDistanceAtTime, _e);

            float GetSqrDistanceAtTime(float _t) => (_path.GetPositionAtTime(_t) - _position).sqrMagnitude;
        }
        #endregion
    }
}