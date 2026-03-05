using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MaroonSeal.Inputs
{
    [System.Serializable]
    public class ValueInputActionHandler<TValue> : InputActionHandlerBase where TValue : struct
    {
        [EditorReadonly][SerializeField] private TValue value;
        public TValue Value => value;
        public event Action<TValue> OnValueChanged;

        override protected void OnPerformed(InputAction.CallbackContext _cntx) => SetValue(_cntx.ReadValue<TValue>());
        override protected void OnCancelled(InputAction.CallbackContext _cntx) => SetValue(default);

        protected void SetValue(TValue _value)
        {
            value = _value;
            OnValueChanged?.Invoke(_value);
        }
    }
}
