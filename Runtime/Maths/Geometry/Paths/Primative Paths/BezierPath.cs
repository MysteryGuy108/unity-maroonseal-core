using UnityEngine;

using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    sealed public class BezierPath : LUTPath
    {
        public override bool IsLoop => false;

        [SerializeField] private CubicBezier bezier;
        [Space]
        [SerializeField][Range(-180.0f, 180.0f)] private float startRoll;
        [SerializeField][Range(-180.0f, 180.0f)] private float endRoll;

        #region Constructors
        public BezierPath() { SetResolution(2); }
        public BezierPath(int _lutResolution = 2) { SetResolution(_lutResolution); }
        public BezierPath(CubicBezier _bezier, int _resolution)
        {
            bezier = _bezier;
            SetResolution(_resolution);
        }
        #endregion

        #region GeometryPath
        public override PointTransform GetPointAtTime(float _t)
        {

            Vector3 position = bezier.EvaluatePositionAtTime(_t);
            Vector3 forward = bezier.EvaluateTangentAtTime(_t);

            forward = (forward == Vector3.zero ? Vector3.forward : forward).normalized;

            float roll = Mathf.Lerp(startRoll, endRoll, _t);
            Quaternion rotation = Quaternion.AngleAxis(roll, forward) * Quaternion.LookRotation(forward, Vector3.up);

            return new(position, rotation);
        }
        #endregion

        #region Bezier Path
        public CubicBezier GetBezier() => bezier;

        public void SetBezier(CubicBezier _bezier, float? _startRoll = null, float? _endRoll = null, int? _lutResolution = null)
        {
            int newResolution = _lutResolution ?? lutResolution;
            newResolution = Mathf.Max(newResolution, 2);

            bool needsRefresh = bezier != _bezier || newResolution != lutResolution;

            bezier = _bezier;

            startRoll = _startRoll ?? startRoll;
            endRoll = _endRoll ?? endRoll;

            lutResolution = _lutResolution ?? lutResolution;

            if (needsRefresh) { Refresh(); }
        }

        public void SetBezierPoints(Vector3 _anchorA, Vector3 _controlA, Vector3 _controlB, Vector3 _anchorB,
                                    float? _startRoll = null, float? _endRoll = null, int? _lutResolution = null)
        {
            SetBezier(new(_anchorA, _controlA, _controlB, _anchorB), _startRoll, _endRoll, _lutResolution);
        }

        public void SetResolution(int _lutResolution) => SetBezier(bezier, startRoll, endRoll, _lutResolution);
        #endregion
    }
}