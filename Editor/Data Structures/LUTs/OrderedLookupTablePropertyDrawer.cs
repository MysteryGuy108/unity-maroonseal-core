using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal.DataStructures.LUTs;

namespace MaroonSealEditor.DataStructures.LUTs {

    [CustomPropertyDrawer(typeof(OrderedLookupTableBase<,>), true)]
    public class OrderedLookupTablePropertyDrawer : LookupTableBasePropertyDrawer
    {
        protected override void InstantiateListView(SerializedProperty _listProperty, ListView _listView)
        {
            _listView.reorderable = false;
        }

        protected override void BindItemElement(VisualElement _element, int _index)
        {
            _element.style.paddingRight = 4.0f;
        }
    }
}