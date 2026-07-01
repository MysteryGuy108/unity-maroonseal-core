using System;
using UnityEngine;

namespace MaroonSeal
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class FixedListViewAttribute : PropertyAttribute
    {
        readonly public bool isFixedSize;
        readonly public bool isFixedOrder;

        public FixedListViewAttribute(bool _fixedSize = true, bool _fixedOrder = true)
        {
            isFixedSize = _fixedSize;
            isFixedOrder = _fixedOrder;
        }
    }
}
