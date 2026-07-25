using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.DataStructures.LUTs;

namespace MaroonSealEditor.DataStructures.LUTs {

    [CustomPropertyDrawer(typeof(LookupTableBase<,>))]
    public abstract class LookupTableBasePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            SerializedProperty itemsListProperty = _property.FindPropertyRelative("items");

            PropertyField itemsField = new(itemsListProperty);
            ListView itemsListView = itemsField.Q<ListView>();

            itemsField.RegisterCallbackOnce<GeometryChangedEvent>(OnGeometryChanged);
            itemsField.label = _property.displayName;
            return itemsField;

            void OnGeometryChanged(GeometryChangedEvent _evnt)
            {
                itemsListView = itemsField.Q<ListView>();
                itemsListView.bindItem += BindItemElement;

                InstantiateListView(itemsListProperty, itemsListView);
            }
        }

        protected abstract void InstantiateListView(SerializedProperty _listProperty, ListView _listView);

        protected virtual void BindItemElement(VisualElement _element, int _index) {}

        static protected void SetUniqueLUTItemValues(SerializedProperty _property, int _index)
        {
            SerializedProperty keyElement = _property.FindPropertyRelative("key");
            switch(keyElement.propertyType)
            {
                case SerializedPropertyType.Integer:
                    keyElement.intValue = _index;
                    break;
                    
                case SerializedPropertyType.Float:
                    keyElement.floatValue = _index;
                    break;

                case SerializedPropertyType.Vector2:
                    keyElement.vector2Value = Vector2.right * _index;
                    break;
                case SerializedPropertyType.Vector2Int:
                    keyElement.vector2IntValue = Vector2Int.right * _index;
                    break;
                case SerializedPropertyType.Vector3:
                    keyElement.vector3Value = Vector3.right * _index;
                    break;
                case SerializedPropertyType.Vector3Int:
                    keyElement.vector3IntValue = Vector3Int.right * _index;
                    break;

                case SerializedPropertyType.String:
                    keyElement.stringValue = "Element " + _index.ToString();
                    break;

                case SerializedPropertyType.Enum:
                    keyElement.enumValueIndex = _index;
                    break;

                case SerializedPropertyType.ObjectReference:
                    keyElement.objectReferenceValue = null;
                    break;

                case SerializedPropertyType.Boolean:
                    keyElement.boolValue = false;
                    break;
            }
        }
    }
}