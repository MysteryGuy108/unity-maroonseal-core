using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSealEditor.Maths.Geometry.Paths
{
    [CustomPropertyDrawer(typeof(BezierPath))]
    public class BezierPathDrawer : ShapePathPropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            VisualElement root = base.CreatePropertyGUI(_property);
            SerializedProperty bezierProperty = _property.FindPropertyRelative("bezier");


            root.Add(new PropertyField(bezierProperty.FindPropertyRelative("anchorA")));
            root.Add(new PropertyField(bezierProperty.FindPropertyRelative("controlA")));
            root.Add(new PropertyField(bezierProperty.FindPropertyRelative("controlB")));
            root.Add(new PropertyField(bezierProperty.FindPropertyRelative("anchorB")));

            root.Add(new PropertyField(_property.FindPropertyRelative("startRoll")));
            root.Add(new PropertyField(_property.FindPropertyRelative("endRoll")));
            root.Add(new PropertyField(_property.FindPropertyRelative("lutResolution")));
            return root;
        }
    }
}