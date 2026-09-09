using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry;
using MaroonSeal.Maths.Geometry.Paths;


namespace MaroonSealEditor.GeometryPaths {
    [CustomPropertyDrawer(typeof(SplinePoint), true)]
    sealed public class SplinePointPropertyDrawer : PropertyDrawer
    {
        #region UI Toolkit
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            
            VisualElement root = new();
            
            Foldout foldout = new() {
                text = _property.displayName,
                bindingPath = _property.propertyPath
            };
            
            // Position Field
            PropertyField positionField = new(_property.FindPropertyRelative("position"));
            positionField.AddToClassList("unity-base-field__aligned");
            foldout.Add(positionField);

            // Roll Field
            PropertyField rollField = new(_property.FindPropertyRelative("roll"));
            rollField.AddToClassList("unity-base-field__aligned");
            foldout.Add(rollField);

            // Size Field
            PropertyField sizeField = new(_property.FindPropertyRelative("size"));
            sizeField.AddToClassList("unity-base-field__aligned");
            foldout.Add(sizeField);

            bool hasPrevious = _property.FindPropertyRelative("hasPrevious").boolValue;
            bool hasNext = _property.FindPropertyRelative("hasNext").boolValue;

            // Tangent Mode Field
            PropertyField tangentModeField = new(_property.FindPropertyRelative("tangentMode"));
            foldout.Add(tangentModeField);

            // TangentIn Field
            SerializedProperty tangentInProperty = _property.FindPropertyRelative("tangentIn");
            Vector3Field tangentInField = new(){ label = "Tangent In" };
            tangentInField.AddToClassList("unity-base-field__aligned");
            tangentInField.BindProperty(tangentInProperty);

            // TangentOut Field
            SerializedProperty tangentOutProperty = _property.FindPropertyRelative("tangentOut");
            Vector3Field tangentOutField = new(){ label = "Tangent Out" };
            tangentOutField.AddToClassList("unity-base-field__aligned");
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
                tangentInField.enabledSelf = _property.FindPropertyRelative("hasPrevious").boolValue;
                tangentOutField.enabledSelf = _property.FindPropertyRelative("hasNext").boolValue;
            }

            void RefreshTangentMode(SerializedPropertyChangeEvent _changeEvent) =>
                ConstrainTangentProperties(_property, tangentOutField, tangentInField);

            void RefreshTangentIn(ChangeEvent<Vector3> _changeEvent) {
                if (_changeEvent.newValue == _changeEvent.previousValue) { return; }
                ConstrainTangentProperties(_property, tangentOutField, tangentInField);
            }

            void RefreshTangentOut(ChangeEvent<Vector3> _changeEvent) {
                if (_changeEvent.newValue == _changeEvent.previousValue) { return; }
                ConstrainTangentProperties(_property, tangentInField, tangentOutField);
            }
            #endregion
        }

        private void ConstrainTangentProperties(SerializedProperty _splinePointProperty, Vector3Field _current, Vector3Field _target) {
            SerializedProperty controlModeProperty = _splinePointProperty.FindPropertyRelative("tangentMode");
            SplinePoint.TangentMode tangentMode = (SplinePoint.TangentMode)controlModeProperty.enumValueIndex;

            _current.value = SplinePoint.ConstrainTangent(tangentMode, _current.value, _target.value);
        }
        #endregion
    }
}