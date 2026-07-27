using System;
using UnityEngine;

namespace MaroonSeal.Maths.Algorithms {

    static public class BinarySearch
    {
        #region Binary Searching
        static public (int, int) Search<TValue>(TValue _value, int _count, Func<int, TValue> _lookup, Func<TValue, TValue, int> _compare) {
            if (_count <= 0) { return (-1, -1); }
            else if (_compare(_value, _lookup(0)) <= 0) { return (0, 0); }
            else if (_compare(_value, _lookup(_count-1)) > 0) { return (_count-1, _count-1); }
            return Search(_value, 0, _count-1, _lookup, _compare);
        }

        static private (int, int) Search<TValue>(TValue _value, int _low, int _high, Func<int, TValue> _lookup, Func<TValue, TValue, int> _compare) {
            
            if (_low == _high) { return (_low, _high); }

            int mid = (_high + _low)/2;
            int midComparison = _compare(_value, _lookup(mid));
            int nextComparison = _compare(_value, _lookup(mid+1));

            if (midComparison >= 0 && nextComparison < 0) { return (mid, mid+1); }
            else if (midComparison < 0) { return Search(_value, _low, mid, _lookup, _compare); }
            else if (nextComparison >= 0) { return Search(_value, mid+1, _high, _lookup, _compare); }

            return (-1, -1);
        }

        static public (int, int) Search(float _value, int _max, Func<int, float> _lookup) =>
            Search(_value, _max, _lookup, (a, b) => a.CompareTo(b));

        static public (int, int) Search(int _value, int _max, Func<int, int> _lookup) =>
            Search(_value, _max, _lookup, (a, b) => a.CompareTo(b));


        #endregion
    }

}