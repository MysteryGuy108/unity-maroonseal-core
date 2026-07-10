using System;

using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;

namespace MaroonSeal.Maths.Geometry.Shapes {

    [System.Serializable]
    public struct ConicSection : IShape3D, IPolarShape3D, ISDFShape
    {
        public enum CurveType { Circle, Ellipse, Parabola, Hyperbola }

        [field : SerializeField] public Transform3D Transform { get; set; }
        
        [Space]
        [Min(-1.0f)] public float eccentricity;
        [Min(0.0f)] public float minRadius;
        
        public readonly float SemiLatusRectum { get => minRadius * (1.0f + eccentricity); }
        public readonly float MajorAxis { get => SemiLatusRectum / (1.0f-(eccentricity * eccentricity)); }

        #region Constructors
        public ConicSection(Transform3D _transform, float _eccentricity, float _minRadius)
        {
            Transform = _transform;
            eccentricity = _eccentricity;
            minRadius = _minRadius;
        }
        #endregion

        #region Operators
        public static bool operator ==(ConicSection _a, ConicSection _b)
        {
            return _a.Transform == _b.Transform &&
                _a.eccentricity == _b.eccentricity &&
                _a.minRadius == _b.minRadius;
        }

        public static bool operator != (ConicSection _a, ConicSection _b) {
            return !(_a.Transform == _b.Transform &&
                _a.eccentricity == _b.eccentricity &&
                _a.minRadius == _b.minRadius);
        }
    
        readonly public override bool Equals(object obj) { return ((ConicSection)obj == this) && obj != null && obj is ConicSection; }
        readonly public override int GetHashCode() { return System.HashCode.Combine(Transform, eccentricity, minRadius); }
        #endregion

        #region IShape3D
        readonly public bool Contains(Vector3 _point)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Conic Section
        readonly public float GetBetweenFociDistance() => 2.0f * eccentricity * MajorAxis;

        readonly public (Vector3, Vector3) GetFoci() {
            Vector3 foci2 = GetBetweenFociDistance() * -Vector3.right;
            return (Transform.position, Transform.TransformPosition(foci2));
        }

        readonly public CurveType GetCurveType() {
            if (eccentricity <= 0) { return CurveType.Circle; }
            else if (eccentricity > 0 && eccentricity < 1.0f) { return CurveType.Ellipse; }
            else if (eccentricity == 1.0f) { return CurveType.Parabola; }
            return CurveType.Hyperbola;
        }
        #endregion

        readonly public float GetRadiusAtTheta(float _theta) => SemiLatusRectum / (1.0f + (eccentricity * Mathf.Cos(_theta)));
        readonly public float GetThetaAtRadius(float _radius) => Mathf.Acos((SemiLatusRectum / (_radius * eccentricity)) - (1.0f / eccentricity));

        #region IPolarShape
        readonly public Vector3 EvaluatePointAtTheta(float _theta)
        {
            float radius = GetRadiusAtTheta(_theta);
            Vector3 localPoint = new Vector3(Mathf.Cos(_theta), Mathf.Sin(_theta)) * radius;
            return Transform.TransformPosition(localPoint);
        }

        readonly public Vector3 EvaluateTangentAtTheta(float _theta) {
            Vector3 localPoint = new Vector3(-Mathf.Sin(_theta), Mathf.Cos(_theta) + eccentricity).normalized;
            return Transform.TransformDirection(localPoint);
        }
        #endregion

        #region ICurve
        readonly public Vector3 EvaluatePointAtTime(float _t) {
            return EvaluatePointAtTheta(_t * Mathf.PI * 2.0f);
        }

        readonly public Vector3 EvaluateTangentAtTime(float _t) {
            return EvaluateTangentAtTheta(_t * Mathf.PI * 2.0f);
        }
        #endregion

        #region ISDFShape
        public float GetSignedDistance(Vector3 _sample) {
            _sample = Transform.InverseTransformPosition(_sample);

            throw new NotImplementedException();
        }
        #endregion
    }
}