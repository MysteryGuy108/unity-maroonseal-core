using UnityEngine;

namespace MaroonSeal {
    public static class AnimationCurveExtensions
    {
        public static void MakeLinear(this AnimationCurve _curve) {
            if (_curve.keys.Length == 0) { return; }

            for(int i = 0; i < _curve.keys.Length; i++) {
                Keyframe point = _curve.keys[i];

                if (i-1 >= 0) {
                    Keyframe previousPoint = _curve.keys[i-1];
                    _curve.keys[i].inTangent = (point.value - previousPoint.value) / (point.time - previousPoint.time);
                }
                
                if (i+1 < _curve.keys.Length) {
                    Keyframe nextPoint = _curve.keys[i+1];
                    _curve.keys[i].outTangent = (nextPoint.value - point.value) / (nextPoint.time - point.time);
                }

                _curve.keys[i] = point;
            }
        }
    }
}
