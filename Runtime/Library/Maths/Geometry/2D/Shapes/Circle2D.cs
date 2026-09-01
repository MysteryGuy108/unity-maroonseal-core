using System;
using System.Collections.Generic;

using UnityEngine;


namespace MaroonSeal.Maths.Geometry {
    [System.Serializable]
    public struct Circle2D : IShape2D, IPolarShape2D, IEquatable<Circle2D>
    {
        [field : SerializeField] public Transform2D Transform {get; set; }
        [Min(0.0f)]public float radius;

        readonly public float Circumference => radius * Mathf.PI * 2.0f;

        #region Constructors
        public Circle2D(Transform2D _transform, float _radius) {
            Transform = _transform;
            radius = _radius;
        }
        
        public Circle2D(Vector2 _position, float _radius) : this(new Transform2D(_position), _radius) {}
        public Circle2D(float _radius) : this(Transform2D.Origin, _radius) {}
       
        public static Circle2D UnitCircle => new(Vector2.zero, 1.0f);
        #endregion

        #region IEquatable
        readonly public bool Equals(Circle2D _other) => this.Transform == _other.Transform && this.radius == _other.radius;
        public override readonly bool Equals(object obj) => obj != null && obj is Circle2D && ((Circle2D)obj).Equals(this);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(Transform, radius); }
        }
        #endregion

        #region Operators
        public static bool operator ==(Circle2D _a, Circle2D _b) => _a.Equals(_b);
        public static bool operator !=(Circle2D _a, Circle2D _b) => !_a.Equals(_b);
        #endregion

        #region Casting
        public static explicit operator Circle(Circle2D _circle2D) => new(_circle2D.Transform.ToXY(), _circle2D.radius);
        #endregion

        #region IShape2D
        readonly public bool ContainsPoint(Vector2 _position) => this.Transform.InverseTransformPoint(_position).magnitude <= radius;
        #endregion

        #region IPolarSpaceShape
        readonly public Vector2 EvaluatePointAtTheta(float _radians) => Transform.TransformPoint(Vector2Maths.FromRadians(_radians, radius));
        readonly public Vector2 EvaluateTangentAtTheta(float _radians) => Transform.TransformDirection(GetTangentAtTheta(_radians));   

        readonly public Vector2 EvaluatePointAtTime(float _t) => EvaluatePointAtTheta(_t * Mathf.PI * 2.0f);
        readonly public Vector2 EvaluateTangentAtTime(float _t) => EvaluateTangentAtTheta(_t * Mathf.PI * 2.0f);
        #endregion

        #region Static Operations
        static public Vector2 GetTangentAtTheta(float _radians)
        {
            _radians += Mathf.PI*0.5f;
            return new Vector2(Mathf.Cos(_radians), Mathf.Sin(_radians)); 
        }

        //https://gieseanw.wordpress.com/2012/09/12/finding-external-tangent-points-for-two-circles/
        static public (float, float, float, float) FindTangentThetas(Circle2D _from, Circle2D _to) {
            Vector2 delta = _to.Transform.position - _from.Transform.position;
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

            float thetaOffset = _from.Transform.angle * Mathf.Deg2Rad;

            tangentThetaA -= thetaOffset;
            tangentThetaB -= thetaOffset;
            tangentThetaC -= thetaOffset;
            tangentThetaD -= thetaOffset;

            return new(tangentThetaA, tangentThetaB, tangentThetaC, tangentThetaD);
        }

        static public (Vector2, Vector2, Vector2, Vector2) FindTangentPoints(Circle2D _from, Circle2D _to) {
            (float, float, float , float) thetas = FindTangentThetas(_from, _to);

            Vector2 pointA = _from.EvaluatePointAtTheta(thetas.Item1);
            Vector2 pointB = _from.EvaluatePointAtTheta(thetas.Item2);
            Vector2 pointC = _from.EvaluatePointAtTheta(thetas.Item3);
            Vector2 pointD = _from.EvaluatePointAtTheta(thetas.Item4);

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