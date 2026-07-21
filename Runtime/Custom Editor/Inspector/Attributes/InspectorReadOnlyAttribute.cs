using System;

using UnityEngine;
using UnityEngine.UIElements;

namespace MaroonSeal
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class InspectorReadOnlyAttribute : PropertyAttribute
    {
        public InspectorReadOnlyAttribute(){}
    }
}