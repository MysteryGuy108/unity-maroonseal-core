using UnityEngine;

namespace MaroonSeal.Maths {
    public static class LerpMaths {

        #region Lerp
        static public float Time(float _a, float _b, float _t) => Mathf.Lerp(_a, _b, _t);

        static public float TimeWrapped(float _a, float _b, float _t) {
            float startAngle = _a / 360.0f; float endAngle = _b / 360.0f;

            float step =  Mathf.LerpAngle(startAngle, endAngle, _t) / 360.0f;
            return Mathf.Repeat(step, 1.0f);
        }

        static public float Towards(float _current, float _target, float _delta) => Mathf.MoveTowards(_current, _target, _delta);

        static public float TowardsWrapped(float _current, float _target, float _delta) {
            return Mathf.MoveTowardsAngle(_current * 360.0f, _target * 360.0f, _delta * 360.0f) / 360.0f;
        }
        #endregion

        #region Delta
        static public float GetTimeDelta(float _from, float _to) => _to - _from;
        static public float GetTimeDeltaWrapped(float _from, float _to) => Mathf.DeltaAngle(_from * 360.0f, _to * 360.0f) / 360.0f; 
        #endregion
    }


}