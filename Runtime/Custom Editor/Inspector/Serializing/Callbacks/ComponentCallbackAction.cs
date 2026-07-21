using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MaroonSeal.Utilities.Callbacks {
    
    public class ComponentCallbackAction : ComponentCallbackBase{
        public ComponentCallbackAction(Component _target, string _methodName) : base(_target, _methodName) {}

        public override Type GetReturnType() { return typeof(void); }
        public override Type[] GetParametreTypes() { return null; }

        public void Invoke() {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return; }
            method.Invoke(targetComponent, null);
        }

        static public Type[] GetParametreTypes(Type[] _genericFieldTypes) {
            return _genericFieldTypes;
        }
    }

    [System.Serializable]
    public class ComponentCallbackAction<Arg0> : ComponentCallbackBase
    {
        public ComponentCallbackAction(Component _target, string _methodName) : base(_target, _methodName) {}

        public void Invoke(Arg0 _arg0) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return; }
            method.Invoke(targetComponent, new object[] { _arg0 });
        }

        public override Type GetReturnType() { return typeof(void); }
        public override Type[] GetParametreTypes() { return new Type[] { typeof(Arg0) }; }
    }

    [System.Serializable]
    public class ComponentCallbackAction<Arg0, Arg1> : ComponentCallbackBase
    {
        public ComponentCallbackAction(Component _target, string _methodName) : base(_target, _methodName) {}

        public void Invoke(Arg0 _arg0, Arg1 _arg1) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return; }
            method.Invoke(targetComponent, new object[] { _arg0, _arg1 });
        }

        public override Type GetReturnType() { return typeof(void); }
        public override Type[] GetParametreTypes() { return new Type[] { typeof(Arg0), typeof(Arg1) }; }
    }

    [System.Serializable]
    public class ComponentCallbackAction<Arg0, Arg1, Arg2> : ComponentCallbackBase
    {
        public ComponentCallbackAction(Component _target, string _methodName) : base(_target, _methodName) {}

        public void Invoke(Arg0 _arg0, Arg1 _arg1, Arg2 _arg2) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return; }
            method.Invoke(targetComponent, new object[] { _arg0, _arg1, _arg2 });
        }

        public override Type GetReturnType() { return typeof(void); }
        public override Type[] GetParametreTypes() { return new Type[] { typeof(Arg0), typeof(Arg1), typeof(Arg2) }; }
    }

    [System.Serializable]
    public class ComponentCallbackAction<Arg0, Arg1, Arg2, Arg3> : ComponentCallbackBase
    {
        public ComponentCallbackAction(Component _target, string _methodName) : base(_target, _methodName) {}

        public void Invoke(Arg0 _arg0, Arg1 _arg1, Arg2 _arg2, Arg2 _arg3) {
            MethodInfo method = FindCurrentMethod();
            if (method == null) { return; }
            method.Invoke(targetComponent, new object[] { _arg0, _arg1, _arg2, _arg3 });
        }

        public override Type GetReturnType() { return typeof(void); }
        public override Type[] GetParametreTypes() { return new Type[] { typeof(Arg0), typeof(Arg1), typeof(Arg2), typeof(Arg3) }; }
    }
}