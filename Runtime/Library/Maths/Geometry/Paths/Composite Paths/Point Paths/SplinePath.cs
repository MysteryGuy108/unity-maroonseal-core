using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {

    [System.Serializable]
    public struct SplinePoint {
        [SerializeField] private Vector3 position;
        public Vector3 Position { readonly get => position; set => position = value; }

        [Range(-180.0f, 180.0f)][SerializeField] private float roll;
        public float Roll { readonly get => roll; set => roll = value; }

        [SerializeField] private Vector2 size;
        public Vector2 Size { readonly get => size; set => size = value; }

        public enum TangentMode { Mirror, Weighted, Corner }
        [Space]
        [SerializeField] private TangentMode tangentMode;
        [SerializeField] private Vector3 tangentIn;
        public Vector3 TangentIn {
            readonly get => tangentIn; 
            set => tangentIn = ConstrainTangent(value, tangentOut); 
        }
        readonly public Vector3 ControlIn => position + tangentIn;

        [SerializeField] Vector3 tangentOut;
        public Vector3 TangentOut {
            readonly get => tangentOut; 
            set => tangentOut = ConstrainTangent(value, tangentIn); 
        } 
        readonly public Vector3 ControlOut => position + tangentOut; 

        public bool hasPrevious;
        public bool hasNext;

        #region Constructors
        public SplinePoint(Vector3 _position, Vector3 _tangentOut, Vector3 _tangentIn, float _roll = 0.0f, Vector2? _size = null, TangentMode _mode = TangentMode.Corner) {
            position = _position;
            roll = _roll;
            size = _size ?? Vector2.zero;
            
            tangentMode = _mode; tangentIn = _tangentIn; tangentOut =_tangentOut;
            hasPrevious = false; hasNext = false;

            tangentOut = ConstrainTangent(tangentOut, tangentIn);
        }
        #endregion

        readonly private Vector3 ConstrainTangent(Vector3 _current, Vector3 _target) {
            return ConstrainTangent(tangentMode, _current, _target);
        }

        static public Vector3 ConstrainTangent(TangentMode _mode, Vector3 _current, Vector3 _target) {
            if (_mode == TangentMode.Weighted) { return -_target.normalized * _current.magnitude; }
            if (_mode == TangentMode.Mirror) { return -_target; }
            return _current;
        }
    }
    
    /// <summary>
    /// Point path comprised of a series of spline path segments.
    /// </summary>
    [System.Serializable]
    sealed public class SplinePath : PointPath<SplinePoint, BezierPath>
    {
        [SerializeField][Min(2)] private int segmentResolution;

        #region Constructors
        public SplinePath() : base() {}
        public SplinePath(List<SplinePoint> _points) : base(_points) {}
        #endregion

        #region Spline Path
        public int GetResolution() => SegmentCount * segmentResolution;
        #endregion

        #region Point Path
        protected override SplinePoint ResetPoint(SplinePoint _point) {
            _point.hasPrevious = false;
            _point.hasNext = false;
            return _point;
        }

        protected override void ApplyPointNeighbour(SplinePoint _start, SplinePoint _end, out SplinePoint _newStart, out SplinePoint _newEnd) {
            _start.hasNext = true;
            _end.hasPrevious = true;
            
            _newStart = _start;
            _newEnd = _end;
        }

        protected override void ApplyPointsToSegment(BezierPath _segment, SplinePoint _start, SplinePoint _end) {
            _segment.SetBezierPoints(_start.Position, _start.ControlOut, _end.ControlIn, _end.Position, _start.Roll, _end.Roll, segmentResolution);
        }
        #endregion
    }
}