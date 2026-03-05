using UnityEngine;
using UnityEditor;

using MaroonSeal.Maths.DataStructures;

namespace MaroonSealEditor.Maths.DataStructures {
    [CustomPropertyDrawer(typeof(SerializableDictionaryBase<,>), true)]
    sealed public class SerializableDictionaryBaseDrawer : PropertyDrawer
    {
        #region Property Drawer
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
            EditorGUI.BeginProperty(_position, _label, _property);

            SerializedProperty itemListProperty = _property.FindPropertyRelative("itemList");

            int startArraySize = itemListProperty.arraySize;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(_position, itemListProperty, _label);

            if (EditorGUI.EndChangeCheck() && itemListProperty.arraySize != startArraySize && itemListProperty.arraySize > 0) {
               ValidateNewItem(itemListProperty);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) {
            SerializedProperty itemListProperty = _property.FindPropertyRelative("itemList");
            return EditorGUI.GetPropertyHeight(itemListProperty);
        }
        #endregion

        #region New Item
        private void ValidateNewItem(SerializedProperty _listProperty) {
            SerializedProperty lastKey = _listProperty.GetArrayElementAtIndex(_listProperty.arraySize-1).FindPropertyRelative("key");
            SetNewItemValidKey(lastKey, _listProperty.arraySize);
        }

        private void SetNewItemValidKey(SerializedProperty _keyProperty, int _itemCount) {
            switch(_keyProperty.propertyType) {
                case SerializedPropertyType.Integer: _keyProperty.intValue = _itemCount; return;
                case SerializedPropertyType.Float: _keyProperty.floatValue = _itemCount; return;
                case SerializedPropertyType.String: _keyProperty.stringValue = "item_" + _itemCount.ToString();  return;
            }
        }
        #endregion
    }  
}