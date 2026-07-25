using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace MaroonSealEditor.UIElements {
    public class PropertyFieldFoldout : VisualElement
    {
        public static readonly string ussClassName = "property-field-foldout";
        public static readonly string headerUssClassName = ussClassName + "__header";

        readonly Toggle arrowToggle;
        readonly VisualElement header;
        readonly PropertyField propertyField;
        readonly VisualElement m_ContentContainer;

        public override VisualElement contentContainer => m_ContentContainer;

        public bool value
        {
            get => arrowToggle.value;
            set => arrowToggle.SetValueWithoutNotify(value);
        }

        // Pass in the SerializedProperty that should be rendered inline next to the arrow
        public PropertyFieldFoldout(SerializedProperty property, string label = null, bool startExpanded = true)
        {
            AddToClassList(ussClassName);

            // --- Header row: arrow + property field ---
            header = new VisualElement { name = "header" };
            header.AddToClassList(headerUssClassName);
            header.style.flexDirection = FlexDirection.Row;
            header.style.alignItems = Align.Center;
            hierarchy.Add(header);

            // A bare Toggle, styled with the SAME class Foldout uses internally.
            // Unity's built-in stylesheet turns the checkmark into the arrow
            // glyph for anything with this class - this is literally how
            // Foldout itself draws its arrow.
            arrowToggle = new Toggle { text = string.Empty };
            arrowToggle.AddToClassList(Foldout.toggleUssClassName);
            arrowToggle.style.flexGrow = 0;
            arrowToggle.RegisterValueChangedCallback(OnToggleChanged);
            header.Add(arrowToggle);

            // The PropertyField takes the place of the Foldout's text label
            propertyField = new PropertyField(property, label ?? property.displayName)
            {
                style = { flexGrow = 1 }
            };
            header.Add(propertyField);

            // --- Content container: same class Foldout uses, so it gets
            //     the same indentation from Unity's default stylesheet ---
            m_ContentContainer = new VisualElement { name = "unity-content" };
            m_ContentContainer.AddToClassList(Foldout.contentUssClassName);
            hierarchy.Add(m_ContentContainer);

            arrowToggle.SetValueWithoutNotify(startExpanded);
            m_ContentContainer.style.display = startExpanded ? DisplayStyle.Flex : DisplayStyle.None;
        }

        void OnToggleChanged(ChangeEvent<bool> evt)
        {
            m_ContentContainer.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            evt.StopPropagation();
        }
    }
}