using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;


namespace MaroonSealEditor.Maths.Geometry.Paths {
    [CustomPropertyDrawer(typeof(SplinePath), true)]
    public class SplinePathPropertyDrawer : PointPathPropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            VisualElement root = base.CreatePropertyGUI(_property);
            root.Add(new PropertyField(_property.FindPropertyRelative("segmentResolution")));
            return root;
        }
    }
}