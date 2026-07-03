using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal;

namespace MaroonSealEditor
{
    [CustomPropertyDrawer(typeof(FixedListViewAttribute))]
    public class FixedListViewAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty _property) {
            
            PropertyField root = new(_property);
            
            FixedListViewAttribute fixedCollectionAttribute = attribute as FixedListViewAttribute;

            root.RegisterCallbackOnce<GeometryChangedEvent>(SetListView);
            
            return root;

            void SetListView(GeometryChangedEvent _evnt)
            {
                ListView listView = root.Q<ListView>();
                if (listView == null) { Debug.Log("No ListView Found"); return; }

                TextField sizeTextField = listView.Q<TextField>("unity-list-view__size-field");
                sizeTextField.enabledSelf = !fixedCollectionAttribute.isFixedSize;

                VisualElement listFooter = listView.Q("unity-list-view__footer");
                listFooter.style.display = fixedCollectionAttribute.isFixedSize ? DisplayStyle.None : listFooter.style.display;

                listView.allowAdd = !fixedCollectionAttribute.isFixedSize;
                listView.allowRemove = !fixedCollectionAttribute.isFixedSize;

                listView.reorderable = !fixedCollectionAttribute.isFixedOrder;
                listView.reorderMode = fixedCollectionAttribute.isFixedOrder ? ListViewReorderMode.Simple : listView.reorderMode;
            }
        }
    }
}