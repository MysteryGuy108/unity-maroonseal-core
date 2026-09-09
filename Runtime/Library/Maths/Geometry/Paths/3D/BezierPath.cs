using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    sealed public class BezierPath : SamplePath
    {
        public override bool IsLoop => false;

        [SerializeField] private CubicBezier bezier;

        #region Constructors
        public BezierPath() {}
        public BezierPath(CubicBezier _bezier) => bezier = _bezier;
        #endregion

        #region GeometryPath
        public override Transform3D EvaluateTime(float _t)
        {
            Vector3 position = bezier.EvaluatePointAtTime(_t);
            Vector3 forward = bezier.EvaluateTangentAtTime(_t);

            forward = (forward == Vector3.zero ? Vector3.forward : forward).normalized;

            float roll = Mathf.Lerp(startRoll, endRoll, _t);
            Quaternion rotation = Quaternion.AngleAxis(roll, forward) * Quaternion.LookRotation(forward, Vector3.up);

            return new(position, rotation);
        }
        #endregion

        #region Bezier Path
        public CubicBezier GetBezier() => bezier;

        public void SetBezier(CubicBezier _bezier, float? _startRoll = null, float? _endRoll = null)
        {
            bezier = _bezier;

            startRoll = _startRoll ?? startRoll;
            endRoll = _endRoll ?? endRoll;

            if (bezier != _bezier) { SetDirty(); }
        }

        public void SetBezierPoints(Vector3 _anchorA, Vector3 _controlA, Vector3 _controlB, Vector3 _anchorB,
                                    float? _startRoll = null, float? _endRoll = null)
        {
            SetBezier(new(_anchorA, _controlA, _controlB, _anchorB), _startRoll, _endRoll);
        }
        #endregion
    }
}