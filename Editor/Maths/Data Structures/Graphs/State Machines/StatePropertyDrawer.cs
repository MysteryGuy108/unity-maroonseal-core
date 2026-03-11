using UnityEngine;
using UnityEditor;

using MaroonSeal.Maths.DataStructures.Graphs.StateMachines;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace MaroonSealEditor.Maths.DataStructures.Graphs.StateMachines
{
    [CustomPropertyDrawer(typeof(StateBase), true)]
    public class StatePropertyDrawer : PropertyDrawer
    {
        [SerializeField] private Texture2D background;

        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            VisualElement root = new();
            SetRoot(root);

            PropertyField stateProperty = new(_property) {};
            SetStateProperty(stateProperty);

            SerializedProperty isActiveProperty = _property.FindPropertyRelative("isActive");
            Toggle toggleField = new() { bindingPath = isActiveProperty.propertyPath };
            toggleField.style.display = DisplayStyle.None;

            root.Add(stateProperty);
            root.Add(toggleField);

            toggleField.RegisterValueChangedCallback(cntx => SetLabel(cntx.newValue));
            SetLabel(isActiveProperty.boolValue);

            return root;

            void SetLabel(bool _isActive)
            {
                stateProperty.label = "State: " + _property.displayName;
                stateProperty.label += _isActive ? " <color=green>[Active]</color>" : "";
            }
        }

        private void SetRoot(VisualElement _root)
        {
            _root.style.backgroundImage = background;
            _root.style.unityBackgroundImageTintColor = new Color(0.35f, 0.35f, 0.35f, 1.0f);
            
            _root.style.unitySliceLeft = 10;
            _root.style.unitySliceRight = 10;
            _root.style.unitySliceTop = 10;
            _root.style.unitySliceBottom = 10;
            _root.style.unitySliceScale = 1;
            _root.style.unitySliceType = SliceType.Tiled;
        }

        private void SetStateProperty(PropertyField _property)
        {
            _property.style.marginLeft = 20.0f;
            _property.style.marginRight = 16.0f;

            _property.style.marginTop = 6.0f;
            _property.style.marginBottom = 6.0f;
        }
    }
}