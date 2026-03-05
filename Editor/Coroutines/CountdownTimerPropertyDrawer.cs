using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Routines;

namespace MaroonSealEditor.Maths {
    [CustomPropertyDrawer(typeof(CountdownTimer), true)]
    sealed public class CountdownTimerPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            Vector2Field root = new() {
                label = _property.displayName,
                bindingPath = _property.propertyPath
            };
            root.AddToClassList("unity-base-field__aligned");

            FloatField maxTimeField = root.Q<FloatField>("unity-x-input");
            maxTimeField.label = "s";
            maxTimeField.bindingPath = _property.FindPropertyRelative("setTime").propertyPath;

            FloatField currentTimeField = root.Q<FloatField>("unity-y-input");
            currentTimeField.label = "◷";
            currentTimeField.bindingPath = _property.FindPropertyRelative("secondsLeft").propertyPath;
            currentTimeField.SetEnabled(false);

            return root;
        }
    }
}