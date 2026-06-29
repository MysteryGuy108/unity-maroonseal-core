using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.Maths;


namespace MaroonSealEditor.Maths
{
    [CustomPropertyDrawer(typeof(Cardinal4Array<>))]
    public class Cardinal4ArrayPropertyDrawer : PropertyDrawer
    {
        public static string[] cardinalDisplayNames = new string[4] { "North", "South", "East", "West" };
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {

            SerializedProperty arrayProperty = _property.FindPropertyRelative("valueArray");
            if (arrayProperty == null) { return null; }

            Foldout root = new() {
                text = _property.displayName,
                bindingPath = _property.propertyPath
            };

            IEnumerator enumerator = arrayProperty.GetEnumerator();
            
            for(int i = 0; i < 4; i++)
            {
                if (!enumerator.MoveNext()) { break; }
                SerializedProperty arrayElementProperty = enumerator.Current as SerializedProperty;

                PropertyField propertyField = new(arrayElementProperty) {
                    name = cardinalDisplayNames[i] + " PropertyField",
                    label = cardinalDisplayNames[i],
                    bindingPath = arrayElementProperty.propertyPath
                };

                //propertyField.schedule.Execute(()=>propertyField.Q<Label>(). );
                
                root.Add(propertyField);
                root.AddToClassList("unity-base-field__aligned");
            }

            return root;
        }
    }
}