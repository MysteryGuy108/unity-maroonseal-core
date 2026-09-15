using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {

    [System.Serializable]
    public struct SplinePoint {
        [SerializeField] private Vector3 anchor;
        public Vector3 Anchor { readonly get => anchor; set => anchor = value; }

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
        readonly public Vector3 ControlIn => anchor + tangentIn;

        [SerializeField] Vector3 tangentOut;
        public Vector3 TangentOut {
            readonly get => tangentOut; 
            set => tangentOut = ConstrainTangent(value, tangentIn); 
        } 
        readonly public Vector3 ControlOut => anchor + tangentOut; 

        [HideInInspector] public bool hasTangentIn;
        [HideInInspector] public bool hasTangentOut;

        #region Constructors
        public SplinePoint(Vector3 _position, Vector3 _tangentOut, Vector3 _tangentIn, float _roll = 0.0f, Vector2? _size = null, TangentMode _mode = TangentMode.Corner) {
            anchor = _position;
            roll = _roll;
            size = _size ?? Vector2.zero;
            
            tangentMode = _mode; tangentIn = _tangentIn; tangentOut =_tangentOut;
            hasTangentIn = false; hasTangentOut = false;

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
        public SplinePath(List<SplinePoint> _points, int _segmentResolution = 2) : base(_points) { segmentResolution = Mathf.Max(2, _segmentResolution); }
        public SplinePath(int _segmentResolution = 2) : base() { segmentResolution = Mathf.Max(2, _segmentResolution); }
        public SplinePath() : this(16) {}
        #endregion

        #region Spline Path
        public int GetResolution() => SegmentCount * segmentResolution;
        #endregion

        #region Point Path
        protected override SplinePoint ResetPoint(SplinePoint _point) {
            _point.hasTangentIn = false;
            _point.hasTangentOut = false;
            return _point;
        }

        protected override void ApplyPointNeighbour(SplinePoint _start, SplinePoint _end, out SplinePoint _newStart, out SplinePoint _newEnd) {
            _start.hasTangentOut = true;
            _end.hasTangentIn = true;
            
            _newStart = _start;
            _newEnd = _end;
        }

        protected override void ApplyPointsToSegment(BezierPath _segment, SplinePoint _start, SplinePoint _end) {
            _segment.SetBezierPoints(_start.Anchor, _start.ControlOut, _end.ControlIn, _end.Anchor, _start.Roll, _end.Roll);
        }
        #endregion

        #region Static Constructors
        public SplinePath UnitCircle => new(new List<SplinePoint>()
            {
                
            });
        #endregion
    }
}