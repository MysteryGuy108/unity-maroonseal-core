using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {

    [System.Serializable]
    abstract public class PointPathBase<TVector, TTransform, TPoint, TSegment> : CompositePathBase<TVector, TTransform, TSegment>
            where TTransform : ITransform<TVector>
            where TSegment : IPath<TVector, TTransform>, new() {

        [SerializeField] private bool isLoop;
        public override bool IsLoop => isLoop;

        [SerializeField] protected List<TPoint> points;
        public int PointCount => points.Count;

        #region Constructor
        public PointPathBase() : base() => points = new(2);
        public PointPathBase(List<TPoint> _points) : base() => points = new(_points);
        #endregion

        #region PathBase
        protected override void OnEnsureClean()
        {
            RebuildSegmentsFromPoints();
            base.OnEnsureClean();
        }
        #endregion

        #region PointPath
        public TPoint GetPointAtIndex(int _index) { return points[_index]; }

        public void AddPoint(TPoint _point) {
            points.Add(_point);
            SetDirty();
        }

        public void AddPointRange(List<TPoint> _points) {
            points.AddRange(_points);
            SetDirty();
        }

        override public void Clear() {
            base.Clear();
            points.Clear();
        }
        #endregion

        #region Refresh
        virtual protected TPoint ResetPoint(TPoint _point) => _point;

        virtual protected void ApplyPointNeighbour(TPoint _start, TPoint _end, out TPoint _newStart, out TPoint _newEnd) {
            _newStart = _start;
            _newEnd = _end;
        }
        abstract protected void ApplyPointsToSegment(TSegment _segment, TPoint _start, TPoint _end);
        #endregion

        #region Geometry Path
        private void RebuildSegmentsFromPoints() {
            points ??= new(); segments ??= new();

            int newSegmentCount = Mathf.Max(0, IsLoop ? points.Count : points.Count-1);

            while (segments.Count != newSegmentCount) {
                if(segments.Count < newSegmentCount) { segments.Add(new()); }
                else if(segments.Count > newSegmentCount) {
                    segments[^1].Clear();
                    segments.RemoveAt(segments.Count-1);
                }
            }

            for(int i = 0; i < points.Count; i++) { points[i] = ResetPoint(points[i]); }

            for(int i = 0; i < segments.Count; i++) {
                TPoint start = points[i];
                TPoint end = points[(i+1)%points.Count];

                ApplyPointNeighbour(start, end, out start, out end);
                ApplyPointsToSegment(segments[i], start, end);

                points[i] = start;
                points[(i+1)%points.Count] = end;
            }
        }
        #endregion
    }

    [System.Serializable]
    abstract public class PointPath<TPoint, TSegment> : PointPathBase<Vector3, Transform3D, TPoint, TSegment>, IPath3D where TSegment : IPath3D, new()
    {
        #region Constructor
        public PointPath() : base() {}
        public PointPath(List<TPoint> _points) : base(_points) {}
        #endregion
    }

    [System.Serializable]
    abstract public class PointPath2D<TPoint, TSegment> : PointPathBase<Vector2, Transform2D, TPoint, TSegment>, IPath2D where TSegment : IPath2D, new()
    {
        #region Constructor
        public PointPath2D() : base() {}
        public PointPath2D(List<TPoint> _points) : base(_points) {}
        #endregion
    }
}