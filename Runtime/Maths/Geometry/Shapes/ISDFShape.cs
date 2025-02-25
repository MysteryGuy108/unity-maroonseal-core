using UnityEngine;

namespace MaroonSeal.Maths {

    #region 3D
    public interface ISDFShape3D {
        public float GetSignedDistance(Vector3 _point);
    }

    public static class SDFShape3DExtensions {
        public static float GetSignedDistance(this ISDFShape2D _shape, Transform _shapeTransform, Vector3 _globalPoint) {
            if (_shapeTransform) { _globalPoint = _shapeTransform.InverseTransformPoint(_globalPoint); }
            return  _shape.GetSignedDistance(_globalPoint);
        }
    }
    #endregion

    #region 2D
    public interface ISDFShape2D {
        public float GetSignedDistance(Vector2 _point);
    }

    public static class SDFShape2DExtensions {
        public static float GetSignedDistance(this ISDFShape2D _shape, Transform _shapeTransform, Vector3 _globalPoint) {
            if (_shapeTransform) { _globalPoint = _shapeTransform.InverseTransformPoint(_globalPoint); }
            return  _shape.GetSignedDistance(_globalPoint);
        }
    }
    #endregion


}