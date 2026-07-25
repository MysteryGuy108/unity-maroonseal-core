using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.DataStructures.LUTs;

namespace MaroonSealEditor.DataStructures.LUTs {
    [CustomPropertyDrawer(typeof(LookupTable<,>), true)]
    public class LookupTablePropertyDrawer : LookupTableBasePropertyDrawer
    {
        protected override void InstantiateListView(SerializedProperty _listProperty, ListView _listView)
        {
            // button adding behaviour
            _listView.overridingAddButtonBehavior = (baseListView, addButton) =>
            {
                int newIndex = _listProperty.arraySize;
                _listProperty.InsertArrayElementAtIndex(newIndex);

                SerializedProperty newItem = _listProperty.GetArrayElementAtIndex(newIndex);
                SetUniqueLUTItemValues(newItem, newIndex);

                _listProperty.serializedObject.ApplyModifiedProperties();

                baseListView.RefreshItems();
                baseListView.selectedIndex = newIndex;
            };
        }

        protected override void BindItemElement(VisualElement _element, int _index)
        {
            _element.parent.style.paddingLeft = 0.0f;
        }
    }
}