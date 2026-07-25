using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs {
    public abstract class LookupTableBase<TKey, TValue> : IEnumerable<LUTItem<TKey, TValue>>
    {
        [SerializeField] protected List<LUTItem<TKey, TValue>> items = new();

        public virtual LUTItem<TKey, TValue> this[int _index] => items[_index];
        public int Count => items.Count;

        #region Constructors
        public LookupTableBase() => items = new();
        ~LookupTableBase() => items.Clear();
        #endregion

        public virtual IEnumerable<TKey> Keys
        {
            get { foreach(LUTItem<TKey, TValue> item in items) { yield return item.Key; } }
        }

        public virtual IEnumerable<TValue> Values
        {
            get { foreach(LUTItem<TKey, TValue> item in items) { yield return item.Value; } }
        }

        public virtual void Add(LUTItem<TKey, TValue> _item) => items.Add(new LUTItem<TKey, TValue>(_item));
        public void Add(TKey key, TValue value) => this.Add(new LUTItem<TKey, TValue>(key, value));

        public virtual bool Remove(LUTItem<TKey, TValue> _item) => items.Remove(_item);
        public virtual void RemoveAt(int _index) => items.RemoveAt(_index);
        public virtual void Clear() => items.Clear();

        public virtual bool Contains(LUTItem<TKey, TValue> _item) => items.Contains(_item);

        #region IEnumerable<>
        public IEnumerator<LUTItem<TKey, TValue>> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}