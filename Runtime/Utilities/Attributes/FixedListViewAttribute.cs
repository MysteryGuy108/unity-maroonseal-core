using System;
using UnityEngine;

namespace MaroonSeal
{
    /// <summary>
    /// Class attribute used to draw ListViews with fixed size and order.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class FixedListViewAttribute : PropertyAttribute
    {
        readonly public bool isFixedSize;
        readonly public bool isFixedOrder;

        /// <summary>
        /// Fixed list attribute used to draw ListViews with fixed size and order.
        /// </summary>
        /// <param name="_fixedSize">Fix the number of elements in the list view.</param>
        /// <param name="_fixedOrder">Fix the order of elements in the list view.</param>
        public FixedListViewAttribute(bool _fixedSize = true, bool _fixedOrder = true)
        {
            isFixedSize = _fixedSize;
            isFixedOrder = _fixedOrder;
        }
    }
}
