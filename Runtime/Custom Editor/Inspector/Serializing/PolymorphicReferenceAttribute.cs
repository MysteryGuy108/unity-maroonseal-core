using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Utilities.Serialization {
    [AttributeUsage(AttributeTargets.Field)]
    public class PolymorphicReferenceAttribute : PropertyAttribute
    {
        public PolymorphicReferenceAttribute() {
        }
    }
}