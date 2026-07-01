using System;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures
{
    [System.Serializable]
    public class PolarArray<TValue>
    {
        [NonReorderable][SerializeField] private TValue[] valueArray;
        public int Length => valueArray.Length;

        private float DeltaTheta => 360.0f / valueArray.Length;

        #region Constructors
        public PolarArray(int _length) => valueArray = new TValue[_length];

        public PolarArray(int _length, TValue _value) : this(_length) {
            for(int i = 0; i < _length; i++) {
                valueArray[i] = _value;
            }
        }

        public PolarArray(int _length, TValue[] _values) : this(_length) {
            for(int i = 0; i < _length && i < _values.Length; i++) {
                valueArray[i] = _values[i];
            }
        }

        public PolarArray(TValue[] _values) : this(_values.Length, _values) {}
        #endregion

        #region Get/Set
        public TValue this[int _i] {
            get => valueArray[_i];
            set => valueArray[_i] = value;
        }

        public TValue this[float _theta] {
            get => this[(int)(_theta / DeltaTheta)];
            set => this[(int)(_theta / DeltaTheta)] = value;
        }

        public TValue this[ICardinal _cardinal]
        {
            get => this[_cardinal.Theta];
            set => this[_cardinal.Theta] = value;
        }
        #endregion

        #region Operators
        public static bool operator == (PolarArray<TValue> _x, PolarArray<TValue> _y) {
            if (_x.Length != _y.Length) { return false; }

            bool isEqual = true;

            for(int i = 0; i < _x.Length; i++) {
                isEqual = isEqual && _x[i].Equals(_y[i]);
            }
            return isEqual;
        }

        public static bool operator != (PolarArray<TValue> _x, PolarArray<TValue> _y) {
            if (_x.Length == _y.Length) { return false; }

            bool isEqual = true;

            for(int i = 0; i < _x.Length; i++) {
                isEqual = isEqual && _x[i].Equals(_y[i]);
            }
            return !isEqual;
        }

        public override bool Equals(object obj) {
            if (obj is not PolarArray<TValue>) { return false; }
            return this == (PolarArray<TValue>) obj;
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
        #endregion

        protected TValue[] GetRotatedValues(int _rotation) {

            TValue[] rotatedValues = new TValue[Length];
            TValue[] edgeArray = ToArray();

            for (int i = 0; i < 4; i++) {
                int index = (int)Mathf.Repeat(i + _rotation, Length);
                
                rotatedValues[index] = edgeArray[i];
            }

            return rotatedValues;
        }

    }
}
