using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths {
    [CustomPropertyDrawer(typeof(Transform2D))]
    public class Transform2DPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            Foldout root = new() {
                text = _property.displayName,
                bindingPath = _property.propertyPath
            };
            root.AddToClassList("unity-foldout");
            root.AddToClassList("unity-list-view__foldout-header");
            root.AddToClassList("unity-foldout-input");

            root.Add(new PropertyField(_property.FindPropertyRelative("position")));

            AngleField angleField = new();
            angleField.BindProperty(_property.FindPropertyRelative("angle"));
            root.Add(angleField);

            root.Add(new PropertyField(_property.FindPropertyRelative("scale")));

            return root;
        }

        #region Property Conversions
        static public Transform2D FromProperty(SerializedProperty _property) {
            return new Transform2D() {
                position = _property.FindPropertyRelative("position").vector2Value,
                angle = _property.FindPropertyRelative("angle").floatValue,
                scale = _property.FindPropertyRelative("scale").vector2Value
            };
        }

        static public void SetProperty(SerializedProperty _property, Transform2D _transform) {
            _property.FindPropertyRelative("position").vector2Value = _transform.position;
            _property.FindPropertyRelative("angle").floatValue = _transform.angle;
            _property.FindPropertyRelative("scale").vector2Value = _transform.scale;
        }
        #endregion
    }
}