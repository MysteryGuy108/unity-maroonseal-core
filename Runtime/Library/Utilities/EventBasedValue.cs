using System;
using System.Collections.Generic;

using UnityEngine;


namespace MaroonSeal
{
    [System.Serializable]
    public class EventBasedValue<TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] private TValue current;
        private TValue previous;

        readonly private List<Action<TValue>> listeners;

        public TValue Value => current;
        public TValue Previous => previous;

        #region Constructors
        public EventBasedValue(TValue _value) {
            current = _value;
            previous = _value;
            listeners = new();
        }
        public EventBasedValue() : this(default) {}

        ~EventBasedValue() => listeners?.Clear();
        #endregion

        public void SetValue(TValue _value)
        {
            previous = current;
            current = _value;
            InvokeListeners(current);
        }

        #region Callbacks
        public void AddListener(Action<TValue> _listener)
        {
            if (listeners.Contains(_listener) || _listener == null) { return; }
            listeners.Add(_listener);
        }

        public void RemoveListener(Action<TValue> _listener)
        {
            if (!listeners.Contains(_listener)) { return; }
            listeners.Remove(_listener);
        }

        private void InvokeListeners(TValue _value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].Invoke(_value);
            }

        }
        #endregion

        #region ISerializationCallbackReceiver
        public void OnAfterDeserialize()
        {
            if (listeners == null) { return; }
            if (current.Equals(previous)) { return; }

            previous = current;
            InvokeListeners(current);
        }

        public void OnBeforeSerialize() {}
        #endregion
    }
}

