using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths {
    [CustomPropertyDrawer(typeof(PointTransform))]
    public class PointTransformPropertyDrawer : PropertyDrawer
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
            root.Add(new PropertyField(_property.FindPropertyRelative("eulerAngles")));
            root.Add(new PropertyField(_property.FindPropertyRelative("scale")));

            return root;
        }

        #region Property Conversions
        static public PointTransform FromProperty(SerializedProperty _property) {
            return new PointTransform() {
                position = _property.FindPropertyRelative("position").vector3Value,
                eulerAngles = _property.FindPropertyRelative("eulerAngles").vector3Value,
                scale = _property.FindPropertyRelative("scale").vector3Value
            };
        }

        static public void SetProperty(SerializedProperty _property, PointTransform _pointTransform) {
            _property.FindPropertyRelative("position").vector3Value = _pointTransform.position;
            _property.FindPropertyRelative("eulerAngles").vector3Value = _pointTransform.eulerAngles;
            _property.FindPropertyRelative("scale").vector3Value = _pointTransform.scale;
        }
        #endregion
    }
}


