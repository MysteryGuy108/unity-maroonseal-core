using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace MaroonSealEditor
{
    public class AngleField : BaseField<float>
    {
        private readonly Vector2Field angleField;

        public AngleField(string label = "Angle") : base(label, new Vector2Field(label))
        {
            angleField = this.Q<Vector2Field>(className: inputUssClassName);
            angleField.AddToClassList("unity-base-field__aligned");
            
            labelElement.style.display = DisplayStyle.None;

            this.style.marginLeft = 0.0f;
            this.style.flexGrow = 1.0f;

            FloatField xInput = angleField.Q<FloatField>("unity-x-input");
            FloatField yInput = angleField.Q<FloatField>("unity-y-input");

            xInput.label = "θ";
            xInput.style.flexGrow = 0.49125f;
            yInput.style.display = DisplayStyle.None;

            angleField.RegisterValueChangedCallback(evt => value = evt.newValue.x);
        }

        override public void SetValueWithoutNotify(float _value)
        {
            base.SetValueWithoutNotify(_value);
            angleField.SetValueWithoutNotify(new Vector2(_value, 0.0f));
        }
    }
}