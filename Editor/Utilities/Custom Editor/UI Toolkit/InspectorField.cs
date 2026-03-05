using UnityEngine;
using UnityEngine.UIElements;

namespace MaroonSealEditor
{
    public class InspectorField : BaseField<object>
    {   
        public InspectorField(string label, VisualElement content) : base(label, null)
        {
            this.AddToClassList(alignedFieldUssClassName);

            // We need to remove the default visualInput element since it takes space between the label and our custom content
            var defaultInput = this.Q(className: inputUssClassName);
            defaultInput?.parent.Remove(defaultInput);
            
            Add(content);
        }
    }
}

