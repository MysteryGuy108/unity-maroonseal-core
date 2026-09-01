using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaroonSeal.Inputs
{
    public interface IInputActionHandler
    {
        public event Action OnInputChanged;
        public void Register();
        public void Enable();
        public void Disable();

        static protected InputAction GetInputActionFromPath(InputActionAsset _inputActionsAsset, string _path)
        {
            if (!_inputActionsAsset) { return null; } 
            if (_path == "") { return null; }

            string[] tokenisedPath = _path.Split('/');
            InputActionMap map = _inputActionsAsset.FindActionMap(tokenisedPath[0]);

            if (map == null) { return null; }
            return map.FindAction(tokenisedPath[1]);
        }

        static public List<string> GetAssetActionPaths(InputActionAsset _asset)
        {
            List<string> paths = new();
            foreach(InputActionMap map in _asset.actionMaps)
            {
                foreach(InputAction action in map.actions)
                {
                    paths.Add(map.name + "/" + action.name);
                }
            }
            return paths;
        }
    }

    abstract public class InputActionHandlerBase : IInputActionHandler
    {
        [SerializeField] private InputActionAsset inputAsset;
        [SerializeField] private string actionPath;

        private InputAction action;

        public event Action OnInputChanged;

        #region IInputActionHandler
        public void Register()
        {
            InputAction newAction = IInputActionHandler.GetInputActionFromPath(inputAsset, actionPath);

            if (action != null)
            {
                action.performed -= SetPerformed;
                action.canceled -= SetCancelled;
            }

            action = newAction;

            if (action != null)
            {
                action.performed += SetPerformed;
                action.canceled += SetCancelled;
            }
        }

        public void Enable() => action?.Enable();
        public void Disable() => action?.Disable();
        #endregion

        #region Event Callbacks
        private void SetPerformed(InputAction.CallbackContext _cntx)
        {
            OnPerformed(_cntx);
            OnInputChanged?.Invoke();
        }

        private void SetCancelled(InputAction.CallbackContext _cntx)
        {
            OnCancelled(_cntx);
            OnInputChanged?.Invoke();
        }

        abstract protected void OnPerformed(InputAction.CallbackContext _cntx);
        abstract protected void OnCancelled(InputAction.CallbackContext _cntx);
        #endregion
    }
}
