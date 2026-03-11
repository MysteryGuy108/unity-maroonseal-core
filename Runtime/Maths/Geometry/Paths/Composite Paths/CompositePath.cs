using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry.Paths.LUTs;

namespace MaroonSeal.Maths.Geometry.Paths {

    public interface ICompositePath : IShapePath {
        public int SegmentCount { get;}
    }

    public class CompositePath<TPathSegment> : ShapePath, ICompositePath where TPathSegment : IShapePath 
    {
        protected List<TPathSegment> segments;
        public int SegmentCount => segments == null ? 0 : segments.Count;

        [SerializeField] protected PathFloatLUT distanceLUT = new();
        override public float Length => distanceLUT == null ? 0.0f : distanceLUT[^1];

        public override bool IsLoop => false;

        #region Constructors
        public CompositePath() { segments = new(); }
        ~CompositePath() { segments.Clear(); }
        #endregion

        #region GeometryPath
        public override PointTransform GetPointAtTime(float _time) {
            if (SegmentCount <= 0) { return new(); }
            TPathSegment segment = GetSegmentAtTime(_time, out float _localTime);
            return segment.GetPointAtTime(_localTime);
        }

        override public float GetDistanceAtTime(float _time) {
            if (segments.Count <= 0) { return 0.0f;}
            int segmentIndex = GetSegmentIndexAtTime(_time, out float localTime);
            TPathSegment segment = GetSegmentAtIndex(segmentIndex);
            return segment.GetDistanceAtTime(localTime) + GetDistanceAtSegmentIndex(segmentIndex);
        }

        override public float GetTimeAtDistance(float _distance) {
            if (segments.Count <= 0) { return 0.0f;}
            int segmentIndex = GetSegmentIndexAtDistance(_distance, out float localDistance);
            TPathSegment segment = GetSegmentAtIndex(segmentIndex);
            return (segment.GetTimeAtDistance(localDistance) / SegmentCount) + GetTimeAtSegmentIndex(segmentIndex);
        }
        
        override public float GetTimeClosestToPosition(Vector3 _position) {
            if (segments.Count <= 0) { return 0.0f;}

            float closestSqrDistance = Mathf.Infinity;
            int closestIndex = -1;
            float closestTime = -1.0f;

            for(int i = 0; i < SegmentCount; i++) {
                float time = this.GetTimeAtSample(i, SegmentCount);
                float sqrDistance = (_position - GetPointAtTime(time).position).sqrMagnitude;
                if (sqrDistance < closestSqrDistance) {
                    closestSqrDistance = sqrDistance;
                    closestIndex = i;
                }
            }

            float timePerSegment = 1.0f / SegmentCount;
            return (closestTime * timePerSegment) + (closestIndex * timePerSegment);
        }
        
        override public void Refresh() {
            distanceLUT ??= new();
            distanceLUT.Clear();
            
            if(segments == null) { distanceLUT.AddValue(0.0f); return; }
            if(segments.Count <= 0) { distanceLUT.AddValue(0.0f); return; }

            foreach(TPathSegment segment in segments) { 
                distanceLUT.AddValue(segment.Length); 
            }
        }

        override public void Clear() {
            segments?.Clear();
            distanceLUT?.Clear();
        }
        #endregion

        #region Path Segments
        public TPathSegment GetSegmentAtIndex(int _index) => segments.Count > 0 ? segments[_index] : default;

        public int GetSegmentIndexAtTime(float _time, out float _localTime) {
            int index = (int)Mathf.Clamp(_time * this.SegmentCount, 0, this.SegmentCount-1);
            if (index < 0) { _localTime = 0.0f; return -1; }

            _localTime = Mathf.Clamp01(_time * this.SegmentCount - index);
            return index;
        }

        public int GetSegmentIndexAtDistance(float _distance, out float _localDistance) {
            int index = GetSegmentIndexAtTime(distanceLUT.EvaulateTimeAtValue(_distance), out float localTime);
            if (index < 0) { _localDistance = 0.0f; return -1; }

            _localDistance = (GetDistanceAtSegmentIndex(index+1) - GetDistanceAtSegmentIndex(index)) * localTime;
            return index;
        }

        public TPathSegment GetSegmentAtTime(float _time, out float _localTime) => GetSegmentAtIndex(GetSegmentIndexAtTime(_time, out _localTime));
        public TPathSegment GetSegmentAtDistance(float _distance, out float _localDistance) => GetSegmentAtIndex(GetSegmentIndexAtDistance(_distance, out _localDistance));

        public float GetTimeAtSegmentIndex(int _index) => distanceLUT.GetTimeAtIndex(_index);
        public float GetDistanceAtSegmentIndex(int _index) => distanceLUT.GetValueAtIndex(_index);
        #endregion
    }


}