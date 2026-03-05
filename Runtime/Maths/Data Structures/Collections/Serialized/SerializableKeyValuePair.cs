using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {

    [System.Serializable]
    public struct SerializableKeyValuePair<TKey, TValue> {

        [SerializeField] private bool keyIsReadonly;
        [SerializeField] private bool valueIsReadonly;
        [SerializeField] private TKey key;
        readonly public TKey Key { get { return key; } }
        [SerializeField] private TValue value;
        readonly public TValue Value { get { return value; } }

        public SerializableKeyValuePair(TKey _key, TValue _value, bool _keyIsReadonly = false, bool _valueIsReadonly = false) { 
            key = _key; 
            value = _value; 
            keyIsReadonly = _keyIsReadonly; 
            valueIsReadonly = _valueIsReadonly; 
        }
        
        public SerializableKeyValuePair((TKey, TValue) _pair, bool _keyIsReadonly = false, bool _valueIsReadonly = false) { 
            key = _pair.Item1; 
            value = _pair.Item2;  
            keyIsReadonly = _keyIsReadonly; 
            valueIsReadonly = _valueIsReadonly;  
        }

        public SerializableKeyValuePair(KeyValuePair<TKey, TValue> _pair, bool _keyIsReadonly = false, bool _valueIsReadonly = false) { 
            key = _pair.Key; 
            value = _pair.Value; 
            keyIsReadonly = _keyIsReadonly; 
            valueIsReadonly = _valueIsReadonly; 
        }

        public static explicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> _other) {
            return new(_other.Key, _other.Value);
        }

        readonly public override bool Equals(object _obj) {

            if (_obj is not SerializableKeyValuePair<TKey, TValue>) { return false; }
            SerializableKeyValuePair<TKey, TValue> item = (SerializableKeyValuePair<TKey, TValue>) _obj;
            return this.Key.Equals(item.Key) && this.Value.Equals(item.Value);
        }

        readonly override public int GetHashCode() {
            unchecked {
                return HashCode.Combine(key.GetHashCode(), value.GetHashCode());
            }
        }
    }
}