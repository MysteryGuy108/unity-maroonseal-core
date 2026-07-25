using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace MaroonSealEditor {
    public class PropertyRowField : VisualElement
    {

        public VisualElement propertyAField;
        public VisualElement propertyBField;

        public PropertyRowField(SerializedProperty _propertyA, SerializedProperty _propertyB, string labelA = null, string labelB = null)
        {
            style.flexDirection = FlexDirection.Row;

            propertyAField = CreateSubField(_propertyA, labelA);
            propertyBField = CreateSubField(_propertyB, labelB);

            Add(propertyAField);
            Add(propertyBField);
        }

        static VisualElement CreateSubField(SerializedProperty prop, string label)
        {
            string labelText = label ?? prop.displayName;
            var field = new PropertyField(prop, labelText);

            field.style.flexGrow = 1;
            field.style.flexBasis = 0;
            field.style.marginRight = 2;

            field.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

            return field;

            void OnGeometryChanged(GeometryChangedEvent evt)
            {
                // Strip the global inspector alignment
                field.Query<VisualElement>(className: BaseField<int>.alignedFieldUssClassName)
                    .ForEach(el => el.RemoveFromClassList(BaseField<int>.alignedFieldUssClassName));

                // Size the label to fit its own text.
                Label innerLabel = field.Q<Label>(className: "unity-base-field__label");
                if (innerLabel != null)
                {
                    float measured = EditorStyles.label.CalcSize(new GUIContent(labelText)).x;
                    innerLabel.style.width = measured + 4; // small buffer
                    innerLabel.style.minWidth = measured + 4; // clear any USS min-width
                    innerLabel.style.flexShrink = 0;
                    innerLabel.style.flexGrow = 0;
                }

                // Only needs to run once per field
                field.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            }
        }
    }
}