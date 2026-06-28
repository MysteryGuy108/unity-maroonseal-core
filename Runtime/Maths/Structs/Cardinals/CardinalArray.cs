using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct CardinalArray<TValue> {
        [SerializeField] private TValue north;
        [SerializeField] private TValue northEast;
        [SerializeField] private TValue east;
        [SerializeField] private TValue southEast;
        [SerializeField] private TValue south;
        [SerializeField] private TValue southWest;
        [SerializeField] private TValue west;
        [SerializeField] private TValue northWest;

        #region Constructors
        public CardinalArray(TValue _value) {
            north = _value;
            northEast = _value;
            east = _value;
            southEast = _value;
            south = _value;
            southWest = _value;
            west = _value;
            northWest = _value;
        }

        public CardinalArray(TValue _north, TValue _northEast, TValue _east, TValue _southEast,
                            TValue _south, TValue _southWest, TValue _west, TValue _northWest) {
            north = _north;
            northEast = _northEast;
            east = _east;
            southEast = _southEast;
            south = _south;
            southWest = _southWest;
            west = _west;
            northWest = _northWest;
        }

        public CardinalArray(TValue[] _values) {
            north = _values[0];
            northEast = _values[1];
            east = _values[2];
            southEast = _values[3];
            south = _values[4];
            southWest = _values[5];
            west = _values[6];
            northWest = _values[7];
        }

        public CardinalArray(CardinalArray<TValue> _other) {
            north = _other[0];
            northEast = _other[1];
            east = _other[2];
            southEast = _other[3];
            south = _other[4];
            southWest = _other[5];
            west = _other[6];
            northWest = _other[7];
        }
        #endregion

        #region Get/Set
        public TValue this[int i] {
            readonly get {
                return i switch {
                    0 => north,
                    1 => northEast,
                    2 => east,
                    3 => southEast,
                    4 => south,
                    5 => southWest,
                    6 => west,
                    7 => northWest,
                    _ => default,
                };
            }

            set {
                switch (i) {
                    case 0: north = value; break;
                    case 1: northEast = value; break;
                    case 2: east = value; break;
                    case 3: southEast = value; break;
                    case 4: south = value; break;
                    case 5: southWest = value; break;
                    case 6: west = value; break;
                    case 7: northWest = value; break;
                }
            }
        }

        public TValue this[Cardinal8 _i] {
            readonly get { return this[_i.Index]; }
            set { this[_i.Index] = value; }
        }

        readonly public TValue[] ToArray() => new TValue[8] { north, northEast, east, southEast, south, southWest, west, northWest }; 
        #endregion

        #region Operators
        public static bool operator == (CardinalArray<TValue> _x, CardinalArray<TValue> _y) {
            return _x.north.Equals(_y.north) &&
                _x.northEast.Equals(_y.northEast) && 
                _x.east.Equals(_y.east) &&
                _x.southEast.Equals(_y.southEast) &&
                _x.south.Equals(_y.south) &&
                _x.southWest.Equals(_y.southWest) &&
                _x.west.Equals(_y.west) &&
                _x.northWest.Equals(_y.northWest);
        }

        public static bool operator != (CardinalArray<TValue> _x, CardinalArray<TValue> _y) {
            return !(_x.north.Equals(_y.north) &&
                _x.northEast.Equals(_y.northEast) && 
                _x.east.Equals(_y.east) &&
                _x.southEast.Equals(_y.southEast) &&
                _x.south.Equals(_y.south) &&
                _x.southWest.Equals(_y.southWest) &&
                _x.west.Equals(_y.west) &&
                _x.northWest.Equals(_y.northWest));
        }

        public override readonly bool Equals(object obj) {
            if (obj is not CardinalArray<TValue>) { return false; }

            CardinalArray<TValue> mys = (CardinalArray<TValue>) obj;
            return this == mys;
        }

        readonly public override int GetHashCode() {
            return System.HashCode.Combine(north, northEast, east, southEast, south, southWest, west, northWest);
        }
        #endregion

        readonly public CardinalArray<TValue> GetRotated(int _rotation) {

            TValue[] rotatedEdges = new TValue[4];
            TValue[] edgeArray = ToArray();

            for (int i = 0; i < 4; i++) {
                int index = (int)Mathf.Repeat(i + _rotation, 8);
                
                rotatedEdges[index] = edgeArray[i];
            }

            return new CardinalArray<TValue>(rotatedEdges);
        }

        readonly public CardinalArray<TValue> GetRotated(Cardinal8 _rotation, bool _isClockwise = true)
        {
            return GetRotated(_isClockwise ? _rotation.Index : -_rotation.Index);
        }
    }
}

