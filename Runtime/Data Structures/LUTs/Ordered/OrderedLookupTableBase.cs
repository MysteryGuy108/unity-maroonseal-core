using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.DataStructures.LUTs
{
    abstract public class OrderedLookupTableBase<TKey, TValue> : LookupTableBase<TKey, TValue>, ISerializationCallbackReceiver
    {
        protected bool isDirty = true;

        public override LUTItem<TKey, TValue> this[int _index] {
            get {
                EnsureSorted();
                return base[_index];
            }
        }

        #region LookupTable<,>
        public override void Add(LUTItem<TKey, TValue> _item) {
            base.Add(_item);
            isDirty = true;
        }

        public override bool Remove(LUTItem<TKey, TValue> _item)
        {
            bool removed = base.Remove(_item);
            isDirty |= removed;
            return removed;
        }

        public override void RemoveAt(int _index)
        {
            base.RemoveAt(_index);
            isDirty = true;
        }
        #endregion

        abstract protected int CompareKeys(TKey _a, TKey _b);

        protected void EnsureSorted()
        {
            if (!isDirty) { return; }
            items.Sort((a, b) => CompareKeys(a.Key, b.Key));
            isDirty = false;
        }

        protected bool CheckSorted()
        {
            bool isSorted = true;
            for(int i = 0; i < Count-1; i++)
            {
                if (CompareKeys(items[i].Key, items[i+1].Key) <= 0) { continue; }
                isSorted = false;
                break;
            }
            return isSorted;
        }

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize() {}
        public void OnAfterDeserialize() {
            isDirty = true;
            EnsureSorted();
        }
        #endregion
    }
}
