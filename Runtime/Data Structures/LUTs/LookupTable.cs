using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs
{
    [System.Serializable]
    public class LookupTable<TKey, TValue> : LookupTableBase<TKey, TValue>, ISerializationCallbackReceiver
    {
        Dictionary<TKey, TValue> lookup = new();

        #region IDictionary<,>
        public TValue this[TKey key] { get => lookup[key]; set => lookup[key] = value; }

        public bool IsReadOnly => false;

        public override void Add(LUTItem<TKey, TValue> _item)
        {
            lookup.Add(_item.Key, _item.Value);
            SyncListFromDictionary();
        }
        
        public bool Remove(TKey key) {
            bool removed = lookup.Remove(key);
            if (removed) { SyncListFromDictionary(); }
            return removed;
        }

        public override void Clear()
        {
            base.Clear();
            lookup.Clear();
        }

        public bool TryGetValue(TKey key, out TValue value) => lookup.TryGetValue(key, out value);

        public bool ContainsKey(TKey key) => lookup.ContainsKey(key);
        #endregion

        private void SyncListFromDictionary()
        {
            items.Clear();
            foreach(KeyValuePair<TKey, TValue> item in lookup)
            {
                items.Add(new(item.Key, item.Value));
            }
        }

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize() => SyncListFromDictionary();

        public void OnAfterDeserialize()
        {
            Dictionary<TKey, TValue> fallback = new(lookup);
            lookup.Clear();
            lookup = new(Count);

            foreach(LUTItem<TKey, TValue> item in items)
            {
                if (lookup.ContainsKey(item.Key)) {
                    lookup.Clear(); 
                    lookup = fallback;
                    SyncListFromDictionary();
                    Debug.LogError("Lookup Table dictionary cannot have two keys with the same value of " + item.Key.ToString() + ".");
                    return;
                }

                lookup.Add(item.Key, item.Value);
            }

            fallback.Clear();
        }
        #endregion
    }
}
