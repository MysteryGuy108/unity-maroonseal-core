using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal;

namespace MaroonSealEditor
{
    [CustomPropertyDrawer(typeof(EventBasedValue<>), true)]
    public class EventBasedValuePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            PropertyField valueProperty = new(_property.FindPropertyRelative("current"))
            {
                label = _property.displayName
            };

            return valueProperty;
        }
    }
}
