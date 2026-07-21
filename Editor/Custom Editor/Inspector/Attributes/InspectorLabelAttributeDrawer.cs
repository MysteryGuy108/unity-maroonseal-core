using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal;

namespace MaroonSealEditor
{
    [CustomPropertyDrawer(typeof(InspectorLabelAttribute))]
    public class InspectorLabelAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            PropertyField root = new(_property) {
                label = (attribute as InspectorLabelAttribute).Label
            };
            return root;
        }
    }
}