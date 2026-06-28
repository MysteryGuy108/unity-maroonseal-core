using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths
{
    [CustomPropertyDrawer(typeof(Cardinal4))]
    public class Cardinal4PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            PropertyField root = new(_property.FindPropertyRelative("direction")) {
                label = _property.displayName
            };
            return root;
        }
    }
}