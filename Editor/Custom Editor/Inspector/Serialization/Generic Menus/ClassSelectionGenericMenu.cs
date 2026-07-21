using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MaroonSealEditor.Utilities {

#if UNITY_EDITOR
    public class ClassSelectionGenericMenu
    {
        GenericMenu genericMenu;

        public ClassSelectionGenericMenu(Type _baseType, Type _current, GenericMenu.MenuFunction2 _func) {
            genericMenu = new GenericMenu();

            var subclasses = _baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(_baseType));
            foreach(var sub in subclasses) {
                if (sub.IsAbstract) { continue; }
                string path = GetClassMenuString(_baseType, sub);
                bool isCurrent = _current == sub && _current != null;
                AddMenuSelection(genericMenu, path, isCurrent, sub, _func);
            }
        }

        public void ShowAsContext() {
            genericMenu.ShowAsContext();
        }

        protected void AddMenuSelection(GenericMenu _menu, string _path, bool _isCurrent, object _classType, GenericMenu.MenuFunction2 _func) {
            _menu.AddItem(new GUIContent(_path), _isCurrent, _func, _classType);
        }

        protected string GetClassMenuString(Type _baseType, Type _classType) {
            if (!_classType.IsSubclassOf(_baseType)) { return ""; }
            string classPath = _classType.Name;
            var current = _classType.BaseType;

            while(current != null && current != _baseType) {
                classPath = current.Name + "/" + classPath;
                current = current.BaseType;
            }

            return classPath;
        }
    }
#endif
}