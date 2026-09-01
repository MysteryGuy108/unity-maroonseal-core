using System;
using UnityEngine;

namespace MaroonSeal
{
    public struct ColourHSV : IEquatable<ColourHSV>
    {
        public float h;
        public float s;
        public float v;
        public float a;

        #region Constructors
        public ColourHSV(float _h, float _s, float _v, float _a)
        {
            h = _h;
            s = _s;
            v = _v;
            a = _a;
        }

        public ColourHSV(Color _rgba)
        {
            float max = Mathf.Max(_rgba.r, Mathf.Max(_rgba.g, _rgba.b));
            float min = Mathf.Min(_rgba.r, Mathf.Min(_rgba.g, _rgba.b));

            if (max == _rgba.r) { h = (_rgba.g - _rgba.b)/(max-min); }
            else if (max == _rgba.g) { h = 2.0f + (_rgba.b-_rgba.r)/(max-min); }
            else { h = 4.0f + (_rgba.r-_rgba.g)/(max-min); }
            
            s = (max == 0.0f) ? 0.0f : 1.0f - (1.0f * min / max);
            v = max;
            a = _rgba.a;
        }
        #endregion


        public readonly bool Equals(ColourHSV _other) => 
            this.h == _other.h && this.s == _other.s && this.v == _other.v && this.a == _other.a;

        public Color ToRGBA()
        {
            throw new NotImplementedException();
        }
    }
}
