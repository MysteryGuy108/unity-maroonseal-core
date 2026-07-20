using System;
using UnityEngine;

namespace MaroonSeal.DataStructures {
    public interface ISortableLUT {
        public void SortByKeys();
        public void SortKeysDecoupled();
    }

    public interface ISortableValuesLUT : ISortableLUT {
        public void SortByValues();
        public void SortValuesDecoupled();
    }
}