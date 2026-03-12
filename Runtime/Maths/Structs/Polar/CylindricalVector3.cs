using System;
using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct CylindricalVector3 : IEquatable<CylindricalVector3>
    {
        public float radius;

        public float theta;
        public float ThetaDegrees { 
            readonly get => theta * Mathf.Rad2Deg; 
            set => theta = value * Mathf.Deg2Rad; 
        }

        public float height;

        readonly public Vector3 Vector3Right => 
            new(height,
                Mathf.Sin(theta) * radius,
                Mathf.Cos(theta) * radius);

        readonly public Vector3 Vector3Up => 
            new(Mathf.Cos(theta) * radius, 
                height,
                Mathf.Sin(theta) * radius);
        
        readonly public Vector3 Vector3Forward => 
            new(Mathf.Cos(theta) * radius,
                Mathf.Sin(theta) * radius,
                height);

        #region Constructors
        /// <summary>
        /// Default parametre constructor for SphericalVector3 coordinate.
        /// </summary>
        /// <param name="_radius">Radius of the coordinate</param>
        /// <param name="_theta">Polar angle in radians</param>
        /// <param name="_height">Height along cylinder</param>
        public CylindricalVector3(float _radius, float _theta, float _height) {
            radius = _radius;
            theta = _theta;
            height = _height;
        }

        /// <summary>
        /// Cartesian coordinate conversion constructor for SphericalVector3 coordinate.
        /// </summary>
        /// <param name="_cartesian">Cartesian coordinate</param>
        public CylindricalVector3(Vector3 _cartesian, Vector3? _axisDirection = null) {
            Vector3 axis = _axisDirection ?? Vector3.forward;
            _cartesian = Quaternion.Inverse(Quaternion.LookRotation(axis, Vector3.up)) * _cartesian;

            radius = _cartesian.magnitude;
            theta = Mathf.Atan2(_cartesian.z, _cartesian.x);
            height = _cartesian.y;
        }
        #endregion

        #region Operators
        // +
        static public CylindricalVector3 operator +(CylindricalVector3 _a, CylindricalVector3 _b) => 
            new(_a.radius + _b.radius, _a.theta + _b.theta, _a.height + _b.height);

        // -
        static public CylindricalVector3 operator -(CylindricalVector3 _a, CylindricalVector3 _b) => 
            new(_a.radius - _b.radius, _a.theta - _b.theta, _a.height - _b.height);

        // *
        static public CylindricalVector3 operator *(CylindricalVector3 _a, float _b) => 
            new(_a.radius * _b, _a.theta * _b, _a.height * _b);
        static public CylindricalVector3 operator *(float _b, CylindricalVector3 _a) => 
            _a * _b;

        // /
        static public CylindricalVector3 operator /(CylindricalVector3 _a, float _b) => 
            new(_a.radius / _b, _a.theta / _b, _a.height / _b);

        // ==
        readonly public bool Equals(CylindricalVector3 _other) => 
            this.radius == _other.radius && this.theta == _other.theta;
        public override readonly bool Equals(object obj) => this.Equals((CylindricalVector3)obj);

        public static bool operator ==(CylindricalVector3 _a, CylindricalVector3 _b) => _a.Equals(_b);
        public static bool operator !=(CylindricalVector3 _a, CylindricalVector3 _b) => !_a.Equals(_b);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(this.radius, this.theta, this.height); }
        }
        #endregion
    
        #region Methods
        readonly public Vector3 GetVector3AlongAxis(Vector3 _axis) => Quaternion.LookRotation(_axis, Vector3.up) * Vector3Forward;

        #endregion
    }
}