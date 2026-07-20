
using UnityEngine;

namespace MaroonSeal.DataStructures {
    public class IntToIntLUT : IntToValueLUT<int>, ISortableValuesLUT
    {
        #region ISortableLUT
        public void SortByValues() {
            throw new System.NotImplementedException();
        }

        public void SortValuesDecoupled()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}