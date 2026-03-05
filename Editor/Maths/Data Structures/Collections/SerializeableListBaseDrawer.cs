using UnityEngine;
using UnityEditor;

using MaroonSeal.Maths.DataStructures;

namespace MaroonSealEditor.Maths.DataStructures {
    [CustomPropertyDrawer(typeof(SerializableListBase<>), true)]
    sealed public class SerializableListBaseDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
            //if (!_property.GetType().GenericParameterAttributes[0].IsSerializable) { return; }
            EditorGUI.BeginProperty(_position, _label, _property);
            EditorGUI.PropertyField(_position, _property.FindPropertyRelative("list"), _label, true);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) {
            SerializedProperty itemListProperty = _property.FindPropertyRelative("list");
            return EditorGUI.GetPropertyHeight(itemListProperty);
        }
    }   
}