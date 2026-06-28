using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths {
    [CustomPropertyDrawer(typeof(PolarVector2), true)]
    sealed public class PolarVector2PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            Vector2Field root = new() {
                label = _property.displayName,
                bindingPath = _property.propertyPath
            };
            root.AddToClassList("unity-base-field__aligned");

            FloatField input0 = root.Q<FloatField>("unity-x-input");
            input0.label = "R";
            input0.bindingPath = _property.FindPropertyRelative("radius").propertyPath;

            FloatField input1 = root.Q<FloatField>("unity-y-input");
            input1.label = "θ";
            input1.bindingPath = _property.FindPropertyRelative("theta").propertyPath;

            return root;
        }
    }
}
