using System;
using System.Collections.Generic;

using UnityEngine;


namespace MaroonSeal.Maths.Geometry.Paths 
{
    [System.Serializable]
    abstract public class CompositePathBase<TVector, TTransform, TSegment> : PathBase<TVector, TTransform>, ISegmentPath
        where TTransform : ITransform<TVector>
        where TSegment : IPath<TVector, TTransform>
    {
        public override bool IsLoop => false;
        public override float Length => distanceTable.TotalLength;

        [SerializeField] protected List<TSegment> segments = new();
        public int SegmentCount => segments.Count;

        [SerializeField][HideInInspector] CumulativeDistanceTable distanceTable = new();

        #region PathBase
        sealed public override TTransform EvaluateTime(float _time) {
            if (SegmentCount == 0) { return default; }
            EnsureClean();
            TSegment segment = GetSegmentAtTime(_time, out float blend);
            return segment.EvaluateTime(blend);
        }

        public sealed override float TimeToDistance(float _time)
        {
            if (SegmentCount == 0) { return 0.0f; }
            EnsureClean();
            TSegment segment = GetSegmentAtTime(_time, out float _blend);
            return segment.TimeToDistance(_blend);
        }

        public sealed override float DistanceToTime(float _distance)
        {
            if (SegmentCount == 0) { return 0.0f; }
            EnsureClean();
            TSegment segment = GetSegmentAtDistance(_distance, out float _blend);
            return segment.DistanceToTime(_blend);
        }

        public sealed override float ClosestTimeToPoint(TVector _point)
        {
            if (segments.Count <= 0) { return 0.0f; }

            float closestSqrDistance = Mathf.Infinity;
            int closestIndex = 0;
            float closestLocalTime = 0.0f;

            for (int i = 0; i < SegmentCount; i++)
            {
                TSegment segment = GetSegment(i);
                float localTime = segment.ClosestTimeToPoint(_point);
                float sqrDistance = segment.EvaluateTime(localTime).SqrDistanceTo(_point);

                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    closestIndex = i;
                    closestLocalTime = localTime;
                }
            }

            float timePerSegment = 1.0f / SegmentCount;
            return (closestIndex + closestLocalTime) * timePerSegment;
        }

        protected override void OnEnsureClean() => distanceTable.Rebuild(SegmentCount, i => GetSegment(i).Length);

        public override void Clear() => distanceTable.Clear();
        #endregion

        #region Indices
        protected int GetIndexAtTime(float _time, out float _blend)
        {
            EnsureClean();
            int index = (int)Mathf.Max(0, Mathf.Clamp(_time * this.SegmentCount, 0, this.SegmentCount-1));
            _blend = Mathf.Clamp01(_time * this.SegmentCount - index);
            return index;
        }

        protected int GetIndexAtDistance(float _distance, out float _blend)
        {
            EnsureClean();
            int index = GetIndexAtTime(distanceTable.EvaluateDistance(_distance), out float time);
            _blend = _distance - distanceTable.DistanceAt(index);
            return index;
        }
        #endregion

        #region Segments
        public TSegment GetSegment(int _index) => segments.Count > 0 ? segments[_index] : default;

        public TSegment GetSegmentAtTime(float _time, out float _blend) => GetSegment(GetIndexAtTime(_time, out _blend));
        public TSegment GetSegmentAtDistance(float _distance, out float _blend) => GetSegment(GetIndexAtDistance(_distance, out _blend));
        #endregion
    }

    [System.Serializable]
    public class CompositePath<TSegment> : CompositePathBase<Vector3, Transform3D, TSegment>, IPath3D where TSegment : IPath3D
    {        

    }

    [System.Serializable]
    public class CompositePath2D<TSegment> : CompositePathBase<Vector2, Transform2D, TSegment>, IPath2D where TSegment : IPath2D
    {

    }
}
