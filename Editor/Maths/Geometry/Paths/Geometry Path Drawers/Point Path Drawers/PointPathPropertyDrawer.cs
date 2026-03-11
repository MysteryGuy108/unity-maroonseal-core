using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;


namespace MaroonSealEditor.Maths.Geometry.Paths {
    [CustomPropertyDrawer(typeof(PointPath<,>), true)]
    public class PointPathPropertyDrawer : ShapePathPropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            VisualElement root = new();
            
            root.Add(new PropertyField(_property.FindPropertyRelative("isLoop")));
            root.Add(new PropertyField(_property.FindPropertyRelative("points")));

            return root;
        }
    }
}