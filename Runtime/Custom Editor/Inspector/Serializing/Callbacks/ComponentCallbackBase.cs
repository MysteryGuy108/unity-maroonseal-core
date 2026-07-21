using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace MaroonSeal.Utilities.Callbacks {
    [System.Serializable]
    abstract public class ComponentCallbackBase
    {
        public static BindingFlags callbackBindingFlags = BindingFlags.Public | BindingFlags.Instance; 
        [SerializeField] protected GameObject parentObject;
        [SerializeField] protected Component targetComponent;
        [SerializeField] protected string methodName;

        public ComponentCallbackBase(Component _target, string _methodName) {
            targetComponent = _target;
            methodName = _methodName;
        }

        protected MethodInfo FindCurrentMethod() {
            if (targetComponent == null) { return null;}
            return targetComponent.GetType().GetMethod(methodName);
        }

        abstract public Type GetReturnType();
        abstract public Type[] GetParametreTypes();
        
        public List<string> GetListOfValidMethodNames() {
            if (targetComponent == null) { return null; }
            Type returnType = GetReturnType();
            Type[] parametreTypes = GetParametreTypes();

            List<string> validMethodNames = new();

            foreach (MethodInfo method in targetComponent.GetType().GetMethods(callbackBindingFlags)) {
                if (!IsMethodValid(method, returnType, parametreTypes)) { continue; }
                validMethodNames.Add(method.Name);
            }
            return validMethodNames;
        }

        static public bool IsMethodValid(MethodInfo _method, Type _return, Type[] _parametres) {
            if (_method.ReturnType != _return) { return false; }
            ParameterInfo[] methodParameters = _method.GetParameters();

            if (methodParameters == null) { return _parametres.Length <= 0; }
            if (_parametres == null) { return methodParameters.Length <= 0; }
            
            if (methodParameters.Length != _parametres.Length) { return false; }
            
            for(int i = 0; i < methodParameters.Length; i++) {
                if (methodParameters[i].ParameterType != _parametres[i]) { return false; }
            }

            return true;
        }
    }
}