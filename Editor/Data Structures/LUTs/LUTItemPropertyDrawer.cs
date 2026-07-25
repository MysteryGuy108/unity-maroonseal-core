using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.DataStructures;

namespace MaroonSealEditor.DataStructures.LUTs {

    [CustomPropertyDrawer(typeof(LUTItem<,>), true)]
    public class LUTItemPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            VisualElement root;
            SerializedProperty keyProperty = _property.FindPropertyRelative("key");
            SerializedProperty valueProperty = _property.FindPropertyRelative("value");

            if (valueProperty.propertyType == SerializedPropertyType.Generic)
            {
                root = new();
            }
            else
            {
                PropertyRowField propertiesField = new(keyProperty, valueProperty, "  Key", "Value");

                propertiesField.RegisterCallbackOnce<GeometryChangedEvent>((cntx) =>
                {
                    ApplyDelayedInput(propertiesField.propertyAField);
                    ApplyDelayedInput(propertiesField.propertyBField);
                });

                root = propertiesField;
            }

            return root;
        }

        static void ApplyDelayedInput(VisualElement _root)
        {
            if (_root == null) { return; }
            _root.Query<TextField>().ForEach(f => f.isDelayed = true);
            _root.Query<IntegerField>().ForEach(f => f.isDelayed = true);
            _root.Query<FloatField>().ForEach(f => f.isDelayed = true);
            _root.Query<LongField>().ForEach(f => f.isDelayed = true);
            _root.Query<DoubleField>().ForEach(f => f.isDelayed = true);
        }
    }
}