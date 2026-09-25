using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace MaroonSealEditor.UIElements {
    public class PolymorphicReferenceField : VisualElement
    {
        private readonly SerializedObject serializedObject;
        private readonly string propertyPath;

        private readonly HashSet<Type> ignoreTypes;

        public Type SelectedType => GetPropertyType(serializedObject.FindProperty(propertyPath));

        public event Action<Type> SelectedTypeChanged;

        #region Constructor
        public PolymorphicReferenceField(SerializedProperty _property, Type[] _ignoreTypes = null)
        {
            propertyPath = _property.propertyPath;
            serializedObject = _property.serializedObject;

            ignoreTypes = _ignoreTypes == null ? new() : new(_ignoreTypes);
            ignoreTypes.Add(null);
            ignoreTypes.Add(typeof(UnityEngine.Object));

            // Building foldout.
            Foldout foldout = new() { text = _property.displayName };
            this.Add(foldout);

            // Building header and container for type field.
            DropdownField dropdown = BuildDropdown();
            VisualElement fieldContainer = new();
            
            foldout.Add(dropdown);
            foldout.Add(fieldContainer);
            dropdown.RegisterCallback<ClickEvent>((cntx) => OnButtonClicked());

            this.TrackPropertyValue(_property, Refresh);
            Refresh(_property);

            void Refresh(SerializedProperty changedProperty)
            {
                RebuildField(fieldContainer, changedProperty);
                RefreshDropdownLabel(dropdown, SelectedType);
            }

            void OnButtonClicked()
            {
                serializedObject.Update();
                // Getting type through reflection.
                Type propertyType = PolymorphicReferenceMenu.GetFieldType(_property);
                PolymorphicReferenceMenu.Build(propertyType, SelectedType, (cntx) => AssignType(serializedObject, propertyPath, cntx), ignoreTypes);
            }
        }
        #endregion

        #region Header Building
        private DropdownField BuildDropdown()
        {
            DropdownField dropdownButton = new("Type");
            dropdownButton.AddToClassList(DropdownField.alignedFieldUssClassName); // "unity-base-field__input"
            dropdownButton.AddToClassList(DropdownField.ussClassName + "__inspector-field"); // "unity-base-field__inspector-field"

            return dropdownButton;
        }
        
        private void RefreshDropdownLabel(DropdownField _dropdown, Type _propertyType)
        {
            string labelText;

            if (ignoreTypes.Contains(_propertyType)) labelText = "Select Type";
            else labelText = ObjectNames.NicifyVariableName(_propertyType.Name); 

            TextElement textElement = _dropdown.Q<TextElement>(className: DropdownField.textUssClassName);
            if (textElement != null) textElement.text = labelText;
        }
        #endregion

        #region Type Field
        private void AssignType(SerializedObject _serializedObject, string _propertyPath, Type _type)
        {
            _serializedObject.Update();
            var property = _serializedObject.FindProperty(_propertyPath);
            if (SelectedType == _type) return;

            property.managedReferenceValue = _type != null ? Activator.CreateInstance(_type) : null;
            _serializedObject.ApplyModifiedProperties();

            SelectedTypeChanged?.Invoke(SelectedType);
        }

        private void RebuildField(VisualElement _container, SerializedProperty _property)
        {
            _container.Clear();
            if (ignoreTypes.Contains(SelectedType)) return; 

            PropertyField propertyField = new(_property);
            propertyField.Bind(_property.serializedObject);
            _container.Add(propertyField);

            propertyField.RegisterCallback<GeometryChangedEvent>(HideInnerFoldout);

            void HideInnerFoldout(GeometryChangedEvent _evt)
            {
                Foldout innerFoldout = propertyField.Q<Foldout>();
                if (innerFoldout == null) return;
                innerFoldout.value = true;

                VisualElement toggle = innerFoldout.Q(className: Foldout.toggleUssClassName);
                if (toggle != null) toggle.style.display = DisplayStyle.None;

                VisualElement fieldContent = innerFoldout.Q(className: Foldout.contentUssClassName);
                if (fieldContent != null) fieldContent.style.marginLeft = 0f;
            }
        }

        private static Type GetPropertyType(SerializedProperty _property)
        {
            Type selectedType = _property?.managedReferenceValue?.GetType();
            return CheckFieldTypeValid(selectedType) ? selectedType : null;
        }

        private static bool CheckFieldTypeValid(Type _type) 
            => _type != null && !_type.IsNestedPrivate;
        #endregion
    }
}