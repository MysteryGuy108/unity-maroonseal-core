using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    public class LinearFloatToFloatLUT : FloatToFloatLUT
    {
        #region Value Searching
        public (float, float) SearchValueForKeys(float _value, out float _t) {
            (int, int) indexes = SearchingAlgorithms.BinarySearch(_value, itemList.Count, GetValueAtIndex);
            _t = -1.0f;

            if (indexes.Item1 < 0 || indexes.Item2 < 0) { return (default, default); }

            float low = GetValueAtIndex(indexes.Item1);
            float high = GetValueAtIndex(indexes.Item2);
            _t = Mathf.InverseLerp(low, high, _value);

            return (GetKeyAtIndex(indexes.Item1), GetKeyAtIndex(indexes.Item2));
        }

        public (float, float) SearchValueForKeys(float _value) { return SearchValueForKeys(_value, out float t); }
        public float SearchFlooredValueForKeys(float _value) { return SearchValueForKeys(_value).Item1; }
        public float SearchCeiledValueForKeys(float _value) { return SearchValueForKeys(_value).Item2; }

        public float SearchRoundedValueForKeys(float _value) {
            (float, float) keys = SearchValueForKeys(_value, out float t);
            return t < 0.5f ? keys.Item1 : keys.Item2;
        }
        #endregion

        #region Evaluation
        public float Evaluate(float _key) {
            (float, float) values = SearchKeyForValues(_key, out float t);
            return Mathf.Lerp(values.Item1, values.Item2, t);
        }

        public float InverseEvaluate(float _value) {
            (float, float) keys = SearchValueForKeys(_value, out float t);
            return Mathf.Lerp(keys.Item1, keys.Item2, t);
        }
        #endregion

        #region ISerializableDictionary<,>
        protected override void RefreshItemList()  {
            SortByKeys();
            SortValuesDecoupled();
        }
        #endregion

    }
}