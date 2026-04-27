using UnityEngine;

namespace MaroonSeal.Maths {
    public interface IPointTransform
    {
        public Matrix4x4 ToWorldMatrix { get; }
        public Matrix4x4 ToLocalMatrix { get; }

        #region Transformations
        public Vector3 TransformPosition(Vector3 _position) => ToWorldMatrix.MultiplyPoint(_position);
        public Vector3 InverseTransformPosition(Vector3 _position) => ToLocalMatrix.MultiplyPoint(_position);
        
        public Vector3 TransformVector(Vector3 _vector) => ToWorldMatrix.MultiplyVector(_vector);
        public Vector3 InverseTransformVector(Vector3 _vector) => ToLocalMatrix.MultiplyVector(_vector);

        public Vector3 TransformDirection(Vector3 _direction) => TransformVector(_direction).normalized;
        public Vector3 InverseTransformDirection(Vector3 _direction) => InverseTransformVector(_direction).normalized;
        #endregion
    }
}