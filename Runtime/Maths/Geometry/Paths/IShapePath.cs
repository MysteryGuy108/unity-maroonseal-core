using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {
    public interface IShapePath
    {
        public bool IsLoop { get; }
        public float Length { get; }

        public PointTransform GetPointAtTime(float _t);
        public PointTransform GetPointAtDistance(float _distance) => GetPointAtTime(GetTimeAtDistance(_distance));


        #region Time Conversions
        public float GetDistanceAtTime(float _t);
        public float GetTimeAtDistance(float _distance);
        #endregion

        #region Closest To Point
        public float GetTimeClosestToPosition(Vector3 _position);
        #endregion
    }

    #region Extensions
    public static class IPathExtensions {

        #region Time Evaluations
        static public Vector3 GetPositionAtTime(this IShapePath _path, float _t) => _path.GetPointAtTime(_t).position;
        static public Quaternion GetRotationAtTime(this IShapePath _path, float _t) => _path.GetPointAtTime(_t).Rotation;
        static public Vector3 GetScaleAtTime(this IShapePath _path, float _t) => _path.GetPointAtTime(_t).scale;
        #endregion

        #region Distance Evaluations
        static public Vector3 GetPositionAtDistance(this IShapePath _path, float _distance) => _path.GetPointAtDistance(_distance).position;
        static public Quaternion GetRotationAtDistance(this IShapePath _path, float _distance) =>  _path.GetPointAtDistance(_distance).Rotation;
        static public Vector3 GetScaleAtDistance(this IShapePath _path, float _distance) =>  _path.GetPointAtDistance(_distance).scale;
        #endregion

        static public PointTransform GetPointClosestToPosition(this IShapePath _path, Vector3 _position) {
            return _path.GetPointAtTime(_path.GetTimeClosestToPosition(_position));
        }

        #region Samples
        static public float GetTimeAtSample(this IShapePath _path, int _index, int _maxSamples) {
            return Mathf.Clamp01(_index / (float)(_maxSamples-1));
        }
        static public float GetDistanceAtSample(this IShapePath _path, int _index, int _maxSamples) {
            return _path.GetDistanceAtTime(_path.GetTimeAtSample(_index, _maxSamples));
        }
        static public PointTransform GetPointAtDistanceSample(this IShapePath _path, int _index, int _maxSamples) { 
            return _path.GetPointAtDistance(_path.GetDistanceAtSample(_index, _maxSamples)); 
        }
        #endregion
    }
    #endregion
}