using System;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using MaroonSeal.Utilities.Serialization;

namespace MaroonSealEditor.Utilities.Serialization {

    [CustomPropertyDrawer(typeof(PolymorphicReferenceAttribute))]
    public class PolymorphicReferenceAttributeDrawer : PropertyDrawer
    {
        SerializedProperty activeProperty;

        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {

            if (_property.propertyType != SerializedPropertyType.ManagedReference) {
                Debug.LogError("PolymorphicReference must be called on a Managed Reference type");
                return;
            }

            PolymorphicReferenceAttribute polymorphicReference = attribute as PolymorphicReferenceAttribute;
            int currentIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.BeginProperty(_position, _label, _property);

            Rect headerPosition = _position;
            headerPosition.width -= 15.0f; headerPosition.height = 18.0f; 
            headerPosition.x += 15.0f;

            headerPosition = EditorGUI.PrefixLabel(headerPosition, _label);
            

            string title = "Select Class";

            if (_property.managedReferenceValue != null) {
                if (!_property.managedReferenceValue.GetType().IsAbstract) { title = _property.managedReferenceValue.GetType().Name; }
            }
           
            if (EditorGUI.DropdownButton(headerPosition, new GUIContent(title), FocusType.Keyboard)) {
                activeProperty = _property;

                ClassSelectionGenericMenu selectionMenu = new(fieldInfo.FieldType, _property.managedReferenceValue?.GetType(), ClickHandler);
                selectionMenu.ShowAsContext();
            }
            
            EditorGUI.PropertyField(_position, _property, GUIContent.none, true);

            EditorGUI.EndProperty();
            EditorGUI.indentLevel = currentIndent;
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) {
            return EditorGUI.GetPropertyHeight(_property);
        }

        private void ClickHandler(object _returnType) {
            Type newType = (Type)_returnType;
            activeProperty.managedReferenceValue = FormatterServices.GetUninitializedObject(newType);
            activeProperty.serializedObject.ApplyModifiedProperties();
            activeProperty = null;
        }
    }
}