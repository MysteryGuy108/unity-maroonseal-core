using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSealEditor.Maths.Geometry.Paths
{
    [CustomPropertyDrawer(typeof(ArcPath))]
    public class ArcPathDrawer : ShapePathPropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            VisualElement root = base.CreatePropertyGUI(_property);
            SerializedProperty arcProperty = _property.FindPropertyRelative("arc");
            root.Add(new PropertyField(arcProperty.FindPropertyRelative("transform")));
            root.Add(new PropertyField(arcProperty.FindPropertyRelative("radius")));
            root.Add(new PropertyField(arcProperty.FindPropertyRelative("startDegrees")));
            root.Add(new PropertyField(arcProperty.FindPropertyRelative("endDegrees")));
            return root;
        }
    }
}