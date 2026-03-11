using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSealEditor.Maths.Geometry.Paths
{
    [CustomPropertyDrawer(typeof(LinePath))]
    public class LinePathDrawer : ShapePathPropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            VisualElement root = base.CreatePropertyGUI(_property);
            SerializedProperty lineProperty = _property.FindPropertyRelative("line");

            root.Add(new PropertyField(lineProperty.FindPropertyRelative("start")));
            root.Add(new PropertyField(lineProperty.FindPropertyRelative("end")));
            root.Add(new PropertyField(_property.FindPropertyRelative("flipTangent")));
            return root;
        }
    }
}