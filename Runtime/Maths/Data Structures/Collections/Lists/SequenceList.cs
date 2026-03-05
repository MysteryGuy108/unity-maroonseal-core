using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    public class SequenceList<TData>
    {
        public int Index { get; private set; }
        public TData Current { 
            get {
                if (Index < 0) { return default; }
                if (HasReachedEnd) { return dataOrder[dataOrder.Length-1]; }
                return dataOrder[Index];
            } 
        }

        public int Length { get { return dataOrder.Length; } }

        public bool HasReachedEnd{ get { return Index >= dataOrder.Length; } }
        public bool OnLastItem { get { return Index == dataOrder.Length - 1; }}
        
        [SerializeField] private TData[] dataOrder;

        #region Constructors
        public SequenceList() { }
        public SequenceList(List<TData> _dataList) : this(_dataList.ToArray()) { }  
        public SequenceList(List<TData> _dataList, int _startIndex) : this(_dataList.ToArray(), _startIndex) { }  

        public SequenceList(TData[] _dataArray) : this(_dataArray, 0) { }

        public SequenceList(TData[] _dataArray, int _startIndex) {
            dataOrder = _dataArray;
            Begin(_startIndex);
        }

        #endregion

        public void Begin() { SetIndex(0); }

        public void Begin(int _startIndex) { SetIndex(_startIndex); }

        public void Progress() {
            if (HasReachedEnd) { return; }

            int newIndex = Index + 1;
            SetIndex(newIndex);
        }

        private void SetIndex(int _index) {
            Index = _index;
        }
    }
}
