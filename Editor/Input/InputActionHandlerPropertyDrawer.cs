using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Inputs;
using UnityEngine.InputSystem;

namespace MaroonSealEditor.Inputs
{
    [CustomPropertyDrawer(typeof(InputActionHandlerBase), true)]
    public class InputActionHandlerPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {

            VisualElement root = new()
            {
                name = "PropertyDrawer:InputActionHandler"
            };
            root.style.flexDirection = FlexDirection.Row;
            root.style.flexGrow = 1.0f;

            SerializedProperty inputAssetProperty = _property.FindPropertyRelative("inputAsset");
            InputActionAssetOptionsField popup = new((InputActionAsset)inputAssetProperty.objectReferenceValue)
            {
                label = _property.displayName,
                bindingPath = _property.FindPropertyRelative("actionPath").propertyPath
            };
            
            popup.style.width = 250.0f;

            root.Add(popup);

            ObjectField objectField = new()
            {
                name = "ObjectField:inputAsset",
                objectType = typeof(InputActionAsset),
                bindingPath = _property.FindPropertyRelative("inputAsset").propertyPath
            };

            objectField.style.flexGrow = 1.0f;

            objectField.RegisterValueChangedCallback(cntx => popup.SetAsset(cntx.newValue as InputActionAsset));

            root.Add(objectField);

            return root;
        }
    }
}