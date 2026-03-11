using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSealEditor.Maths.Geometry.Paths
{
    [CustomPropertyDrawer(typeof(ConicSectionPath))]
    public class ConicSectionPathDrawer : ShapePathPropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            VisualElement root = base.CreatePropertyGUI(_property);
            SerializedProperty conicSectionProperty = _property.FindPropertyRelative("conicSection");

            root.Add(new PropertyField(conicSectionProperty.FindPropertyRelative("transform")));
            root.Add(new PropertyField(conicSectionProperty.FindPropertyRelative("eccentricity")));
            root.Add(new PropertyField(conicSectionProperty.FindPropertyRelative("minRadius")));

            root.Add(new PropertyField(_property.FindPropertyRelative("lutResolution")));

            return root;
        }
    }
}