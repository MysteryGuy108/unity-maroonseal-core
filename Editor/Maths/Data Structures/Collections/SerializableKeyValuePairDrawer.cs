using UnityEngine;
using UnityEditor;

using MaroonSeal.Maths.DataStructures;

namespace MaroonSealEditor.Maths.DataStructures {
    [CustomPropertyDrawer(typeof(SerializableKeyValuePair<,>), true)]
    sealed public class SerializableKeyValuePairDrawer : PropertyDrawer
    {
        #region Property Drawer
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
            EditorGUI.BeginProperty(_position, _label, _property);
            
            SerializedProperty valueProperty = _property.FindPropertyRelative("value");
            if (valueProperty.propertyType == SerializedPropertyType.Generic) {
                DrawVerticalKeyValue(_position, _property, _label);
            }
            else {
                DrawInLineKeyValue(_position, _property, _label);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) {
            return EditorGUI.GetPropertyHeight(_property.FindPropertyRelative("value"), _label, true);
        }
        #endregion

        #region Value Drawing
        private void DrawInLineKeyValue(Rect _position, SerializedProperty _property, GUIContent _label) {
            Rect headerPosition = _position;
            headerPosition.height = 18.0f;
            headerPosition.width = EditorGUIUtility.labelWidth;

            bool isKeyReadonly = _property.FindPropertyRelative("keyIsReadonly").boolValue;

            EditorGUI.BeginDisabledGroup(isKeyReadonly);
            DrawDelayedKeyField(headerPosition, _property.FindPropertyRelative("key"));
            EditorGUI.EndDisabledGroup();

            Rect contentPosition = _position;
            contentPosition.x += EditorGUIUtility.labelWidth+8;
            contentPosition.width -= EditorGUIUtility.labelWidth+8;

            contentPosition.height = 18.0f;

            bool isValueReadonly = _property.FindPropertyRelative("valueIsReadonly").boolValue;
            SerializedProperty valueProperty = _property.FindPropertyRelative("value");

            EditorGUI.BeginDisabledGroup(isValueReadonly);
            DrawDelayedKeyField(contentPosition, valueProperty);
            EditorGUI.EndDisabledGroup();
        }

        private void DrawVerticalKeyValue(Rect _position, SerializedProperty _property, GUIContent _label) {
            Rect headerPosition = _position;
            headerPosition.x += 16.0f;
            headerPosition.height = 18.0f;
            headerPosition.width = EditorGUIUtility.labelWidth;

            bool isKeyReadonly = _property.FindPropertyRelative("keyIsReadonly").boolValue;
            EditorGUI.BeginDisabledGroup(isKeyReadonly);
            DrawDelayedKeyField(headerPosition, _property.FindPropertyRelative("key"));
            EditorGUI.EndDisabledGroup();

            Rect contentPosition = _position;

            SerializedProperty valueProperty = _property.FindPropertyRelative("value");
            EditorGUI.PropertyField(contentPosition, valueProperty, GUIContent.none, true);
        }
        #endregion

        private void DrawDelayedKeyField(Rect _position, SerializedProperty _keyProperty) {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);

            switch(_keyProperty.propertyType) {
                case SerializedPropertyType.Float:
                    EditorGUI.DelayedFloatField(_position, _keyProperty, GUIContent.none);
                    break;
                case SerializedPropertyType.Integer:
                    EditorGUI.DelayedIntField(_position, _keyProperty, GUIContent.none);
                    break;
                case SerializedPropertyType.String:
                    EditorGUI.DelayedTextField(_position, _keyProperty, GUIContent.none);
                    break;
                default:
                    EditorGUI.PropertyField(_position, _keyProperty, GUIContent.none);
                    break;
            }

            EditorGUI.EndDisabledGroup();
        }
    }  
}