using UnityEngine;

namespace MaroonSeal.Maths {
    public interface IPointTransform
    {
        public Matrix4x4 ToWorldMatrix { get; }
        public Matrix4x4 ToLocalMatrix { get; }

        #region Transformations
        /// <summary>
        /// Transforms position from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        public Vector3 TransformPosition(Vector3 _position) => ToWorldMatrix.MultiplyPoint(_position);
        /// <summary>
        /// Transforms position from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        public Vector3 InverseTransformPosition(Vector3 _position) => ToLocalMatrix.MultiplyPoint(_position);

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        public Vector3 TransformVector(Vector3 _vector) => ToWorldMatrix.MultiplyVector(_vector);
        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        public Vector3 InverseTransformVector(Vector3 _vector) => ToLocalMatrix.MultiplyVector(_vector);

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        public Vector3 TransformDirection(Vector3 _direction) => TransformVector(_direction).normalized;
        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="_position"></param>
        /// <returns></returns>
        public Vector3 InverseTransformDirection(Vector3 _direction) => InverseTransformVector(_direction).normalized;
        #endregion

    }
}