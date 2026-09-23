using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    public interface ITransform<TVector>
    {
        public TVector Position { get; set; }
        public void SetHeading(TVector _direction, bool _flip = false, float _roll = 0f);

        #region Transformations
        /// <summary>
        /// Transforms point from local space to world space.
        /// </summary>
        /// <param name="_position">Point in local space</param>
        /// <returns>Point in world space</returns>
        public TVector TransformPoint(TVector _position);

        /// <summary>
        /// Transforms direction from local space to world space.
        /// </summary>
        /// <param name="_direction">Direction in local space</param>
        /// <returns>Direction in world space</returns>
        public TVector TransformDirection(TVector _direction);

        /// <summary>
        /// Transforms vector from local space to world space.
        /// </summary>
        /// <param name="_vector">Vector in local space</param>
        /// <returns>Vector in world space</returns>
        public TVector TransformVector(TVector _vector);
        #endregion

        #region Inverse Transformations
        /// <summary>
        /// Transforms point from world space to local space.
        /// </summary>
        /// <param name="_position">Point in world space</param>
        /// <returns>Point in local space</returns>
        public TVector InverseTransformPoint(TVector _position);

        /// <summary>
        /// Transforms direction from world space to local space.
        /// </summary>
        /// <param name="_direction">Direction in world space</param>
        /// <returns>Direction in local space</returns>
        public TVector InverseTransformDirection(TVector _direction);
        
        /// <summary>
        /// Transforms vector from world space to local space.
        /// </summary>
        /// <param name="_vector">Vector in world space</param>
        /// <returns>Vector in local space</returns>
        public TVector InverseTransformVector(TVector _vector);
        #endregion

        #region Vector Math
        public float SqrDistanceTo(TVector _vector);
        public float DistanceTo(TVector _vector) => Mathf.Sqrt(SqrDistanceTo(_vector));
        public TVector DirectionTo(TVector _direction);
        #endregion
    }
}