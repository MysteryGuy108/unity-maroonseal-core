using System;
using System.Collections.Generic;
using UnityEngine;


namespace MaroonSeal.Maths.Geometry.Shapes {
    [System.Serializable]
    public struct Circle2D : IShape2D, IPolarShape
    {
        public PointTransform2D transform;
        readonly public PointTransform2D Transform => transform;

        [Min(0.0f)]public float radius;

        #region Constructors
        public Circle2D(PointTransform2D _transform, float _radius) {
            transform = _transform;
            radius = _radius;
        }

        public Circle2D(Vector2 _position, float _radius) {
            transform = new(_position);
            radius = _radius;
        }

        public Circle2D(float _radius) {
            transform = PointTransform2D.Origin;
            radius = _radius;
        }
        #endregion

        #region Operators
        readonly public bool Equals(Circle2D _other) {
            return this.transform == _other.transform && 
                this.radius == _other.radius;
        }
        public override readonly bool Equals(object obj) => this.Equals((Circle2D)obj);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(transform, radius); }
        }
        public static bool operator ==(Circle2D _a, Circle2D _b) => _a.Equals(_b);
        public static bool operator !=(Circle2D _a, Circle2D _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static explicit operator Circle(Circle2D _circle2D) => new(_circle2D.transform.ToXY(), _circle2D.radius);
        #endregion

        #region Shape2D
        public void Rotate(float _rotation) =>
            transform.angle += _rotation;

        public void Translate(Vector2 _translation) =>
            transform.position += _translation;
        #endregion

        #region Circle2D
        readonly public bool IsPositionInRadius(Vector2 _position)
        {
            return this.transform.InverseTransformPosition(_position).magnitude <= radius;
        }
        #endregion

        #region IPolarSpaceShape
        readonly public Vector2 EvaluatePositionAtTheta(float _radians) => IPolarShape.GetCartesianPosition(transform, radius, _radians);
        readonly public Vector2 EvaluateTangentAtTheta(float _radians) => transform.TransformDirection(IPolarShape.GetCircleTangent(_radians));   

        readonly public Vector2 EvaluatePositionAtTime(float _t) => EvaluatePositionAtTheta(_t * Mathf.PI * 2.0f);
        readonly public Vector2 EvaluateTangentAtTime(float _t) => EvaluateTangentAtTheta(_t * Mathf.PI * 2.0f);

        readonly Vector3 IPolarShape.EvaluatePositionAtTheta(float _radians) => this.EvaluatePositionAtTheta(_radians);
        readonly Vector3 IPolarShape.EvaluateTangentAtTheta(float _radians) => this.EvaluateTangentAtTheta(_radians);

        readonly Vector3 IInterpolationShape.EvaluatePositionAtTime(float _t) => this.EvaluatePositionAtTime(_t);
        readonly Vector3 IInterpolationShape.EvaluateTangentAtTime(float _t) => this.EvaluateTangentAtTime(_t);
        #endregion

        #region Static Operations
        //https://gieseanw.wordpress.com/2012/09/12/finding-external-tangent-points-for-two-circles/
        static public (float, float, float, float) FindTangentThetas(Circle2D _from, Circle2D _to) {
            Vector2 delta = _to.transform.position - _from.transform.position;
            float thetaTo = Mathf.Atan2(delta.y, delta.x);

            float distance = delta.magnitude;
            float distance2 = distance * distance;

            float R1 = _from.radius;
            float R2 = _to.radius;

            float rDelta = R1 - R2;

            float H = Mathf.Sqrt(distance2 - (rDelta * rDelta));
            float Y = Mathf.Sqrt((H*H) + (R2*R2));

            float thetaA = Mathf.Acos(((R1*R1) + distance2 - (Y*Y)) / (2*R1*distance));
            float thetaB = Mathf.Acos((R1 + R2) / distance);

            float tangentThetaA = thetaTo + thetaA;
            float tangentThetaB = thetaTo + thetaB;
            float tangentThetaC = thetaTo + (Mathf.PI * 2.0f) - thetaA;
            float tangentThetaD = thetaTo + (Mathf.PI *2.0f) - thetaB;

            float thetaOffset = _from.transform.angle * Mathf.Deg2Rad;

            tangentThetaA -= thetaOffset;
            tangentThetaB -= thetaOffset;
            tangentThetaC -= thetaOffset;
            tangentThetaD -= thetaOffset;

            return new(tangentThetaA, tangentThetaB, tangentThetaC, tangentThetaD);
        }

        static public (Vector2, Vector2, Vector2, Vector2) FindTangentPoints(Circle2D _from, Circle2D _to) {
            (float, float, float , float) thetas = FindTangentThetas(_from, _to);

            Vector2 pointA = _from.EvaluatePositionAtTheta(thetas.Item1);
            Vector2 pointB = _from.EvaluatePositionAtTheta(thetas.Item2);
            Vector2 pointC = _from.EvaluatePositionAtTheta(thetas.Item3);
            Vector2 pointD = _from.EvaluatePositionAtTheta(thetas.Item4);

            return new(pointA, pointB, pointC, pointD);
        }

        static public (Line2D, Line2D, Line2D, Line2D) FindTangentLines(Circle2D _from, Circle2D _to) {
            (Vector2, Vector2, Vector2, Vector2) fromPoints = FindTangentPoints(_from, _to);
            (Vector2, Vector2, Vector2, Vector2) toPoints = FindTangentPoints(_to, _from);

            return new(
                new Line2D(fromPoints.Item1, toPoints.Item3),
                new Line2D(fromPoints.Item2, toPoints.Item2),
                new Line2D(fromPoints.Item3, toPoints.Item1),
                new Line2D(fromPoints.Item4, toPoints.Item4)
            );
        }
        #endregion
    }
}