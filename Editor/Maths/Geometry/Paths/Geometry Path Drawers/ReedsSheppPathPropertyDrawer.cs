using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths.ReedsShepp;

namespace MaroonSealEditor.Maths.Geometry.Paths.ReedsShepp {
    [CustomPropertyDrawer(typeof(ReedsSheppPath), true)]
    sealed public class ReedsSheppPathPropertyDrawer : ShapePathPropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property){
            VisualElement root = base.CreatePropertyGUI(_property);
            
            root.AddToClassList("unity-base-field__aligned");

            //root.Add(new PropertyField(_property.FindPropertyRelative("currentWord")) { enabledSelf = false });

            root.Add(new PropertyField(_property.FindPropertyRelative("minRadius")));
            root.Add(new PropertyField(_property.FindPropertyRelative("startPoint")));

            return root;
        }
    }
}