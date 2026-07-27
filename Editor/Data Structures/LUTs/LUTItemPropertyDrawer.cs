using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.DataStructures.LUTs;

using MaroonSealEditor.UIElements;


namespace MaroonSealEditor.DataStructures.LUTs {

    [CustomPropertyDrawer(typeof(LUTItem<,>), true)]
    public class LUTItemPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            VisualElement root;
            SerializedProperty keyProperty = _property.FindPropertyRelative("key");
            SerializedProperty valueProperty = _property.FindPropertyRelative("value");

            VisualElement keyField;

            if (valueProperty.propertyType == SerializedPropertyType.Generic)
            {
                PropertyFieldFoldout propertyFoldout = new(keyProperty, "");
                
                foreach(SerializedProperty child in GetImmediateChildren(valueProperty))
                {
                    propertyFoldout.Add(new PropertyField(child));
                }

                keyField = propertyFoldout;
                root = propertyFoldout;
            }
            else
            {
                PropertyRowField propertiesField = new(keyProperty, valueProperty, "  Key", "Value");
                keyField = propertiesField.propertyAField;
                root = propertiesField;
            }

            root.RegisterCallbackOnce<GeometryChangedEvent>((cntx) =>
            {
                ApplyDelayedInput(keyField);
            });

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

        static IEnumerable<SerializedProperty> GetImmediateChildren(SerializedProperty _parent)
        {
            var enumerator = _parent.GetEnumerator();
            int depth = _parent.depth;

            while (enumerator.MoveNext()) {
                if (enumerator.Current is not SerializedProperty child) { continue; }
                if (child == null || child.depth > depth + 1) { continue; }
                yield return child;
            }
        }
    }
}