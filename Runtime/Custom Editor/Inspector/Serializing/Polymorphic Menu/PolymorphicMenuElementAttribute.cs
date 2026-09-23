using System;
using UnityEngine;

namespace MaroonSeal.Utilities.Serialization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PolymorphicMenuElementAttribute : PropertyAttribute
    {
        readonly public bool isHidden;
        readonly public string classMenuPath;
        public PolymorphicMenuElementAttribute(string _menuPath, bool _isHidden = false)
        {
            classMenuPath = _menuPath;
            isHidden = _isHidden;
        }
    }
}
