using System;
using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct PointTransform2D : IPointTransform
    {
        public Vector2 position;
        public float angle;
        public Vector2 scale;

        #region Orientation
        public Quaternion Rotation { 
            readonly get => Quaternion.Euler(0.0f, 0.0f, angle);
            set => angle = value.eulerAngles.z;
        }

        public Vector2 Right { 
            readonly get { return Rotation * Vector2.right; }
            set { Up = Vector3.Cross(Vector3.forward, value); }
        }

        public Vector2 Up { 
            readonly get { return Rotation * Vector2.up; }
            set { Rotation = Quaternion.LookRotation(Vector3.forward, value); }
        }
        #endregion

        #region Constructors
        public PointTransform2D(Vector2 _position, float? _angle = null, Vector2? _scale = null) {
            position = _position;
            angle = _angle ?? 0.0f;
            scale = _scale ?? Vector2.one;
        }

        public PointTransform2D(Vector2 _position, Quaternion _rotation, Vector2? _scale = null) {
            position = _position;
            angle = _rotation.eulerAngles.z;
            scale = _scale ?? Vector2.one;
        }

        public PointTransform2D(Transform _transform, bool _worldSpace = true) {
            position = _worldSpace ? _transform.position : _transform.localPosition;
            angle = _worldSpace ? _transform.eulerAngles.z : _transform.localEulerAngles.z;
            scale = _transform.localScale;
        }

        public PointTransform2D(Matrix4x4 _TRS) {
            position = _TRS.GetColumn(3);
            angle = Quaternion.LookRotation(_TRS.GetColumn(2), _TRS.GetColumn(1)).eulerAngles.z;
            scale = new(_TRS.GetColumn(0).magnitude, _TRS.GetColumn(1).magnitude);
        }

        static public PointTransform2D Origin => new(Vector2.zero);
        #endregion

        #region Operators
        readonly public bool Equals(PointTransform2D _other) {
            return this.position == _other.position && 
                this.angle == _other.angle && 
                this.scale == _other.scale;
        }
        public override readonly bool Equals(object obj) => this.Equals((PointTransform2D)obj);

        public override readonly int GetHashCode() {
            unchecked {
                return HashCode.Combine(position, angle, scale);
            }
        }
        public static bool operator ==(PointTransform2D _a, PointTransform2D _b) => _a.Equals(_b);
        public static bool operator !=(PointTransform2D _a, PointTransform2D _b) => !_a.Equals(_b);
        #endregion

        #region IPointTransform
        public readonly Matrix4x4 ToWorldMatrix { 
            get {
                Matrix4x4 transformMatrix = Matrix4x4.identity;
                Vector3 adjustedScale = scale;
                adjustedScale.z = 1.0f;
                transformMatrix.SetTRS(position, Rotation, adjustedScale);
                return transformMatrix;
            }
        }

        public readonly Matrix4x4 ToLocalMatrix => ToWorldMatrix.inverse;
        #endregion

        #region Conversions
        readonly public PointTransform ToXY() { 
            return new PointTransform(this.position, this.Rotation, new(this.scale.x, this.scale.y, 1.0f)); 
        }

        readonly public PointTransform ToXZ() {
            Vector3 position = this.position.ToXZ();
            Quaternion rotation = Quaternion.Euler(0.0f, -this.angle, 0.0f);
            Vector3 scale = this.scale.ToXZ();
            scale.y = 1.0f;
            return new(position, rotation, scale);
        }
        #endregion

        #region Transformations
        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector2 TransformPosition(Vector2 _position) => ToWorldMatrix.MultiplyPoint(_position.ToXY());
        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector2 InverseTransformPosition(Vector2 _position) => ToLocalMatrix.MultiplyPoint(_position.ToXY());

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector2 TransformVector(Vector2 _vector) => ToWorldMatrix.MultiplyVector(_vector.ToXY());
        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector2 InverseTransformVector(Vector2 _vector) => ToLocalMatrix.MultiplyVector(_vector.ToXY());

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector2 TransformDirection(Vector2 _direction) => TransformVector(_direction.ToXY()).normalized;
        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        readonly public Vector2 InverseTransformDirection(Vector2 _direction) => InverseTransformVector(_direction.ToXY()).normalized;
        #endregion
    }
}