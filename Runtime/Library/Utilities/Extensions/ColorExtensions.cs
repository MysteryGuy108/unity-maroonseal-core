using UnityEngine;

namespace MaroonSeal {
    public static class ColorExtensions
    {
        public static Color FromAlpha(this Color _colour, float _alpha) => new(_colour.r, _colour.g, _colour.b, _alpha);
    }
}