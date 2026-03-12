using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths {
    [CustomPropertyDrawer(typeof(PointTransform2D))]
    public class PointTransform2DPropertyDrawer : PropertyDrawer
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
            root.Add(new PropertyField(_property.FindPropertyRelative("angle")));
            root.Add(new PropertyField(_property.FindPropertyRelative("scale")));

            return root;
        }

        #region Property Conversions
        static public PointTransform2D FromProperty(SerializedProperty _property) {
            return new PointTransform2D() {
                position = _property.FindPropertyRelative("position").vector2Value,
                angle = _property.FindPropertyRelative("angle").floatValue,
                scale = _property.FindPropertyRelative("scale").vector2Value
            };
        }

        static public void SetProperty(SerializedProperty _property, PointTransform2D _pointTransform) {
            _property.FindPropertyRelative("position").vector2Value = _pointTransform.position;
            _property.FindPropertyRelative("angle").floatValue = _pointTransform.angle;
            _property.FindPropertyRelative("scale").vector2Value = _pointTransform.scale;
        }
        #endregion
    }
}