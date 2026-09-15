using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSealEditor.Maths.Geometry.Paths {
    [CustomPropertyDrawer(typeof(LinearPath2D), true)]
    sealed public class LinearPath2DPropertyDrawer : PointPathBasePropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            VisualElement root = base.CreatePropertyGUI(_property);
            return root;
        }
    }
}