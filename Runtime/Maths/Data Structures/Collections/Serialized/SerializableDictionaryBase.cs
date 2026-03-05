using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    abstract public class SerializableDictionaryBase<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] protected List<SerializableKeyValuePair<TKey, TValue>> itemList = new();
        private Dictionary<TKey, TValue> dictionary = new();

        #region SerializableDictionaryBase
        protected virtual void RefreshItemList() {}
        #endregion

        #region IDictionary<,>
        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;
        public bool IsReadOnly => false;

        public TValue this[TKey key] { get => dictionary[key]; set => dictionary[key] = value; }

        public void Add(TKey _key, TValue _value) {
            dictionary.Add(_key, _value);
            itemList.Add(new(_key, _value));
            RefreshItemList();
        }

        public void Add(KeyValuePair<TKey, TValue> item) { Add(item.Key, item.Value); }

        public void AddRange(List<KeyValuePair<TKey, TValue>> _list) {
            foreach(KeyValuePair<TKey, TValue> item in _list) {
                dictionary.Add(item.Key, item.Value);
                itemList.Add(new(item));
            }
            RefreshItemList();
        }

        public void AddRange(List<SerializableKeyValuePair<TKey, TValue>> _list) {
            foreach(SerializableKeyValuePair<TKey, TValue> item in _list) {
                dictionary.Add(item.Key, item.Value);
                itemList.Add(item);
            }
            RefreshItemList();
        }

        public void AddRange(TKey[] _keys, TValue[] _values) {
            for(int i = 0; i < Mathf.Min(_keys.Length, _values.Length); i++) {
                dictionary.Add(_keys[i], _values[i]);
                itemList.Add(new(_keys[i], _values[i]));
            }
            RefreshItemList();
        }

        public bool Remove(TKey _key) {
            bool wasRemoved = dictionary.Remove(_key);
            itemList.RemoveAll(x => x.Key.Equals(_key));
            if (wasRemoved) { RefreshItemList(); }
            return wasRemoved;
        }
        public bool Remove(KeyValuePair<TKey, TValue> item) { return Remove(item.Key); }

        public void Clear() { dictionary.Clear(); itemList.Clear(); }

        public bool ContainsKey(TKey _key) { return dictionary.ContainsKey(_key); }

        public bool ContainsValue(TValue _value) { return dictionary.ContainsValue(_value); }

        public bool TryGetValue(TKey _key, out TValue _value) {
            return dictionary.TryGetValue(_key, out _value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            if (!dictionary.ContainsKey(item.Key)) { return false;}
            return dictionary[item.Key].Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] _array, int _arrayIndex) {
            _array[_arrayIndex] = (KeyValuePair<TKey, TValue>)itemList[_arrayIndex];
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return dictionary.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region ISerializationCallbackReceiver
        // Save Dictionary<,> to list
        public void OnBeforeSerialize() {
            itemList.Clear();

            foreach(KeyValuePair<TKey, TValue> item in dictionary) {
                itemList.Add(new(item));
            }
            RefreshItemList();
        }

        // Load Dictionary<,> from list
        public void OnAfterDeserialize() {
            Dictionary<TKey, TValue> fallback = new(dictionary);
            dictionary.Clear();

            for(int i = 0; i < itemList.Count; i++) {

                if (dictionary.ContainsKey(itemList[i].Key)) { 
                    dictionary.Clear(); 
                    dictionary = fallback;
                    Debug.LogError("Dictionary cannot contain two keys of the same value");
                    return;
                }

                dictionary.Add(itemList[i].Key, itemList[i].Value);
            }

            fallback.Clear();
        }
        #endregion
    }
}