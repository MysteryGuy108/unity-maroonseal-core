using System;
using UnityEngine;

namespace MaroonSeal
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PolymorphicMenuElementAttribute : PropertyAttribute
    {
        readonly public string classMenuPath;
        public PolymorphicMenuElementAttribute(string _menuPath)
        {
            classMenuPath = _menuPath;
        }
    }
}
