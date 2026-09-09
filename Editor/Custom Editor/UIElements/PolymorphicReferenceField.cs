using System;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths.Geometry.Paths;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace MaroonSealEditor.UIElements {
    public class PolymorphicReferenceField : VisualElement
    {
        private readonly SerializedObject serializedObject;
        private readonly string propertyPath;

        private readonly List<Type> ignoreTypes;
        private readonly Dictionary<Type, string> typeDisplayNames;

        public Type SelectedType
        {
            get
            {
                SerializedProperty property = serializedObject.FindProperty(propertyPath);
                return property?.managedReferenceValue?.GetType();
            }
        }

        public event Action<Type> SelectedTypeChanged;

        #region Constructor
        public PolymorphicReferenceField(SerializedProperty _property, Type[] _ignoreTypes = null)
        {
            propertyPath = _property.propertyPath;
            serializedObject = _property.serializedObject;

            ignoreTypes = _ignoreTypes == null ? new() : new(_ignoreTypes);
            ignoreTypes.Add(typeof(UnityEngine.Object));
            
            typeDisplayNames = new();

            // Building foldout.
            Foldout foldout = new() { text = _property.displayName };
            this.Add(foldout);

            // Building header and container for type field.
            VisualElement header = BuildHeader(out Button button);
            VisualElement fieldContainer = new();
            
            foldout.Add(header);
            foldout.Add(fieldContainer);

            // Getting the type of the property through reflection.
            Type propertyType = GetFieldType(_property);

            button.clicked += OnButtonClicked;

            this.TrackPropertyValue(_property, Refresh);
            Refresh(_property);

            void Refresh(SerializedProperty currentProperty)
            {
                button.text = GetButtonLabelText(currentProperty);
                RebuildField(fieldContainer, currentProperty);
            }

            void OnButtonClicked()
            {
                serializedObject.Update();

                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("None"), SelectedType == null,
                    () => AssignType(serializedObject, propertyPath, null));

                if (propertyType != null)
                {
                    foreach (var type in TypeCache.GetTypesDerivedFrom(propertyType))
                    {   
                        // Checking if type is to be ignored.
                        bool ignore = false;

                        foreach(var ignoreType in ignoreTypes) {
                            ignore = type.IsSubclassOf(ignoreType) || type == ignoreType;
                            if (ignore) { break; }
                        }

                        if (ignore) continue;

                        if (type.IsAbstract || type.ContainsGenericParameters) continue;
                        var capturedType = type;

                        // Getting menu path display name
                        if (typeDisplayNames.TryGetValue(capturedType, out string menuPath)) { menuPath = name; }
                        else { menuPath = GetInheritancePath(type, propertyType); }

                        // Adding menu item
                        menu.AddItem(new GUIContent(menuPath), capturedType == SelectedType,
                            () => AssignType(serializedObject, propertyPath, capturedType));
                    }
                }
                menu.ShowAsContext();
            }
        }
        #endregion

        #region Header Building
        private VisualElement BuildHeader(out Button _button)
        {
            VisualElement headerContainer = new();
            headerContainer.AddToClassList(BaseField<string>.ussClassName); // "unity-base-field"
            headerContainer.AddToClassList(BaseField<string>.alignedFieldUssClassName); // "unity-base-field__aligned"
            headerContainer.style.flexDirection = FlexDirection.Row;

            Label headerLabel = new("Type");
            headerLabel.AddToClassList(BaseField<string>.labelUssClassName); // "unity-base-field__label"

            _button = new() {};
            _button.AddToClassList(BaseField<string>.inputUssClassName); // "unity-base-field__input"
            _button.style.flexGrow = 1.0f;

            _button.style.marginRight = 0.0f;
            //_button.style.marginLeft = 102.0f;

            headerContainer.Add(headerLabel);
            headerContainer.Add(_button);

            return headerContainer;
        }

        private string GetButtonLabelText(SerializedProperty _property) =>
            _property.managedReferenceValue != null ? ObjectNames.NicifyVariableName(_property.managedReferenceValue.GetType().Name) : "Select Type";
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
            if (_property.managedReferenceValue == null) return;

            PropertyField propertyField = new(_property);
            propertyField.Bind(_property.serializedObject);
            _container.Add(propertyField);

            propertyField.RegisterCallback<GeometryChangedEvent>(HideInnerFoldout);

            void HideInnerFoldout(GeometryChangedEvent _evt)
            {
                Foldout innerFoldout = propertyField.Q<Foldout>();
                if (innerFoldout == null) return; // not built yet — wait for the next layout pass

                // Force it open and hide the toggle row, rather than removing
                // and reparenting content. PropertyField can rebuild its own
                // subtree after the initial construction (e.g. on rebind), and
                // structural edits here don't survive that; style changes do,
                // since we just reapply them on the next GeometryChangedEvent.
                innerFoldout.value = true;

                VisualElement toggle = innerFoldout.Q(className: Foldout.toggleUssClassName);
                if (toggle != null) toggle.style.display = DisplayStyle.None;

                VisualElement fieldContent = innerFoldout.Q(className: Foldout.contentUssClassName);
                if (fieldContent != null) fieldContent.style.marginLeft = 0f;

                // Deliberately left registered — see comment above.
            }
        }
        private static Type GetFieldType(SerializedProperty _property)
        {
            string typenameString = _property.managedReferenceFieldTypename;
            if (string.IsNullOrEmpty(typenameString)) return null;

            // Format is "AssemblyName Namespace.ClassName"
            int splitIndex = typenameString.IndexOf(' ');
            if (splitIndex < 0) return null;

            string assemblyName = typenameString[..splitIndex];
            string className = typenameString[(splitIndex + 1)..];

            var assembly = System.Reflection.Assembly.Load(assemblyName);
            return assembly?.GetType(className);
        }
        
        private static string GetInheritancePath(Type type, Type rootType)
        {
            var chain = new List<string>();
            Type current = type;

            if (rootType.IsInterface)
            {
                // Interfaces never appear in BaseType, so walk up while the
                // current class still implements rootType, and stop once it doesn't.
                while (current != null && rootType.IsAssignableFrom(current))
                {
                    chain.Add(GetCleanTypeName(current));
                    current = current.BaseType;
                }
            }
            else
            {
                while (current != null && current != rootType)
                {
                    chain.Add(GetCleanTypeName(current));
                    current = current.BaseType;
                }
            }

            chain.Reverse();
            return string.Join("/", chain);
        }

        private static string GetCleanTypeName(Type type)
        {
            string name = type.Name;
            int backtickIndex = name.IndexOf('`');
            return ObjectNames.NicifyVariableName(backtickIndex >= 0 ? name[..backtickIndex] : name);
        }
        #endregion
    }
}