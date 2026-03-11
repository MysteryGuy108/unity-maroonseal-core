using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths.LUTs;

namespace MaroonSealEditor.Maths.Geometry.Paths.LUTs {
    //[CustomPropertyDrawer(typeof(PathFloatLUT))]
    sealed public class PathFloatLUTPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            Foldout root = new() {
                text = _property.displayName,
                bindingPath = _property.propertyPath
            };
            root.AddToClassList("unity-foldout");
            root.AddToClassList("unity-list-view__foldout-header");
            root.AddToClassList("unity-foldout-input");

            IntegerField resolutionField = new() { label = "Resolution", enabledSelf = false };
            resolutionField.AddToClassList("unity-base-field__aligned");
            root.Add(resolutionField);

            FloatField totalDistanceField = new() { label = "Approx. Distance", enabledSelf = false };
            totalDistanceField.AddToClassList("unity-base-field__aligned");
            root.Add(totalDistanceField);

            root.TrackPropertyValue(_property, RepaintGUI);
            RepaintGUI(_property);

            return root;

            void RepaintGUI(SerializedProperty _repaintProperty) {
                SerializedProperty distanceListProperty = _repaintProperty.FindPropertyRelative("distances");

                resolutionField.value = distanceListProperty.arraySize;

                if (distanceListProperty.arraySize == 0) { totalDistanceField.value = 0.0f; return; }
                totalDistanceField.value = distanceListProperty.GetArrayElementAtIndex(distanceListProperty.arraySize-1).floatValue;
            }
        }
    }
}