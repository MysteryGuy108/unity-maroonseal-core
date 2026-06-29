using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    sealed public class Cardinal8Array<TValue> : CardinalArrayBase<TValue, Cardinal8>{
        override public int Count => 8;

        #region Constructors
        public Cardinal8Array() : base() {}

        public Cardinal8Array(TValue _value) : base(_value) {}

        public Cardinal8Array(TValue[] _values) : base(_values) {}

        public Cardinal8Array(TValue _N, TValue _NE, TValue _E, TValue _SE,
                            TValue _S, TValue _SW, TValue _W, TValue _NW) : base() {
            valueArray[0] = _N;
            valueArray[1] = _NE;
            valueArray[2] = _E;
            valueArray[3] = _SE;
            valueArray[4] = _S;
            valueArray[5] = _SW;
            valueArray[6] = _W;
            valueArray[7] = _NW;
        }
        #endregion

        #region Conversions
        override public Cardinal4Array<TValue> ToCardinal4Array() => new(valueArray[0], valueArray[2], valueArray[4], valueArray[6]);

        override public Cardinal8Array<TValue> ToCardinal8Array() => new(valueArray);
        #endregion

        public Cardinal8Array<TValue> GetRotated(int _index) => new(GetRotatedValues(_index));

        public Cardinal8Array<TValue> GetRotated(Cardinal8 _rotation, bool _isClockwise = true) {
            return GetRotated(_isClockwise ? _rotation.Index : -_rotation.Index);
        }
    }
}

