using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct PolarVector2 : IEquatable<PolarVector2>
    {
        public float radius;
        public float theta;

        public float Degrees { 
            readonly get => theta * Mathf.Rad2Deg; 
            set => theta = value * Mathf.Deg2Rad;
        }

        readonly public float ArcLength => radius * theta;

        readonly public Vector2 Vector2 => 
            new(Mathf.Cos(theta) * radius, Mathf.Sin(theta) * radius); 
        
        #region Constructors
        /// <summary>
        /// Default parametre constructor for PolarVector2 coordinate.
        /// </summary>
        /// <param name="_radius">Radius of polar coordinate</param>
        /// <param name="_theta">Polar angle theta in radians</param>
        public PolarVector2(float _radius, float _theta) {
            theta = Mathf.Repeat(_theta, Mathf.PI * 2.0f);
            radius = _radius;
        }

        /// <summary>
        /// Cartesian coordinate conversion constructor for PolarVector2 coordinate.
        /// </summary>
        /// <param name="_cartesian">Cartesian coordinate</param>
        public PolarVector2(Vector2 _position) {
            radius = _position.magnitude;
            theta = Mathf.Atan2(_position.y, _position.x);
        }
        #endregion

        #region Operators
        static public PolarVector2 operator +(PolarVector2 _a, PolarVector2 _b) => new(_a.radius + _b.radius, _a.theta + _b.theta);
        static public PolarVector2 operator -(PolarVector2 _a, PolarVector2 _b) => new(_a.radius - _b.radius, _a.theta - _b.theta);

        static public PolarVector2 operator *(PolarVector2 _a, float _b) => new(_a.radius * _b, _a.theta * _b);
        static public PolarVector2 operator *(float _b, PolarVector2 _a) => _a * _b;

        static public PolarVector2 operator /(PolarVector2 _a, float _b) => new(_a.radius / _b, _a.theta / _b);

        readonly public bool Equals(PolarVector2 _other) => this.radius == _other.radius && this.theta == _other.theta;
        public override readonly bool Equals(object obj) => this.Equals((PolarVector2)obj);

        public static bool operator ==(PolarVector2 _a, PolarVector2 _b) => _a.Equals(_b);
        public static bool operator !=(PolarVector2 _a, PolarVector2 _b) => !_a.Equals(_b);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(this.radius, this.theta); }
        }
        #endregion

        #region Methods

        #endregion

        #region Distances
        static public float CartesianDistance(PolarVector2 _a, PolarVector2 _b) => Vector2.Distance(_a.Vector2, _b.Vector2);

        static public float LinearRadialDistance(PolarVector2 _a, PolarVector2 _b) => _a.radius + _b.radius;

        static public float PolarDistance(PolarVector2 _a, PolarVector2 _b) {
            float angleDelta = Mathf.Abs(Mathf.DeltaAngle(_a.Degrees, _b.Degrees)) * Mathf.Deg2Rad;
            float radiusDelta = Mathf.Abs(_a.radius - _b.radius);
            return angleDelta + radiusDelta;
        }
        #endregion

        #region Lerp
        static public PolarVector2 Lerp(PolarVector2 _a, PolarVector2 _b, float _t, bool _angularLerp = true) {
            float lerpTheta;
            if (_angularLerp) { lerpTheta = Mathf.LerpAngle(_a.Degrees, _b.Degrees, _t) * Mathf.Deg2Rad; }
            else { lerpTheta = Mathf.Lerp(_a.theta, _b.theta, _t); }
            
            float lerpRadius = Mathf.Lerp(_a.radius, _b.radius, _t);

            return new PolarVector2(lerpRadius, lerpTheta);
        }

        static public float InverseLerp(PolarVector2 _a, PolarVector2 _b, PolarVector2 _t) {
            float inverseLerpTheta = Mathf.InverseLerp(_a.theta, _b.theta, _t.theta);
            float inverseLerpRadius = Mathf.InverseLerp(_a.radius, _b.radius, _t.radius);

            return (inverseLerpTheta + inverseLerpRadius) / 2.0f;
        }
        #endregion

        #region Move Towards
        static public PolarVector2 MoveTowardsTarget(PolarVector2 _current, PolarVector2 _target, float _deltaTheta, float _deltaRadius, bool _angularStep) {
            float newTheta;
            if (_angularStep) { newTheta = Mathf.MoveTowardsAngle(_current.Degrees, _target.Degrees, _deltaTheta * Mathf.Rad2Deg) * Mathf.Deg2Rad; }
            else { newTheta = Mathf.MoveTowards(_current.theta, _target.theta, _deltaTheta); }

            float newRadius = Mathf.MoveTowards(_current.radius, _target.radius, _deltaRadius);

            return new PolarVector2(newRadius, newTheta);
        }

        static public PolarVector2 MoveTowardsTargetThroughOrigin(PolarVector2 _current, PolarVector2 _target, float _stepDelta) {
            if (_current.theta == _target.theta || _current.radius <= 0.0f) {
                return new PolarVector2(_target.theta, Mathf.MoveTowards(_current.radius, _target.radius, _stepDelta));
            }

            float newTheta = _current.theta;
            float newRadius = _current.radius - _stepDelta;
            if (newRadius <= 0.0f) { newTheta = _target.theta; }

            newRadius = Mathf.Abs(newRadius);

            return new PolarVector2(newRadius, newTheta);
        }
        #endregion


    }
}

