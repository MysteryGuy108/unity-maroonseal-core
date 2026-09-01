using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {

    public interface IPointPath : ICompositeShapePath{
        public int PointCount { get; }
    }

    [System.Serializable]
    abstract public class PointPath<TPoint, TSegment> : CompositeShapePath<TSegment>, IPointPath where TSegment : ShapePath, new() {
        [SerializeField] private bool isLoop;
        public override bool IsLoop => isLoop;

        [SerializeField] protected List<TPoint> points = new(2);
        public int PointCount => points.Count;

        #region Constructors
        public PointPath() : base(){ Refresh(); }

        public PointPath(List<TPoint> _points) {
            points = new(_points);
            Refresh();
        }
        #endregion

        #region PointPath
        public TPoint GetPointAtIndex(int _index) { return points[_index]; }

        public void AddPoint(TPoint _point, bool _recalculate = true) {
            points.Add(_point);
            if (_recalculate) { Refresh(); }
        }

        public void AddPointRange(List<TPoint> _points, bool _recalculate = true) {
            points.AddRange(_points);
            if (_recalculate) { Refresh(); }
        }

        override public void Clear() {
            points.Clear();
            segments.Clear();
            base.Clear();
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
        public override void Refresh() {
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

            base.Refresh();
        }
        #endregion
    }
}