using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.DataStructures {
    [System.Serializable]
    public class HopperList<TData>
    {
        [SerializeField] private TData[] data;
        [Space]
        [SerializeField] private List<TData> hopper;

        public int SupplyCount { get { return data.Length; } }
        public int HopperCount { get { return hopper.Count; } }

        public bool AutoRefill { get; private set; }
        public bool IsEmpty { get { return hopper.Count <= 0 && !AutoRefill; } }

        #region Constructors
        public HopperList(List<TData> _data) : this(_data, false) { }
        public HopperList(List<TData> _data, bool _autoRefill) : this(_data.ToArray(), _autoRefill) { }

        public HopperList(TData[] _data) : this(_data, false) { }
        public HopperList(TData[] _data,  bool _autoRefill) {
            data = _data;
            AutoRefill = _autoRefill;
            hopper = new List<TData>();
            Fill();
        }
        #endregion

        #region Unloading Items
        public TData UnloadItem() {
            if (hopper.Count <= 0) { return default(TData); }

            TData item = hopper[0];
            hopper.RemoveAt(0);

            CheckRefill();

            return item;
        }

        public List<TData> UnloadItems(int _count) {
            List<TData> itemList = new List<TData>();

            for(int i = 0; i < _count; i++) {
                itemList.Add(UnloadItem());
                if (IsEmpty) { return itemList; }
            }

            return itemList;
        }
        
        public void RemoveHopperItem(TData _item) {
            hopper.Remove(_item);
            CheckRefill();
        }

        public List<TData> Empty() {
            List<TData> itemList = new List<TData>(hopper);
            hopper.Clear();
            return itemList;
        }
        
        public void Clear() {
            hopper.Clear();
        }
        #endregion

        #region Filling Hopper
        public void Fill() {
            List<TData> fillData = new List<TData>(data);
            while (fillData.Count > 0) {
                int index = Random.Range(0, fillData.Count);

                if (!hopper.Contains(fillData[index])) { hopper.Add(fillData[index]); }
                fillData.RemoveAt(index);
            }
        }

        private void CheckRefill() {
            if (hopper.Count > 1 || !AutoRefill) { return; }
            Fill();
        }
        #endregion
    }
}

