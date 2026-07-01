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

            root.schedule.Execute(SetListView);
            
            return root;

            void SetListView()
            {
                ListView listView = root.Q<ListView>();
                if (listView == null) { Debug.Log("NO LIST FOUND"); return; }

                if (fixedCollectionAttribute.isFixedSize)
                {
                    
                }

                listView.reorderable = fixedCollectionAttribute.isFixedOrder;
                listView.reorderMode = fixedCollectionAttribute.isFixedOrder ? ListViewReorderMode.Simple : listView.reorderMode;

                //if ()
            }
        }
    }
}