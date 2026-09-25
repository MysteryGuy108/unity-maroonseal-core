using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Utilities.Serialization;

namespace MaroonSealEditor.UIElements {

    public static class PolymorphicReferenceMenu
    {
        public static void Build(Type _targetType, Type _selectedType, Action<Type> _setType, HashSet<Type> _ignoreTypes)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("None"), _selectedType == null, () => _setType.Invoke(null));

            if (_targetType == null) { menu.ShowAsContext(); return; }

            foreach (Type type in TypeCache.GetTypesDerivedFrom(_targetType))
            {   
                if (CheckIgnore(type, _ignoreTypes)) continue;

                if (type.IsAbstract || type.ContainsGenericParameters || type.IsNestedPrivate) continue;
                Type capturedType = type;

                PolymorphicReferenceMenuItemAttribute menuElement = (PolymorphicReferenceMenuItemAttribute)Attribute.GetCustomAttribute(capturedType,
                    typeof(PolymorphicReferenceMenuItemAttribute));
                
                string menuPath;
                if (menuElement != null)
                {
                    if (menuElement.isHidden) { continue; }
                    menuPath = menuElement.classMenuPath;
                }
                else
                {
                    menuPath = GetClassInheritancePath(type, _targetType);
                }

                // Adding menu item
                menu.AddItem(new GUIContent(menuPath), capturedType == _selectedType,
                    () => _setType.Invoke(capturedType));
            }

            menu.ShowAsContext();
        }

        public static Type GetFieldType(SerializedProperty _property)
        {
            string typenameString = _property.managedReferenceFieldTypename;
            if (string.IsNullOrEmpty(typenameString)) return null;

            // Format is "AssemblyName Namespace.ClassName"
            int splitIndex = typenameString.IndexOf(' ');
            if (splitIndex < 0) return null;

            string assemblyName = typenameString[..splitIndex];
            string className = typenameString[(splitIndex + 1)..];

            var assembly = System.Reflection.Assembly.Load(assemblyName);
            return assembly?.GetType(className);
        }

        public static string GetClassInheritancePath(Type type, Type rootType)
        {
            var chain = new List<string>();
            Type current = type;

            if (rootType.IsInterface)
            {
                // Interfaces never appear in BaseType, so walk up while the
                // current class still implements rootType, and stop once it doesn't.
                while (current != null && rootType.IsAssignableFrom(current))
                {
                    chain.Add(GetCleanTypeName(current));
                    current = current.BaseType;
                }
            }
            else
            {
                while (current != null && current != rootType)
                {
                    chain.Add(GetCleanTypeName(current));
                    current = current.BaseType;
                }
            }

            chain.Reverse();
            return string.Join("/", chain);
        }

        private static bool CheckIgnore(Type _type, HashSet<Type> _ignoreTypes)
        {
            if (_type == null) return true;
            foreach(var ignoreType in _ignoreTypes) {
                if (ignoreType == null) continue; 
                if (_type.IsSubclassOf(ignoreType) || _type == ignoreType) return true; 
            }

            return false;
        }

        private static string GetCleanTypeName(Type type)
        {
            string name = type.Name;
            int backtickIndex = name.IndexOf('`');
            return ObjectNames.NicifyVariableName(backtickIndex >= 0 ? name[..backtickIndex] : name);
        }
    }
}