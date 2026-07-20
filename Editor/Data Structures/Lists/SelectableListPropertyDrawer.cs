using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using MaroonSeal.DataStructures;

namespace MaroonSealEditor.DataStructures {

    [CustomPropertyDrawer(typeof(SelectableList<>))]
    public class SelectableListPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
            EditorGUI.BeginProperty(_position, _label, _property);
            EditorGUI.LabelField(_position, _label);
            
            SerializedProperty indexProperty = _property.FindPropertyRelative("index");
            SerializedProperty tagListProperty = _property.FindPropertyRelative("tagList");
            
            
            string[] optionNames = new string[tagListProperty.arraySize];

            for(int i = 0; i < tagListProperty.arraySize; i++) {
                optionNames[i] = tagListProperty.GetArrayElementAtIndex(i).stringValue;
            }

            indexProperty.intValue = EditorGUI.Popup(_position, indexProperty.intValue, optionNames);

            EditorGUI.EndProperty();
        }
    }
}