using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    [System.Serializable]
    public class SelectableList<TData> {
        [SerializeField] private int index;
        public TData Selected { 
            get {
                if (optionsList == null) { return default; }
                if (optionsList.Count <= 0) { return default; }
                return optionsList[index]; 
            } 
        }

        public string SelectedName { get { return tagList[index]; } }

        [SerializeField] private List<TData> optionsList;
        [SerializeField] private List<string> tagList;
        [SerializeField] private Dictionary<string, int> tagToIndexLUT;

        public bool Push(TData _data, string _tag) {
            optionsList ??= new(); tagList ??= new(); tagToIndexLUT ??= new();

            if (tagToIndexLUT.ContainsKey(_tag)) { return false; }

            optionsList.Add(_data);
            tagList.Add(_tag);
            tagToIndexLUT.Add(_tag, optionsList.Count-1);

            return true;
        }

        public void Pop() {
            if (optionsList == null) { return; }
            if (optionsList.Count <= 0) { return; }

            tagToIndexLUT.Remove(tagList[^1]);
            tagList.RemoveAt(tagList.Count-1);
            optionsList.RemoveAt(optionsList.Count-1);
        }

        public void SetSelected(int _index) { index = _index; }

        public void SetSelected(string _tag) { index = tagList.FindIndex(x => x == _tag); }

        public bool Contains(TData _data) { return optionsList.Contains(_data); }

        public bool Contains(string _tag) { return tagList.Contains(_tag); }

        public string[] GetTagList() { return tagList.ToArray(); }

        public void Clear() {
            optionsList?.Clear();
            tagList?.Clear();
            tagToIndexLUT?.Clear();
        }
    }
}