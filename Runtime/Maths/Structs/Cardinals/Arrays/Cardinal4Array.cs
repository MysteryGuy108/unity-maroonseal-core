using UnityEngine;

namespace MaroonSeal.Maths
{
    [System.Serializable]
    public class Cardinal4Array<TValue> : CardinalArrayBase<TValue, Cardinal4>{
        override public int Count => 4;

        #region Constructors
        public Cardinal4Array() : base() {}

        public Cardinal4Array(TValue _value) : base(_value) {}

        public Cardinal4Array(TValue[] _values) : base(_values) {}

        public Cardinal4Array(TValue _North, TValue _East, TValue _South, TValue _West) : base() {
            valueArray[0] = _North;
            valueArray[1] = _East;
            valueArray[2] = _South;
            valueArray[3] = _West;
        }
        #endregion

        #region Conversions
        override public Cardinal4Array<TValue> ToCardinal4Array() => new(valueArray);

        override public Cardinal8Array<TValue> ToCardinal8Array() => new(valueArray[0], default, valueArray[1], default, valueArray[2], default, valueArray[3], default);
        #endregion

        public Cardinal4Array<TValue> GetRotated(int _index) => new(GetRotatedValues(_index));

        public Cardinal4Array<TValue> GetRotated(Cardinal4 _rotation, bool _isClockwise = true) {
            return GetRotated(_isClockwise ? _rotation.Index : -_rotation.Index);
        }
    }
}
