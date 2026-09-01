using System;
using System.Collections.Generic;
using MaroonSeal.Maths.Algorithms;
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
            set
            {
                base[_index] = value;
                isDirty = true;
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

        #region Sorting
        public void EnsureSorted()
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
        #endregion

        #region Evaluating
        public (TValue, TValue) EvaluateKeyNeighbours(TKey _key) {
            (int, int) indices = SearchIndices(_key, (i) => this[i].Key, CompareKeys);
            return (this[indices.Item1].Value, this[indices.Item2].Value);
        }
        public TValue EvaluateKeyFloor(TKey _key) => EvaluateKeyNeighbours(_key).Item1;
        public TValue EvaluateKeyCeiling(TKey _key) => EvaluateKeyNeighbours(_key).Item2;

        protected (int, int) SearchIndices<TSearch>(TSearch _search, Func<int, TSearch> _indexToSearch, Func<TSearch, TSearch, int> _compare)
        {
            EnsureSorted();
            if (this.Count == 0) { throw new InvalidOperationException("Interpolated table has no points."); }
            if (this.Count == 1) { return (0, 0); }

            return BinarySearch.Search(_search, this.Count, _indexToSearch, _compare);
        }
        #endregion

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize() {}
        public void OnAfterDeserialize() {
            isDirty = true;
            EnsureSorted();
        }
        #endregion
    }
}
