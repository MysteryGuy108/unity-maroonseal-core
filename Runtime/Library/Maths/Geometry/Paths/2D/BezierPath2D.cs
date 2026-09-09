using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    sealed public class BezierPath2D : SamplePath2D
    {
        public override bool IsLoop => false;

        [SerializeField] private CubicBezier2D bezier;

        #region Constructors
        public BezierPath2D() : base() {}
        public BezierPath2D(CubicBezier2D _bezier) : base() => bezier = _bezier;
        #endregion

        #region GeometryPath
        public override Transform2D EvaluateTime(float _t)
        {
            Vector3 position = bezier.EvaluatePointAtTime(_t);
            Vector3 forward = bezier.EvaluateTangentAtTime(_t);

            forward = (forward == Vector3.zero ? Vector3.forward : forward).normalized;

            return new(position) { Right = forward};
        }
        #endregion

        #region Bezier Path
        public CubicBezier2D GetBezier() => bezier;

        public void SetBezier(CubicBezier2D _bezier)
        {
            bezier = _bezier;
            if (bezier != _bezier) { SetDirty(); }
        }

        public void SetBezierPoints(Vector2 _anchorA, Vector2 _controlA, Vector2 _controlB, Vector2 _anchorB)
        {
            SetBezier(new(_anchorA, _controlA, _controlB, _anchorB));
        }
        #endregion
    }
}
