using UnityEngine;

namespace MaroonSeal.Maths {
    public struct AngleMaths
    {
        static public float ModPI(float _theta) {
            _theta = Mathf.Repeat(_theta, 2.0f * Mathf.PI);
            if (_theta < -Mathf.PI) { return _theta + (2.0f * Mathf.PI); }
            if (_theta >= Mathf.PI) { return _theta - (2.0f * Mathf.PI); }
            return _theta;
        }

        static public float Mod180(float _theta) {
            _theta = Mathf.Repeat(_theta, 360.0f);
            if (_theta < -180.0f) return _theta + 360.0f;
            if (_theta >= 180.0f) return _theta - 360.0f;
            return _theta;
        }
    }
}
