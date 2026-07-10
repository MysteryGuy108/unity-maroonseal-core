using System;
using UnityEngine;

namespace MaroonSeal.Maths {
    /// <summary>
    /// A struct used to represent a transform at a point in 2D space.
    /// </summary>
    [System.Serializable]
    public struct Transform2D : ITransform
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
        public Transform2D(Vector2 _position, float? _angle = null, Vector2? _scale = null) {
            position = _position;
            angle = _angle ?? 0.0f;
            scale = _scale ?? Vector2.one;
        }

        public Transform2D(Vector2 _position, Quaternion _rotation, Vector2? _scale = null) {
            position = _position;
            angle = _rotation.eulerAngles.z;
            scale = _scale ?? Vector2.one;
        }

        public Transform2D(Transform _transform, bool _worldSpace = true) {
            position = _worldSpace ? _transform.position : _transform.localPosition;
            angle = _worldSpace ? _transform.eulerAngles.z : _transform.localEulerAngles.z;
            scale = _transform.localScale;
        }

        public Transform2D(Matrix4x4 _TRS) {
            position = _TRS.GetColumn(3);
            angle = Quaternion.LookRotation(_TRS.GetColumn(2), _TRS.GetColumn(1)).eulerAngles.z;
            scale = new(_TRS.GetColumn(0).magnitude, _TRS.GetColumn(1).magnitude);
        }

        static public Transform2D Origin => new(Vector2.zero);
        #endregion

        #region Operators
        readonly public bool Equals(Transform2D _other) {
            return this.position == _other.position && 
                this.angle == _other.angle && 
                this.scale == _other.scale;
        }
        public override readonly bool Equals(object obj) => this.Equals((Transform2D)obj);

        public override readonly int GetHashCode() {
            unchecked {
                return HashCode.Combine(position, angle, scale);
            }
        }
        public static bool operator ==(Transform2D _a, Transform2D _b) => _a.Equals(_b);
        public static bool operator !=(Transform2D _a, Transform2D _b) => !_a.Equals(_b);
        #endregion

        #region ITransform
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
        readonly public Transform3D ToXY() { 
            return new Transform3D(this.position, this.Rotation, new(this.scale.x, this.scale.y, 1.0f)); 
        }

        readonly public Transform3D ToXZ() {
            Vector3 position = this.position.ToXZ();
            Quaternion rotation = Quaternion.Euler(0.0f, -this.angle, 0.0f);
            Vector3 scale = this.scale.ToXZ();
            scale.y = 1.0f;
            return new(position, rotation, scale);
        }
        #endregion

        #region Transformations
        readonly public Vector2 TransformPosition(Vector2 _position) => ToWorldMatrix.MultiplyPoint(_position.ToXY());
        readonly public Vector2 InverseTransformPosition(Vector2 _position) => ToLocalMatrix.MultiplyPoint(_position.ToXY());
        readonly public Vector2 TransformVector(Vector2 _vector) => ToWorldMatrix.MultiplyVector(_vector.ToXY());
        readonly public Vector2 InverseTransformVector(Vector2 _vector) => ToLocalMatrix.MultiplyVector(_vector.ToXY());
        readonly public Vector2 TransformDirection(Vector2 _direction) => TransformVector(_direction.ToXY()).normalized;
        readonly public Vector2 InverseTransformDirection(Vector2 _direction) => InverseTransformVector(_direction.ToXY()).normalized;
        #endregion

        #region IPointTransform
        readonly public Vector3 TransformPosition(Vector3 _position) => TransformPosition((Vector2)_position);
        readonly public Vector3 InverseTransformPosition(Vector3 _position) => TransformPosition((Vector2)_position);
        
        readonly public Vector3 TransformVector(Vector3 _vector) => TransformPosition((Vector2)_vector);
        readonly public Vector3 InverseTransformVector(Vector3 _vector) => TransformPosition((Vector2)_vector);

        readonly public Vector3 TransformDirection(Vector3 _direction) => TransformPosition((Vector2)_direction);
        readonly public Vector3 InverseTransformDirection(Vector3 _direction) => TransformPosition((Vector2)_direction);
        #endregion
    }
}