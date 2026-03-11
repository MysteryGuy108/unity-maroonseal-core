
using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Inputs;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace MaroonSealEditor.Inputs
{
    [CustomEditor(typeof(InputHandler<>), true)]
    [CanEditMultipleObjects]
    public class InputHandlerEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            InspectorElement.FillDefaultInspector(root, this.serializedObject, this);

            root.Q<PropertyField>("PropertyField:OnInputChanged").BringToFront();

            PropertyField currentInputField = root.Q<PropertyField>("PropertyField:currentInput");
            currentInputField?.BringToFront();
            currentInputField?.SetEnabled(false);

            return root;
        }
    }
}