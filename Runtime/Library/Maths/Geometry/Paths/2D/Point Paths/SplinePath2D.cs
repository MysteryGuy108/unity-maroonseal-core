using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {

    [System.Serializable]
    public struct SplinePoint2D {
        [SerializeField] private Vector2 position;
        public Vector2 Position { readonly get => position; set => position = value; }

        [SerializeField] private float size;
        public float Size { readonly get => size; set => size = value; }

        public enum TangentMode { Mirror, Weighted, Corner }
        [Space]
        [SerializeField] private TangentMode tangentMode;
        [SerializeField] private Vector2 tangentIn;
        public Vector2 TangentIn {
            readonly get => tangentIn; 
            set => tangentIn = ConstrainTangent(value, tangentOut); 
        }
        readonly public Vector2 ControlIn => position + tangentIn;

        [SerializeField] Vector2 tangentOut;
        public Vector2 TangentOut {
            readonly get => tangentOut; 
            set => tangentOut = ConstrainTangent(value, tangentIn); 
        } 
        readonly public Vector2 ControlOut => position + tangentOut; 

        public bool hasPrevious;
        public bool hasNext;

        #region Constructors
        public SplinePoint2D(Vector3 _position, Vector3 _tangentOut, Vector3 _tangentIn, float _roll = 0.0f, float? _size = null, TangentMode _mode = TangentMode.Corner) {
            position = _position;
            size = _size ?? 0.0f;
            
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
    sealed public class SplinePath2D : PointPath2D<SplinePoint2D, BezierPath2D>
    {
        [SerializeField][Min(2)] private int segmentResolution;

        #region Constructors
        public SplinePath2D(List<SplinePoint2D> _points, int _segmentResolution = 2) : base(_points) { segmentResolution = Mathf.Max(2, _segmentResolution); }
        public SplinePath2D(int _segmentResolution = 2) : base() { segmentResolution = Mathf.Max(2, _segmentResolution); }
        public SplinePath2D() : this(2) {}
        #endregion

        #region Spline Path
        public int GetResolution() => SegmentCount * segmentResolution;
        #endregion

        #region Point Path
        protected override SplinePoint2D ResetPoint(SplinePoint2D _point) {
            _point.hasPrevious = false;
            _point.hasNext = false;
            return _point;
        }

        protected override void ApplyPointNeighbour(SplinePoint2D _start, SplinePoint2D _end, out SplinePoint2D _newStart, out SplinePoint2D _newEnd) {
            _start.hasNext = true;
            _end.hasPrevious = true;
            
            _newStart = _start;
            _newEnd = _end;
        }

        protected override void ApplyPointsToSegment(BezierPath2D _segment, SplinePoint2D _start, SplinePoint2D _end) {
            _segment.SetBezierPoints(_start.Position, _start.ControlOut, _end.ControlIn, _end.Position);
        }
        #endregion

        #region Static Constructors
        public SplinePath UnitCircle => new(new List<SplinePoint>()
            {
                
            });
        #endregion
    }
}