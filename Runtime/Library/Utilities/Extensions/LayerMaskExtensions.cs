using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal {
    public static class LayerMaskExtensions
    {
        public static bool ContainsLayer(this LayerMask _mask, int _layer) {
            return  _mask == (_mask | (1 << _layer));
        }
    }
}