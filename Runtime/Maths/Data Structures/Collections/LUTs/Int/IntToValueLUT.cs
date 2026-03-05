using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    public class IntToValueLUT<TValue> : LUTBase<int, TValue>, ISortableLUT
    {
        #region ISortableKeysLUT
        public void SortByKeys() { SortKeys(itemList); }

        public void SortKeysDecoupled()
        {
            throw new System.NotImplementedException();
        }

        protected void SortKeys(List<SerializableKeyValuePair<int, TValue>> _list) {
            List<SerializableKeyValuePair<int, TValue>> temp = _list;
            _list = _list.OrderBy(x=>x.Key).ToList();
            temp.Clear();
        }
        #endregion

    }
}