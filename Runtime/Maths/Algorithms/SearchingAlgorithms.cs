using System;
using UnityEngine;

namespace MaroonSeal.Maths {

    static public class SearchingAlgorithms
    {
        #region Binary Searching
        static public (int, int) BinarySearch(float _value, int _count, Func<int, float> _lookup) {
            if (_count <= 0) { return (-1, -1); }
            return BinarySearch(_value, 0, _count-1, _lookup); 
        }

        static private (int, int) BinarySearch(float _value, int _low, int _high, Func<int, float> _lookup) {
            int mid = (_high + _low)/2;
            
            if (_low == _high) { return (_low, _high); }
            else if (_value >= _lookup(mid) && _value < _lookup(mid+1)) { return (mid, mid+1); }
            else if (_value < _lookup(mid)) { return BinarySearch(_value, _low, mid, _lookup); }
            else if (_value >= _lookup(mid+1)) { return BinarySearch(_value, mid+1, _high, _lookup); }

            return (-1, -1);
        }

        static public (int, int) BinarySearch(float _value, int _count, Func<int, int> _lookup) {
            if (_count <= 0) { return (-1, -1); }
            return BinarySearch(_value, 0, _count-1, _lookup); 
        }

        static private (int, int) BinarySearch(float _value, int _low, int _high, Func<int, int> _lookup) {
            int mid = (_high + _low)/2;
            
            if (_low == _high) { return (_low, _high); }
            else if (_value >= _lookup(mid) && _value < _lookup(mid+1)) { return (mid, mid+1); }
            else if (_value < _lookup(mid)) { return BinarySearch(_value, _low, mid, _lookup); }
            else if (_value >= _lookup(mid+1)) { return BinarySearch(_value, mid+1, _high, _lookup); }

            return (-1, -1);
        }
        #endregion
    }

}