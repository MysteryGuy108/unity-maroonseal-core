using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths 
{
    public interface IPath
    {
        public bool IsLoop { get; }
        public float Length { get; }

        public void Clear();
    }

    public interface ISegmentPath : IPath
    {
        public int SegmentCount { get; }
        public float GetTimeAtIndex(int _index) => _index / (float)SegmentCount;
        public IEnumerable<float> GetSegmentTimes()
        {
            for(int i = 0; i < SegmentCount; i++)
            {
                float time = GetTimeAtIndex(i);
                yield return time;
            }
        }
    }

    public interface IPath<TVector, TTransform> : IPath where TTransform : ITransform<TVector>
    {
        public TTransform EvaluateTime(float _t);
        public TTransform EvaluateDistance(float _distance) => EvaluateTime(DistanceToTime(_distance));

        public float TimeToDistance(float _t);
        public float DistanceToTime(float _distance);

        public float ClosestTimeToPoint(TVector _point);
        public float ClosestDistanceToPoint(TVector _point) => TimeToDistance(ClosestTimeToPoint(_point));
        public TTransform ClosestPoint(TVector _point) => EvaluateTime(ClosestTimeToPoint(_point));
    }

    public interface IPath3D : IPath<Vector3, Transform3D> {}

    public interface IPath2D : IPath<Vector2, Transform2D> {}


}
