using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths.LUTs
{
    [System.Serializable]

    abstract public class PathLUT<TValue> : IEnumerable
    {
        [SerializeField] protected List<TValue> values = new();
        public int Resolution => values.Count;
        public TValue this[System.Index _index] { get { return values[_index]; } }

        virtual public void AddValue(TValue _value)
        {
            values.Add(_value);
        }

        virtual public void Clear()
        {
            values ??= new(); values.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();

        #region Indexes
        public float GetTimeAtIndex(int _index) { return _index / (values.Count - 1.0f); }
        public TValue GetValueAtIndex(int _index) => values[_index];

        public int GetIndexAtTime(float _time)
        {
            _time *= this.Resolution;
            return (int)Mathf.Clamp(_time, 0, this.Resolution - 1);
        }

        abstract public int GetIndexAtValue(TValue _value);
        #endregion

        #region Evaluations
        abstract public TValue EvaulateValueAtTime(float _time);

        abstract public float EvaulateTimeAtValue(TValue _value);
        #endregion
    }
}