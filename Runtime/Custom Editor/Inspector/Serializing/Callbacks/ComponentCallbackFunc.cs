using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MaroonSeal.Utilities.Callbacks {

    abstract public class ComponentCallbackFunc : ComponentCallbackBase {
        public ComponentCallbackFunc(Component _target, string _methodName) : base(_target, _methodName) {}

        static public Type GetReturnType(Type[] _genericFieldTypes) {
            if (_genericFieldTypes == null) { return null; }
            if (_genericFieldTypes.Length <= 0) { return null; }
            return _genericFieldTypes[0];
        }

        static public Type[] GetParametreTypes(Type[] _genericFieldTypes) {
            if (_genericFieldTypes == null) { return null; }
            if (_genericFieldTypes.Length <= 1) { return null; }
            return _genericFieldTypes[2..];
        }
    }

    [System.Serializable]
    public class ComponentCallbackFunc<TResult> : ComponentCallbackFunc
    {
        public ComponentCallbackFunc(Component _target, string _methodName) : base(_target, _methodName) {}
        public TResult Invoke() {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return default;}
            return (TResult)method.Invoke(targetComponent, null);
        }

        public override Type GetReturnType() { return typeof(TResult); }
        public override Type[] GetParametreTypes() { return null; }
    }

    [System.Serializable]
    public class ComponentCallbackFunc<TResult, Arg0> : ComponentCallbackFunc
    {
        public ComponentCallbackFunc(Component _target, string _methodName) : base(_target, _methodName) {}

        public TResult Invoke(Arg0 _arg0) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return default;}
            return (TResult)method.Invoke(targetComponent, new object[] { _arg0 });
        }

        public override Type GetReturnType() { return typeof(TResult); }
        public override Type[] GetParametreTypes() { return new[] { typeof(Arg0) }; }
    }

    [System.Serializable]
    public class ComponentCallbackFunc<TResult, Arg0, Arg1> : ComponentCallbackFunc
    {
        public ComponentCallbackFunc(Component _target, string _methodName) : base(_target, _methodName) {}

        public TResult Invoke(Arg0 _arg0, Arg1 _arg1) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return default;}
            return (TResult)method.Invoke(targetComponent, new object[] { _arg0, _arg1 });
        }

        public override Type GetReturnType() { return typeof(TResult); }
        public override Type[] GetParametreTypes() { return new[] { typeof(Arg0), typeof(Arg1) }; }
    }

    [System.Serializable]
    public class ComponentCallbackFunc<TResult, Arg0, Arg1, Arg2> : ComponentCallbackFunc
    {
        public ComponentCallbackFunc(Component _target, string _methodName) : base(_target, _methodName) {}

        public TResult Invoke(Arg0 _arg0, Arg1 _arg1, Arg2 _arg2) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return default;}
            return (TResult)method.Invoke(targetComponent, new object[] { _arg0, _arg1, _arg2 });
        }

        public override Type GetReturnType() { return typeof(TResult); }
        public override Type[] GetParametreTypes() { return new[] { typeof(Arg0), typeof(Arg1), typeof(Arg2) }; }
    }

    [System.Serializable]
    public class ComponentCallbackFunc<TResult, Arg0, Arg1, Arg2, Arg3> : ComponentCallbackFunc
    {
        public ComponentCallbackFunc(Component _target, string _methodName) : base(_target, _methodName) {}

        public TResult Invoke(Arg0 _arg0, Arg1 _arg1, Arg2 _arg2, Arg3 _arg3) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return default;}
            return (TResult)method.Invoke(targetComponent, new object[] { _arg0, _arg1, _arg2, _arg3 });
        }

        public override Type GetReturnType() { return typeof(TResult); }
        public override Type[] GetParametreTypes() { return new[] { typeof(Arg0), typeof(Arg1), typeof(Arg2), typeof(Arg3) }; }
    }
}