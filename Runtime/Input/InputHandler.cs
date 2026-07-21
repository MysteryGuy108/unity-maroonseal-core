using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MaroonSeal.Inputs
{
    abstract public class InputHandler<TInputContext> : MonoBehaviour, IEnumerable<IInputActionHandler> 
        where TInputContext : struct
    {
        [Header("Events")]
        [SerializeField] private UnityEvent<TInputContext> OnInputChanged;

        [SerializeField] private TInputContext currentInput;
        public TInputContext CurrentInput => currentInput;

        #region MonoBehaviour
        virtual protected void Awake()
        {
            foreach(IInputActionHandler handler in this) { 
                handler?.Register(); 
                handler.OnInputChanged += RefreshInput;
            }
        }

        virtual protected void OnEnable()
        {
            foreach(IInputActionHandler handler in this)
            {
                handler?.Enable();
            }
        }

        virtual protected void OnDisable()
        {
            foreach(IInputActionHandler handler in this)
            {
                handler?.Disable();
            }
        }
        #endregion

        private void RefreshInput() {
            currentInput = GetCurrentContext();
            OnInputChanged.Invoke(currentInput);
        }

        abstract public TInputContext GetCurrentContext();

        #region IEnumerable
        abstract public IEnumerator<IInputActionHandler> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
