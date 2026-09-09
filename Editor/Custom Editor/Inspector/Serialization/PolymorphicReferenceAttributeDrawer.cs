using System;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;

using MaroonSeal.Utilities.Serialization;

using MaroonSealEditor.UIElements;

namespace MaroonSealEditor.Utilities.Serialization {

    /// <summary>
    /// BUGGY! Can sometimes cause recursive drawing behaviour in the Inspector!
    /// </summary>
    [CustomPropertyDrawer(typeof(PolymorphicReferenceAttribute))]
    public class PolymorphicReferenceAttributeDrawer : PropertyDrawer
    {
        private static readonly HashSet<string> activeProperties = new();

        public override VisualElement CreatePropertyGUI(SerializedProperty _property)
        {
            // Guard to stop recursive drawing.
            string guardKey = GetGuardKey(_property);
            activeProperties.Add(guardKey);

            PolymorphicReferenceField referenceField = new(_property);

            referenceField.RegisterCallback<DetachFromPanelEvent>(_ => activeProperties.Remove(guardKey));
            return referenceField;
        }

        #region Recursive Guard
        /// <summary>
        /// True if a PolymorphicReferenceField is already drawing this exact
        /// property. Consulted by PolymorphicReferenceAttributeDrawer to avoid
        /// recursing into itself when this field's own RebuildField creates a
        /// PropertyField for the same [SerializeReference] property.
        /// </summary>
        public static bool IsBeingDrawn(SerializedProperty property) =>
            activeProperties.Contains(GetGuardKey(property));

        private static string GetGuardKey(SerializedProperty property) =>
            $"{property.serializedObject.targetObject.GetEntityId()}:{property.propertyPath}";
        #endregion
    }
}