using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {
    abstract public class LUTBase<TKey, TValue> : SerializableDictionaryBase<TKey, TValue>
    {
        public KeyValuePair<TKey, TValue> this[int _index] { get => (KeyValuePair<TKey, TValue>)itemList[_index]; }

        public TKey GetKeyAtIndex(int _index) { return itemList[_index].Key; }
        public TValue GetValueAtIndex(int _index) { return itemList[_index].Value; }
    }
}