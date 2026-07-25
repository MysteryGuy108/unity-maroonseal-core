using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace MaroonSeal.UIElements
{
    public class EulerAngleField : BaseField<Quaternion>
    {
        private readonly Vector3Field eulerField;

        private Vector3 _cachedEuler;
        private bool _hasCachedEuler;
        
        public EulerAngleField(string label = "Rotation") : base(label, new Vector3Field(label))
        {
            eulerField = this.Q<Vector3Field>(className: inputUssClassName);
            eulerField.AddToClassList("unity-base-field__aligned");
            
            labelElement.style.display = DisplayStyle.None;

            this.style.marginLeft = 0.0f;
            this.style.flexGrow = 1.0f;

            eulerField.RegisterValueChangedCallback(evt => {
                _cachedEuler = evt.newValue;
                _hasCachedEuler = true;

                value = Quaternion.Euler(_cachedEuler); });
        }

        override public void SetValueWithoutNotify(Quaternion _value)
        {
            base.SetValueWithoutNotify(_value);

            Vector3 euler = _value.eulerAngles;

            if (!_hasCachedEuler)
            {
                _cachedEuler = euler;
                _hasCachedEuler = true;
            }
            else
            {
                _cachedEuler.x = ClosestAngle(_cachedEuler.x, euler.x);
                _cachedEuler.y = ClosestAngle(_cachedEuler.y, euler.y);
                _cachedEuler.z = ClosestAngle(_cachedEuler.z, euler.z);
            }

            eulerField.SetValueWithoutNotify(_cachedEuler);
        }

        private static float ClosestAngle(float _previous, float _current)
        {
            _current %= 360f;

            while (_current - _previous > 180f)
                _current -= 360f;

            while (_previous - _current > 180f)
                _current += 360f;

            return _current;
        }
    }
}
