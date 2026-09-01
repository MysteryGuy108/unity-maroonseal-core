using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.DataStructures {

    [System.Serializable]
    public struct SerializableKeyValuePair<TKey, TValue> {

        [SerializeField] private TKey key;
        readonly public TKey Key { get { return key; } }
        [SerializeField] private TValue value;
        readonly public TValue Value { get { return value; } }

        #region Constructors
        public SerializableKeyValuePair(TKey _key, TValue _value) { 
            key = _key; 
            value = _value; 
        }
        
        public SerializableKeyValuePair((TKey, TValue) _pair) { 
            key = _pair.Item1; 
            value = _pair.Item2; 
        }

        public SerializableKeyValuePair(KeyValuePair<TKey, TValue> _pair) { 
            key = _pair.Key; 
            value = _pair.Value;
        }
        #endregion

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