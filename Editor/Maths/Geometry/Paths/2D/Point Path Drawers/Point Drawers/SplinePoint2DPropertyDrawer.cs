using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry;
using MaroonSeal.Maths.Geometry.Paths;


namespace MaroonSealEditor.Maths.Geometry.Paths {
    [CustomPropertyDrawer(typeof(SplinePoint2D), true)]
    sealed public class SplinePoint2DPropertyDrawer : PropertyDrawer
    {
        #region UI Toolkit
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            
            VisualElement root = new();
            
            Foldout foldout = new() {
                text = _property.displayName,
                bindingPath = _property.propertyPath
            };
            
            // Anchor Field
            PropertyField anchorField = new(_property.FindPropertyRelative("anchor"));
            anchorField.AddToClassList(BaseField<Vector2>.alignedFieldUssClassName);
            foldout.Add(anchorField);

            // Size Field
            PropertyField sizeField = new(_property.FindPropertyRelative("size"));
            sizeField.AddToClassList(BaseField<float>.alignedFieldUssClassName);
            foldout.Add(sizeField);

            bool hasTangentIn = _property.FindPropertyRelative("hasTangentIn").boolValue;
            bool hasTangentOut = _property.FindPropertyRelative("hasTangentOut").boolValue;

            // Tangent Mode Field
            PropertyField tangentModeField = new(_property.FindPropertyRelative("tangentMode"));
            foldout.Add(tangentModeField);

            // TangentIn Field
            SerializedProperty tangentInProperty = _property.FindPropertyRelative("tangentIn");
            Vector2Field tangentInField = new(){ label = "Tangent In" };
            tangentInField.AddToClassList(BaseField<Vector2>.alignedFieldUssClassName);
            tangentInField.BindProperty(tangentInProperty);

            // TangentOut Field
            SerializedProperty tangentOutProperty = _property.FindPropertyRelative("tangentOut");
            Vector2Field tangentOutField = new(){ label = "Tangent Out" };
            tangentOutField.AddToClassList(BaseField<Vector2>.alignedFieldUssClassName);
            tangentOutField.BindProperty(tangentOutProperty);

            // Callbacks
            tangentModeField.RegisterValueChangeCallback(RefreshTangentMode);
            tangentInField.RegisterValueChangedCallback(RefreshTangentIn);
            tangentOutField.RegisterValueChangedCallback(RefreshTangentOut);

            foldout.Add(tangentInField);
            foldout.Add(tangentOutField);

            // Root Property Tracking
            root.TrackPropertyValue(_property, RefreshPropertyGUI);
            RefreshPropertyGUI(_property);

            
            root.Add(foldout);
            return root;

            #region GUI Refresh
            void RefreshPropertyGUI(SerializedProperty _splinePointProperty) {
                tangentInField.enabledSelf = _property.FindPropertyRelative("hasTangentIn").boolValue;
                tangentOutField.enabledSelf = _property.FindPropertyRelative("hasTangentOut").boolValue;
            }

            void RefreshTangentMode(SerializedPropertyChangeEvent _changeEvent) =>
                ConstrainTangentProperties(_property, tangentOutField, tangentInField);

            void RefreshTangentIn(ChangeEvent<Vector2> _changeEvent) {
                if (_changeEvent.newValue == _changeEvent.previousValue) { return; }
                ConstrainTangentProperties(_property, tangentOutField, tangentInField);
            }

            void RefreshTangentOut(ChangeEvent<Vector2> _changeEvent) {
                if (_changeEvent.newValue == _changeEvent.previousValue) { return; }
                ConstrainTangentProperties(_property, tangentInField, tangentOutField);
            }
            #endregion
        }

        private void ConstrainTangentProperties(SerializedProperty _splinePointProperty, Vector2Field _current, Vector2Field _target) {
            SerializedProperty controlModeProperty = _splinePointProperty.FindPropertyRelative("tangentMode");
            SplinePoint.TangentMode tangentMode = (SplinePoint.TangentMode)controlModeProperty.enumValueIndex;

            _current.value = SplinePoint.ConstrainTangent(tangentMode, _current.value, _target.value);
        }
        #endregion
    }
}