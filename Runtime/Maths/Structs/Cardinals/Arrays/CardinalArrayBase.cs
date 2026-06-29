using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.Maths
{
    /// <summary>
    /// Base class for an array data structure that can be queried using a cardinal struct. Useful for
    /// storing data about grid cell edges and sprite sheets for different facing directions.
    /// </summary>
    /// <typeparam name="TValue">The type of value that is to be stored in the array.</typeparam>
    /// <typeparam name="TCardinal">The type of cardinal that is used to query the array.</typeparam>
    abstract public class CardinalArrayBase<TValue, TCardinal> : ISerializationCallbackReceiver where TCardinal : ICardinal
    {
        // Number of elements in the array.
        abstract public int Count { get; }
        // The array containing the stored values.
        [SerializeField] protected TValue[] valueArray;

        #region Constructors
        public CardinalArrayBase() => valueArray = new TValue[Count];

        public CardinalArrayBase(TValue _value) : base() {
            for(int i = 0; i < Count; i++) {
                valueArray[i] = _value;
            }
        }

        public CardinalArrayBase(TValue[] _values) : base() {
            for(int i = 0; i < Count; i++) {
                valueArray[i] = _values[i];
            }
        }
        #endregion

        #region Get/Set
        public TValue this[int i] {
            get => valueArray[i];
            set => valueArray[i] = value;
        }

        public TValue this[TCardinal _i] {
            get => this[_i.Index];
            set => this[_i.Index] = value;
        }
        #endregion

        #region Operators
        public static bool operator == (CardinalArrayBase<TValue, TCardinal> _x, CardinalArrayBase<TValue, TCardinal> _y) {
            if (_x.Count != _y.Count) { return false; }

            bool isEqual = true;

            for(int i = 0; i < _x.Count; i++) {
                isEqual = isEqual && _x[i].Equals(_y[i]);
            }
            return isEqual;
        }

        public static bool operator != (CardinalArrayBase<TValue, TCardinal> _x, CardinalArrayBase<TValue, TCardinal> _y) {
            if (_x.Count != _y.Count) { return true; }

            bool isEqual = true;
            
            for(int i = 0; i < _x.Count; i++) {
                isEqual = isEqual && _x[i].Equals(_y[i]);
            }
            return !isEqual;
        }

        public override bool Equals(object obj) {
            if (obj is not CardinalArrayBase<TValue, TCardinal>) { return false; }
            return this == (CardinalArrayBase<TValue, TCardinal>) obj;
        }

        public override int GetHashCode() {
            unchecked 
            {
                HashCode hash = new();
                for(int i = 0; i < valueArray.Length; i++) { hash.Add(valueArray[i]); }
                return hash.ToHashCode();
            }
        }
        #endregion

        #region Conversions
        public TValue[] ToArray() => valueArray.ToArray(); 

        abstract public Cardinal4Array<TValue> ToCardinal4Array();

        abstract public Cardinal8Array<TValue> ToCardinal8Array();
        #endregion

        protected TValue[] GetRotatedValues(int _rotation) {

            TValue[] rotatedValues = new TValue[Count];
            TValue[] edgeArray = ToArray();

            for (int i = 0; i < 4; i++) {
                int index = (int)Mathf.Repeat(i + _rotation, Count);
                
                rotatedValues[index] = edgeArray[i];
            }

            return rotatedValues;
        }

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize()
        {
            if (valueArray.Length != Count) {}
            TValue[] newArray = new TValue[Count];
            valueArray.CopyTo(newArray, 0);
            valueArray = newArray;
        }
        #endregion
    }
}
