using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths
{
    [CustomPropertyDrawer(typeof(Cardinal8))]
    public class Cardinal8PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            PropertyField root = new(_property.FindPropertyRelative("direction")) {
                label = _property.displayName
            };
            return root;
        }
    }
}