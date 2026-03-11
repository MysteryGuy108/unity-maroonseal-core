using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths.LUTs
{
    [System.Serializable]

    public class PathDoubleLUT : PathLUT<double>
    {
        #region LUT
        override public void AddValue(double _value) {
            _value += values[^1];
            base.AddValue(_value);
        }

        override public void Clear() {
            base.Clear();
            values.Add(0.0f);
        }
        
        override public int GetIndexAtValue(double _value)
        {
            _value = Math.Clamp(_value, 0.0f, values[^1]);
            return BinarySearch(_value, 0, this.Resolution - 1, GetValueAtIndex);
        }
        
        override public double EvaulateValueAtTime(float _time)
        {
            int index = GetIndexAtTime(_time);
            if (index + 1 >= Resolution) { return values[^1]; }
            double t = Mathf.InverseLerp(GetTimeAtIndex(index), GetTimeAtIndex(index + 1), _time);
            return GetValueAtIndex(index) * (1.0 - t) + GetValueAtIndex(index + 1) * 1.0;
        }

        override public float EvaulateTimeAtValue(double _value) {
            int index = GetIndexAtValue(_value);
            if (index+1 >= Resolution) { return 0.0f; }
            float lerpTime = Mathf.InverseLerp((float)GetValueAtIndex(index), (float)GetValueAtIndex(index+1), (float)_value);
            return Mathf.Lerp(GetTimeAtIndex(index), GetTimeAtIndex(index+1), lerpTime);
        }
        #endregion

        #region Binary Search
        private int BinarySearch(double _value, int _low, int _high, Func<int, double> _LUT) {
            int mid = (_high + _low)/2;

            if (_low == _high) { return _low; }
            else if (_value >= _LUT(mid) && _value < _LUT(mid+1)) { return mid; }
            else if (_value < _LUT(mid)) { return BinarySearch(_value, _low, mid, _LUT); }
            else if (_value >= _LUT(mid+1)) { return BinarySearch(_value, mid+1, _high, _LUT); }

            return -1;
        }

        public double FindLocalMinimum(double _minT, double _maxT, Func<double, double> _timeWeightFunction, double _e = 0.0001) {
            double min = _minT;
            double max = _maxT;
            double mid;
        
            do {
                mid = (max + min) / 2.0f;

                double minWeight = _timeWeightFunction(mid-_e);
                double maxWeight = _timeWeightFunction(mid+_e);

                if (minWeight < maxWeight) { max = mid; }
                else{ min = mid; }

            } while (max - min > _e);

            return mid;
        }

        public double FindLocalMinimum(double _minT, double _maxT, IShapePath _path, Vector3 _position, double _e = 0.0001) {
            return this.FindLocalMinimum(_minT, _maxT, GetSqrDistanceAtTime, _e);

            double GetSqrDistanceAtTime(double _t) => (_path.GetPositionAtTime((float)_t) - _position).sqrMagnitude;
        }
        #endregion
    }
}