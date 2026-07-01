using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using MaroonSeal;
using System;

namespace MaroonSeal
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class EditorReadOnlyAttribute : PropertyAttribute
    {
        public EditorReadOnlyAttribute(){}
    }
}