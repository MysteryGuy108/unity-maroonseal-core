using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;

using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSealEditor.Maths.Geometry.Paths {
    abstract public class ShapePathPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            Foldout root = new()
            {
                text = _property.displayName,
                bindingPath = _property.propertyPath
            };

            root.AddToClassList("unity-foldout");
            root.AddToClassList("unity-list-view__foldout-header");
            root.AddToClassList("unity-foldout-input");

            return root;
        }
    }
}