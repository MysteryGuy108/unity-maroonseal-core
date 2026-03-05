using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    public class FloatToFloatLUT : FloatToValueLUT<float>, ISortableValuesLUT
    {
        #region Value Normalising
        public void NormaliseValues() { NormaliseValues(itemList); }
        protected void NormaliseValues(List<SerializableKeyValuePair<float, float>> _list) {
            for(int i = 0; i < _list.Count; i++) {
                _list[i] = new(i/(_list.Count-1.0f), _list[i].Value, true);
            }
        }
        #endregion

        #region ISortableKeysLUT
        public void SortByValues() {             
            List<SerializableKeyValuePair<float, float>> temp = itemList;
            itemList.OrderBy(x => x.Value);
            temp.Clear();
        }

        public void SortValuesDecoupled() {             
            List<float> values = new(Values);
            List<float> orderedValues = values.OrderBy(x => x).ToList();
            values.Clear();

            for(int i = 0; i < itemList.Count; i++) {
                itemList[i] = new(itemList[i].Key, orderedValues[i]);
            }
            orderedValues.Clear();
        }
        #endregion

        #region ISerializableDictionary<,>
        protected override void RefreshItemList()  {
            SortByKeys();
        }
        #endregion
    }
}