using System;
using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct PointTransform : IPointTransform, IEquatable<PointTransform> {
        public Vector3 position;
        public Vector3 eulerAngles;
        public Vector3 scale;

        #region Orientation
        public Quaternion Rotation { 
            readonly get => Quaternion.Euler(eulerAngles);
            set => eulerAngles = value.eulerAngles;
        }

        public Vector3 Forward { 
            readonly get { return Rotation * Vector3.forward; }
            set { 
                if (value == Vector3.zero || Up == Vector3.zero) { Debug.Log("HERE"); }
                Rotation = Quaternion.LookRotation(value, Up); 
            }
        }

        public Vector3 Right { 
            readonly get { return Rotation * Vector3.right; }
            set { Up = Vector3.Cross(Forward, value); }
        }

        public Vector3 Up { 
            readonly get { return Rotation * Vector3.up; }
            set { Rotation = Quaternion.LookRotation(Forward, value); }
        }
        #endregion

        #region Transformation Matrices
        public readonly Matrix4x4 ToWorldMatrix { 
            get {
                Matrix4x4 transformMatrix = Matrix4x4.identity;
                transformMatrix.SetTRS(position, Rotation, scale);
                return transformMatrix;
            }
        }

        public readonly Matrix4x4 ToLocalMatrix {
            get => ToWorldMatrix.inverse;
        }
        #endregion

        #region Constructors
        public PointTransform(Vector3 _position, Vector3? _eulerAngles = null, Vector3? _scale = null) {
            position = _position;
            eulerAngles = _eulerAngles ?? Vector3.zero;
            scale = _scale ?? Vector3.one;
        }

        public PointTransform(Vector3 _position, Quaternion _rotation, Vector3? _scale = null) {
            position = _position;
            eulerAngles = _rotation.eulerAngles;
            scale = _scale ?? Vector3.one;
        }

        public PointTransform(Transform _transform) {
            if (_transform == null) { position = Vector3.zero; eulerAngles = Vector3.zero; scale = Vector3.one; return; }
            position = _transform.position;
            eulerAngles = _transform.eulerAngles;
            scale = _transform.localScale;
        }

        public PointTransform(Matrix4x4 _TRS) {
            position = _TRS.GetColumn(3);
            eulerAngles = Quaternion.LookRotation(_TRS.GetColumn(2), _TRS.GetColumn(1)).eulerAngles;
            scale = new(_TRS.GetColumn(0).magnitude, _TRS.GetColumn(1).magnitude, _TRS.GetColumn(2).magnitude);
        }
        
        static public PointTransform Origin { get => new(Vector3.zero); }
        #endregion

        #region Operators
        readonly public bool Equals(PointTransform _other) {
            return this.position == _other.position && 
                this.eulerAngles == _other.eulerAngles && 
                this.scale == _other.scale;
        }
        public override readonly bool Equals(object obj) => this.Equals((PointTransform)obj);

        public override readonly int GetHashCode() {
            unchecked {
                return HashCode.Combine(position, eulerAngles, scale);
            }
        }
        public static bool operator ==(PointTransform _a, PointTransform _b) => _a.Equals(_b);
        public static bool operator !=(PointTransform _a, PointTransform _b) => !_a.Equals(_b);
        #endregion

        #region Casting and Conversions
        public static implicit operator PointTransform2D(PointTransform _transform) => _transform.ConvertToXY();
        
        readonly public PointTransform2D ConvertToXY() { return new(this.position.FlattenXY(), this.eulerAngles.z, this.scale.FlattenXY()); }
        readonly public PointTransform2D ConvertToXZ() { return new(this.position.FlattenXZ(), -this.eulerAngles.y, this.scale.FlattenXZ()); }
        #endregion

        #region Point Transform
        readonly public PointTransform GetLocalPoint(PointTransform _transform) {
            PointTransform localisedPoint = new(this.position, this.eulerAngles, this.scale);

            localisedPoint.position = _transform.InverseTransformPosition(localisedPoint.position);
            localisedPoint.Forward = _transform.InverseTransformDirection(localisedPoint.Forward);
            localisedPoint.Up = _transform.InverseTransformDirection(localisedPoint.Up);

            return localisedPoint;
        }

        readonly public PointTransform GetGlobalPoint(PointTransform _transform) {
            PointTransform globalPoint = new(this.position, this.eulerAngles, this.scale);

            globalPoint.position = _transform.TransformPosition(globalPoint.position);
            globalPoint.Forward = _transform.TransformDirection(globalPoint.Forward);
            globalPoint.Up = _transform.TransformDirection(globalPoint.Up);

            return globalPoint;
        }

        readonly public PointTransform GetLocalPoint(Transform _transform) => GetLocalPoint(new PointTransform(_transform));
        readonly public PointTransform GetGlobalPoint(Transform _transform) => GetGlobalPoint(new PointTransform(_transform));
        #endregion

        #region Transformations
        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector3 TransformPosition(Vector3 _position) => ToWorldMatrix.MultiplyPoint(_position);
        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector3 InverseTransformPosition(Vector3 _position) => ToLocalMatrix.MultiplyPoint(_position);

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector3 TransformVector(Vector3 _vector) => ToWorldMatrix.MultiplyVector(_vector);
        
        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector3 InverseTransformVector(Vector3 _vector) => ToLocalMatrix.MultiplyVector(_vector);

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector3 TransformDirection(Vector3 _direction) => TransformVector(_direction).normalized;
        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector3 InverseTransformDirection(Vector3 _direction) => InverseTransformVector(_direction).normalized;
        #endregion

        #region Static
        static public PointTransform Lerp(PointTransform _a, PointTransform _b, float _t) {
            return new PointTransform(
                Vector3.Lerp(_a.position, _b.position, _t),
                Quaternion.Lerp(_a.Rotation, _b.Rotation, _t),
                Vector3.Lerp(_a.scale, _b.scale, _t)
            );
        }
        #endregion
    }
}
