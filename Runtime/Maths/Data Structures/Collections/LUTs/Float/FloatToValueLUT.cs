using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    public class FloatToValueLUT<TValue> : LUTBase<float, TValue>, ISortableLUT
    {
        #region Key To Value Searching
        public (TValue, TValue) SearchKeyForValues(float _key, out float _t) {
            (int, int) indexes = SearchingAlgorithms.BinarySearch(_key, itemList.Count, GetKeyAtIndex);
            _t = -1.0f;

            if (indexes.Item1 < 0 || indexes.Item2 < 0) { return (default, default); }

            float low = GetKeyAtIndex(indexes.Item1);
            float high = GetKeyAtIndex(indexes.Item2);
            _t = Mathf.InverseLerp(low, high, _key);

            return (GetValueAtIndex(indexes.Item1), GetValueAtIndex(indexes.Item2));
        }

        public (TValue, TValue) SearchKeyForValues(float _key) { return SearchKeyForValues(_key, out float t); }
        public TValue SearchFlooredKeyForValue(float _key) { return SearchKeyForValues(_key).Item1; }
        public TValue SearchCeiledKeyForValue(float _key) { return SearchKeyForValues(_key).Item2; }
        public TValue SearchKeysForRoundedValue(float _key) {
            (TValue, TValue) values = SearchKeyForValues(_key, out float t);
            return t < 0.5f ? values.Item1 : values.Item2;
        }
        #endregion

        #region Normalising
        public void NormaliseKeys() { NormaliseKeys(itemList); }
        protected void NormaliseKeys(List<SerializableKeyValuePair<float, TValue>> _list) {
            for(int i = 0; i < _list.Count; i++) {
                _list[i] = new(i/(_list.Count-1.0f), _list[i].Value, true);
            }
        }
        #endregion

        #region ISortableKeysLUT
        public void SortByKeys() {             
            List<SerializableKeyValuePair<float, TValue>> temp = itemList;
            itemList = itemList.OrderBy(x=>x.Key).ToList();
            temp.Clear();
        }

        public void SortKeysDecoupled() {             
            List<float> keys = new(Keys);
            List<float> orderedKeys = keys.OrderBy(x => x).ToList();
            keys.Clear();

            for(int i = 0; i < itemList.Count; i++) {
                itemList[i] = new(orderedKeys[i], itemList[i].Value);
            }
            orderedKeys.Clear();
        }
        #endregion

        #region ISerializableDictionary<,>
        protected override void RefreshItemList()  {
            SortByKeys();
        }
        #endregion
    }
}