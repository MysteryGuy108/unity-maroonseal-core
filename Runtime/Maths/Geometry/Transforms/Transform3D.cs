using System;
using System.Drawing;
using UnityEngine;

namespace MaroonSeal.Maths {
    /// <summary>
    /// A struct used to represent a transform at a point in 3D space.
    /// </summary>
    [System.Serializable]
    public struct Transform3D : ITransform, IEquatable<Transform3D> {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        #region Matrices
        public readonly Matrix4x4 Matrix => Matrix4x4.TRS(position, rotation, scale);
        public readonly Matrix4x4 InverseMatrix => Matrix.inverse;
        #endregion

        #region Constructors
        public Transform3D(Vector3 _position, Quaternion? _rotation = null, Vector3? _scale = null) {
            position = _position;
            rotation = _rotation ?? Quaternion.identity;
            scale = _scale ?? Vector3.one;
        }

        public Transform3D(Vector3 _position, Vector3 _euler, Vector3? _scale = null) : this(_position, Quaternion.Euler(_euler), _scale) {}

        public Transform3D(Transform _transform) {
            if (_transform == null) { 
                position = Vector3.zero; rotation = Quaternion.identity; scale = Vector3.one; 
                return; 
            }

            position = _transform.position;
            rotation = _transform.rotation;
            scale = _transform.localScale;
        }

        public Transform3D(Matrix4x4 _TRS) {
            position = _TRS.GetColumn(3);
            rotation = Quaternion.LookRotation(_TRS.GetColumn(2), _TRS.GetColumn(1));
            scale = new(_TRS.GetColumn(0).magnitude, _TRS.GetColumn(1).magnitude, _TRS.GetColumn(2).magnitude);
        }
        
        static public Transform3D Origin { get => new(Vector3.zero); }
        #endregion

        #region Orientation
        public Vector3 EulerAngles { 
            readonly get => rotation.eulerAngles;
            set => rotation = Quaternion.Euler(value);
        }

        public Vector3 Forward {
            readonly get => rotation * Vector3.forward;
            set {
                Vector3 forward = value.normalized;
                Vector3 up = Up;

                if (Mathf.Abs(Vector3.Dot(forward, up)) > 0.999f) { up = Vector3.up; }
                rotation = Quaternion.LookRotation(forward, up);
            }
        }

        public Vector3 Up {
            readonly get => rotation * Vector3.up;
            set {
                Vector3 up = value.normalized;
                Vector3 forward = Forward;

                if (Mathf.Abs(Vector3.Dot(forward, up)) > 0.999f) { forward = Vector3.forward; }
                rotation = Quaternion.LookRotation(forward, up);
            }
        }

        public Vector3 Right
        {
            readonly get => rotation * Vector3.right;
            set {
                Vector3 right = value.normalized;
                Vector3 forward = Vector3.Cross(Vector3.up, right);
                if (forward.sqrMagnitude < 1e-6f) { forward = Vector3.Cross(Vector3.forward, right); }

                rotation = Quaternion.LookRotation( forward.normalized, Vector3.Cross(right, forward).normalized);
            }
        }
        #endregion

        #region Operators
        readonly public bool Equals(Transform3D _other) {
            return this.position == _other.position && 
                this.rotation == _other.rotation && 
                this.scale == _other.scale;
        }
        public override readonly bool Equals(object obj) => this.Equals((Transform3D)obj);

        public override readonly int GetHashCode() {
            unchecked {
                return HashCode.Combine(position, EulerAngles, scale);
            }
        }
        public static bool operator ==(Transform3D _a, Transform3D _b) => _a.Equals(_b);
        public static bool operator !=(Transform3D _a, Transform3D _b) => !_a.Equals(_b);
        #endregion

        #region Casting and Conversions
        public static implicit operator Transform2D(Transform3D _transform) => _transform.ConvertToXY();
        
        readonly public Transform2D ConvertToXY() { return new(this.position.FlattenXY(), this.EulerAngles.z, this.scale.FlattenXY()); }
        readonly public Transform2D ConvertToXZ() { return new(this.position.FlattenXZ(), -this.EulerAngles.y, this.scale.FlattenXZ()); }
        #endregion

        #region Point Transform
        readonly public Transform3D GetLocalPoint(Transform3D _transform) {
            Transform3D localisedPoint = new(this.position, this.EulerAngles, this.scale);

            localisedPoint.position = _transform.InverseTransformPosition(localisedPoint.position);
            localisedPoint.Forward = _transform.InverseTransformDirection(localisedPoint.Forward);
            localisedPoint.Up = _transform.InverseTransformDirection(localisedPoint.Up);

            return localisedPoint;
        }

        readonly public Transform3D GetGlobalPoint(Transform3D _transform) {
            Transform3D globalPoint = new(this.position, this.EulerAngles, this.scale);

            globalPoint.position = _transform.TransformPosition(globalPoint.position);
            globalPoint.Forward = _transform.TransformDirection(globalPoint.Forward);
            globalPoint.Up = _transform.TransformDirection(globalPoint.Up);

            return globalPoint;
        }

        readonly public Transform3D GetLocalPoint(Transform _transform) => GetLocalPoint(new Transform3D(_transform));
        readonly public Transform3D GetGlobalPoint(Transform _transform) => GetGlobalPoint(new Transform3D(_transform));
        #endregion

        #region Transformations
        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="_position">Position in local space</param>
        /// <returns>Position in world space</returns>
        readonly public Vector3 TransformPosition(Vector3 _position) => position + rotation * Vector3.Scale(_position, scale);

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="_direction">Direction in local space</param>
        /// <returns>Direction in world space</returns>
        readonly public Vector3 TransformDirection(Vector3 _direction) => rotation * _direction;

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="_vector">Vector in local space</param>
        /// <returns>Vector in world space</returns>
        readonly public Vector3 TransformVector(Vector3 _vector) => rotation * Vector3.Scale(_vector, scale);
        #endregion

        #region Inverse Transformations
        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="_position">Position in world space</param>
        /// <returns>Position in local space</returns>
        readonly public Vector3 InverseTransformPosition(Vector3 _position)
        {
            _position -= position;
            _position = Quaternion.Inverse(rotation) * _position;

            return new Vector3(
                _position.x / scale.x,
                _position.y / scale.y,
                _position.z / scale.z);
        }

        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="_direction">Direction in world space</param>
        /// <returns>Direction in local space</returns>
        readonly public Vector3 InverseTransformDirection(Vector3 _direction) => Quaternion.Inverse(rotation) * _direction;
        
        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="_vector">Vector in world space</param>
        /// <returns>Vector in local space</returns>
        readonly public Vector3 InverseTransformVector(Vector3 _vector)
        {
            _vector = Quaternion.Inverse(rotation) * _vector;

            return new Vector3(
                _vector.x / scale.x,
                _vector.y / scale.y,
                _vector.z / scale.z);
        }

        #endregion
        
        #region Static
        static public Transform3D Lerp(Transform3D _a, Transform3D _b, float _t) {
            return new Transform3D(
                Vector3.Lerp(_a.position, _b.position, _t),
                Quaternion.Lerp(_a.rotation, _b.rotation, _t),
                Vector3.Lerp(_a.scale, _b.scale, _t)
            );
        }
        #endregion
    }
}
