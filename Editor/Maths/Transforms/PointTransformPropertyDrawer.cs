using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;

namespace MaroonSealEditor.Maths {
    [CustomPropertyDrawer(typeof(Transform3D))]
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
            QuaternionEulerField rotationField = new("Rotation");
            rotationField.BindProperty(_property.FindPropertyRelative("rotation"));
            root.Add(rotationField);

            root.Add(new PropertyField(_property.FindPropertyRelative("scale")));

            return root;
        }

        #region Property Conversions
        static public Transform3D FromProperty(SerializedProperty _property) {
            return new Transform3D() {
                position = _property.FindPropertyRelative("position").vector3Value,
                rotation = _property.FindPropertyRelative("rotation").quaternionValue,
                scale = _property.FindPropertyRelative("scale").vector3Value
            };
        }

        static public void SetProperty(SerializedProperty _property, Transform3D _pointTransform) {
            _property.FindPropertyRelative("position").vector3Value = _pointTransform.position;
            _property.FindPropertyRelative("rotation").quaternionValue = _pointTransform.rotation;
            _property.FindPropertyRelative("scale").vector3Value = _pointTransform.scale;
        }
        #endregion
    }
}


