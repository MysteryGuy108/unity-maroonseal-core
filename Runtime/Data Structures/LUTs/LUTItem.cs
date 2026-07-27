using System;
using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs
{
    [System.Serializable]
    public struct LUTItem<TKey, TValue> : IEquatable<LUTItem<TKey, TValue>>
    {
        [SerializeField] private TKey key;
        readonly public TKey Key => key;
        [SerializeField] private TValue value;
        readonly public TValue Value => value;

        [SerializeField] private bool keyIsReadonly;
        public readonly bool KeyIsReadOnly => keyIsReadonly;

        [SerializeField] private bool valueIsReadonly;
        public readonly bool ValueIsReadOnly => valueIsReadonly;

        #region Constructors
        public LUTItem(TKey _key, TValue _value, bool _keyIsReadonly = false, bool _valueIsReadonly = false) { 
            key = _key; 
            value = _value; 
            keyIsReadonly = _keyIsReadonly; 
            valueIsReadonly = _valueIsReadonly; 
        }
        
        public LUTItem((TKey, TValue) _pair, bool _keyIsReadonly = false, bool _valueIsReadonly = false) { 
            key = _pair.Item1; 
            value = _pair.Item2;  
            keyIsReadonly = _keyIsReadonly; 
            valueIsReadonly = _valueIsReadonly;  
        }

        public LUTItem(LUTItem<TKey, TValue> _pair, bool _keyIsReadonly = false, bool _valueIsReadonly = false) { 
            key = _pair.Key; 
            value = _pair.Value; 
            keyIsReadonly = _keyIsReadonly; 
            valueIsReadonly = _valueIsReadonly; 
        }
        #endregion

        #region Operators
        public readonly bool Equals(LUTItem<TKey, TValue> other) =>
            this.Key.Equals(other.Key) && this.Value.Equals(other.Value);

        readonly public override bool Equals(object _obj) {

            if (_obj is not LUTItem<TKey, TValue>) { return false; }
            LUTItem<TKey, TValue> item = (LUTItem<TKey, TValue>) _obj;
            return this.Equals(item);
        }

        readonly override public int GetHashCode() {
            unchecked {
                return HashCode.Combine(key.GetHashCode(), value.GetHashCode());
            }
        }
        #endregion
    }
}
