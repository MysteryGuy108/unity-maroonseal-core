
using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.DataStructures;
using System.Collections.Generic;

namespace MaroonSealEditor.DataStructures
{
    [CustomPropertyDrawer(typeof(CompassArray<>))]
    public class CompassArrayPropertyDrawer : PropertyDrawer
    {
        public static Dictionary<float, string> cardinalDisplayNames = new()
        {
            { 0.0f, "N" },
            { 22.5f, "NNE" },
            { 45.0f, "NE" },
            { 67.5f, "ENE" },
            { 90.0f, "E" },
            { 112.5f, "ESE" },
            { 135.0f, "SE" },
            { 157.5f, "SSE" },
            { 180.0f, "S" },
            { 202.5f, "SSW" },
            { 225.0f, "SW" },
            { 247.5f, "WSW" },
            { 270.0f, "W" },
            { 292.5f, "WNW" },
            { 315.0f, "NW"},
            { 337.5f, "NNW" }
        };

        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            SerializedProperty arrayProperty = _property.FindPropertyRelative("valueArray");

            PropertyField root = new(arrayProperty)
            {
                label = _property.displayName
            };

            root.RegisterCallbackOnce<GeometryChangedEvent>(SetList);
            root.AddToClassList("unity-base-field__aligned");

            return root;

            void SetList(GeometryChangedEvent _evnt)
            {
                ListView list = root.Q<ListView>();
                if (list == null) { return; } 
                list.bindItem += BindItem;
            }

            void BindItem(VisualElement _element, int _index)
            {
                float segment = 360.0f / arrayProperty.arraySize;

                float degrees = _index * segment;
                string cardinalName = "";

                if (cardinalDisplayNames.TryGetValue(degrees, out string cardinal)) { cardinalName = "("+cardinal+")"; }

                (_element as PropertyField).label = degrees.ToString("0.##") + "° " + cardinalName + " Element";
            }
        }
    }
}