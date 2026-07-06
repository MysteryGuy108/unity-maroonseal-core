using UnityEngine;

namespace MaroonSeal.Maths.Geometry.SDFs {

    #region 3D
    public interface ISDFShape {
        public float GetSignedDistance(Vector3 _point);
    }

    public static class ISDFShapeExtensions {
        public static float GetSignedDistance(this ISDFShape _shape, Transform _shapeTransform, Vector3 _globalPoint) {
            if (_shapeTransform) { _globalPoint = _shapeTransform.InverseTransformPoint(_globalPoint); }
            return  _shape.GetSignedDistance(_globalPoint);
        }
    }
    #endregion


}