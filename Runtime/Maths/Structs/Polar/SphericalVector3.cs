using System;
using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct SphericalVector3 : IEquatable<SphericalVector3>
    {
        public float radius;

        public float theta;
        public float ThetaDegrees { 
            readonly get => theta * Mathf.Rad2Deg; 
            set => theta = value * Mathf.Deg2Rad; 
        }

        public float phi;
        public float PhiDegrees { 
            readonly get => phi * Mathf.Rad2Deg; 
            set => phi = value * Mathf.Deg2Rad; 
        }

        readonly public Vector3 Vector3 => 
            new(Mathf.Cos(theta) * Mathf.Cos(phi)* radius, 
                Mathf.Sin(phi)* radius, 
                Mathf.Sin(theta) * Mathf.Cos(phi)* radius); 

        #region Constructors
        /// <summary>
        /// Default parametre constructor for SphericalVector3 coordinate.
        /// </summary>
        /// <param name="_radius">Radius of the coordinate</param>
        /// <param name="_theta">Polar angle in radians</param>
        /// <param name="_phi">Azimuthal angle in radians</param>
        public SphericalVector3(float _radius, float _theta, float _phi) {
            radius = _radius;
            theta = _theta;
            phi = _phi;
        }

        /// <summary>
        /// Cartesian coordinate conversion constructor for SphericalVector3 coordinate.
        /// </summary>
        /// <param name="_cartesian">Cartesian coordinate</param>
        public SphericalVector3(Vector3 _cartesian) {
            radius = _cartesian.magnitude;
            theta = Mathf.Atan2(_cartesian.z, _cartesian.x);
            phi = Mathf.Acos(_cartesian.y / radius);
        }
        #endregion

        #region Operators
        // +
        static public SphericalVector3 operator +(SphericalVector3 _a, SphericalVector3 _b) => 
            new(_a.radius + _b.radius, _a.theta + _b.theta, _a.phi + _b.phi);

        // -
        static public SphericalVector3 operator -(SphericalVector3 _a, SphericalVector3 _b) => 
            new(_a.radius - _b.radius, _a.theta - _b.theta, _a.phi - _b.phi);

        // *
        static public SphericalVector3 operator *(SphericalVector3 _a, float _b) => 
            new(_a.radius * _b, _a.theta * _b, _a.phi * _b);

        static public SphericalVector3 operator *(float _b, SphericalVector3 _a) => 
            _a * _b;

        // /
        static public SphericalVector3 operator /(SphericalVector3 _a, float _b) => 
            new(_a.radius / _b, _a.theta / _b, _a.phi / _b);

        // ==
        readonly public bool Equals(SphericalVector3 _other) => this.radius == _other.radius && this.theta == _other.theta;
        public override readonly bool Equals(object obj) => this.Equals((SphericalVector3)obj);

        public static bool operator ==(SphericalVector3 _a, SphericalVector3 _b) => _a.Equals(_b);
        public static bool operator !=(SphericalVector3 _a, SphericalVector3 _b) => !_a.Equals(_b);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(this.radius, this.theta, this.phi); }
        }
        #endregion
    }
}