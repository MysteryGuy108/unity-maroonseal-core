using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSealEditor.Maths.Geometry.Paths {
    [CustomPropertyDrawer(typeof(LinearPoint), true)]
    sealed public class LinearPointPropertyDrawer : PropertyDrawer
    {
        #region Property Drawer
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
        
            Foldout foldout = new() {
                text = _property.displayName,
                bindingPath = _property.propertyPath
            };
        
            foldout.Add(new PropertyField(_property.FindPropertyRelative("position")));
            foldout.Add(new PropertyField(_property.FindPropertyRelative("roll")));
            foldout.Add(new PropertyField(_property.FindPropertyRelative("size")));

            return foldout;
        }
        #endregion
    }
}

