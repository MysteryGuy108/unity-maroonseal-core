using UnityEngine;

namespace MaroonSeal.Maths {
    public interface ITransform
    {
        #region Transformations
        public Vector3 TransformPosition(Vector3 _position);
        public Vector3 InverseTransformPosition(Vector3 _position);
        
        public Vector3 TransformVector(Vector3 _vector);
        public Vector3 InverseTransformVector(Vector3 _vector);

        public Vector3 TransformDirection(Vector3 _direction);
        public Vector3 InverseTransformDirection(Vector3 _direction);
        #endregion
    }
}