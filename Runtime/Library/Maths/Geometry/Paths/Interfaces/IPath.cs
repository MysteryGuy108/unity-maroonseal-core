using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths 
{
    public interface IPath<TVector, TTransform> where TTransform : ITransform<TVector>
    {
        public bool IsLoop { get; }
        public float Length { get; }

        public TTransform EvaluateTime(float _t);
        public TTransform EvaluateDistance(float _distance) => EvaluateTime(DistanceToTime(_distance));

        public float TimeToDistance(float _t);
        public float DistanceToTime(float _distance);

        public float ClosestTimeToPoint(TVector _point);
        public float ClosestDistanceToPoint(TVector _point) => TimeToDistance(ClosestTimeToPoint(_point));
        public TTransform ClosestPoint(TVector _point) => EvaluateTime(ClosestTimeToPoint(_point));

        public void Clear();
    }

    public interface IPath3D : IPath<Vector3, Transform3D> {}

    public interface IPath2D : IPath<Vector2, Transform2D> {}
}
