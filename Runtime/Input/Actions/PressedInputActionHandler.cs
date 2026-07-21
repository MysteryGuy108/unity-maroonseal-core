using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaroonSeal.Inputs
{
    [System.Serializable]
    public class PressedInputActionHandler : ValueInputActionHandler<bool>
    {
        public bool IsPressed => Value;

        override protected void OnPerformed(InputAction.CallbackContext _cntx) => SetValue(true);
        override protected void OnCancelled(InputAction.CallbackContext _cntx) => SetValue(false);
    }
}
