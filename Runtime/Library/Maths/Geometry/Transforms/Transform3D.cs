using System;
using System.Drawing;
using UnityEngine;

namespace MaroonSeal.Maths {
    /// <summary>
    /// A struct used to represent a transform at a point in 3D space.
    /// </summary>
    [System.Serializable]
    public struct Transform3D : ITransform<Vector3>, IEquatable<Transform3D> {
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

        public Transform3D(Transform _transform, bool _worldSpace = true) {
            if (_transform == null) { 
                position = Vector3.zero; rotation = Quaternion.identity; scale = Vector3.one;
            }
            else if (_worldSpace)
            {
                position = _transform.position;
                rotation = _transform.rotation;
                scale = _transform.localScale;
            }
            else
            {
                position = _transform.localPosition;
                rotation = _transform.localRotation;
                scale = _transform.localScale;
            }
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

        #region IEquatable
        readonly public bool Equals(Transform3D _other) {
            return this.position == _other.position && 
                this.rotation == _other.rotation && 
                this.scale == _other.scale;
        }
        public override readonly bool Equals(object obj) => this.Equals((Transform3D)obj);

        public override readonly int GetHashCode() {
            unchecked { return HashCode.Combine(position, rotation, scale); }
        }
        #endregion

        #region Operators
        public static bool operator ==(Transform3D _a, Transform3D _b) => _a.Equals(_b);
        public static bool operator !=(Transform3D _a, Transform3D _b) => !_a.Equals(_b);
        #endregion

        #region Casting and Conversions
        public static implicit operator Transform2D(Transform3D _transform) => _transform.ToXY();
        
        readonly public Transform2D ToXY() { return new(this.position.ToXY(), this.EulerAngles.z, this.scale.ToXY()); }
        readonly public Transform2D ToXZ() { return new(this.position.ToXZ(), -this.EulerAngles.y, this.scale.ToXZ()); }
        #endregion

        #region Point Transform
        readonly public Transform3D GetLocalTransform(Transform3D _transform) {
            Transform3D localisedPoint = new(this.position, this.EulerAngles, this.scale);

            localisedPoint.position = _transform.InverseTransformPoint(localisedPoint.position);
            localisedPoint.Forward = _transform.InverseTransformDirection(localisedPoint.Forward);
            localisedPoint.Up = _transform.InverseTransformDirection(localisedPoint.Up);

            return localisedPoint;
        }

        readonly public Transform3D GetGlobalTransform(Transform3D _transform) {
            Transform3D globalPoint = new(this.position, this.EulerAngles, this.scale);

            globalPoint.position = _transform.TransformPoint(globalPoint.position);
            globalPoint.Forward = _transform.TransformDirection(globalPoint.Forward);
            globalPoint.Up = _transform.TransformDirection(globalPoint.Up);

            return globalPoint;
        }

        readonly public Transform3D GetLocalTransform(Transform _transform) => GetLocalTransform(new Transform3D(_transform));
        readonly public Transform3D GetGlobalTransform(Transform _transform) => GetGlobalTransform(new Transform3D(_transform));
        #endregion

        #region Transformations
        readonly public Vector3 TransformPoint(Vector3 _point) => position + rotation * Vector3.Scale(_point, scale);
        readonly public Vector3 TransformDirection(Vector3 _direction) => rotation * _direction;
        readonly public Vector3 TransformVector(Vector3 _vector) => rotation * Vector3.Scale(_vector, scale);
        #endregion

        #region Inverse Transformations
        readonly public Vector3 InverseTransformPoint(Vector3 _point)
        {
            _point = Quaternion.Inverse(rotation) * (_point - position);
            return new Vector3(_point.x / scale.x, _point.y / scale.y, _point.z / scale.z);
        }
        readonly public Vector3 InverseTransformDirection(Vector3 _direction) => Quaternion.Inverse(rotation) * _direction;
        readonly public Vector3 InverseTransformVector(Vector3 _vector)
        {
            _vector = Quaternion.Inverse(rotation) * _vector;
            return new Vector3(_vector.x / scale.x, _vector.y / scale.y, _vector.z / scale.z);
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
