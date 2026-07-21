using System;

using UnityEngine;

namespace MaroonSeal
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class InspectorLabelAttribute : PropertyAttribute
    {
        public string Label { get; }
        public InspectorLabelAttribute(string _label)
        {
            Label = _label;
        }
    }
}
