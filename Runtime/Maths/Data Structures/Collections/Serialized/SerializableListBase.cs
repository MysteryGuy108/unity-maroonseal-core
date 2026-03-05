using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    abstract public class SerializableListBase<T> : IList<T>, ISerializationCallbackReceiver
    {
        private bool m_Initialised;
        [SerializeField] private List<T> list;

        public T this[int _index] { get => list[_index]; set => list[_index] = value; }

        #region IList<>
        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(T _item) { list.Add(_item); }
        public void Insert(int _index, T _item) { list.Insert(_index, _item); }

        public bool Remove(T _item) { return list.Remove(_item); }
        public void RemoveAt(int _index) { list.RemoveAt(_index); }

        public void Clear() { list.Clear(); }

        public bool Contains(T _item) { return list.Contains(_item); }

        public void CopyTo(T[] _array, int _arrayIndex) { list.CopyTo(_array, _arrayIndex); }

        public int IndexOf(T _item) { return list.IndexOf(_item); }


        public IEnumerator<T> GetEnumerator() { return list.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region ISerializationCallbackReciever
        virtual public void OnBeforeSerialize() {}

        virtual public void OnAfterDeserialize() {
            if (m_Initialised) { return; }
            list = new();
            m_Initialised = true;
        }
        #endregion
    }
}
