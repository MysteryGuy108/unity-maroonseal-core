using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {

    [System.Serializable]
    public struct SplinePoint2D {
        [SerializeField] private Vector2 anchor;
        public Vector2 Anchor { readonly get => anchor; set => anchor = value; }

        [SerializeField] private float size;
        public float Size { readonly get => size; set => size = value; }

        [Space]
        [SerializeField] private SplinePoint.TangentMode tangentMode;
        [SerializeField] private Vector2 tangentIn;
        public Vector2 TangentIn {
            readonly get => tangentIn; 
            set => tangentIn = SplinePoint.ConstrainTangent(tangentMode, value, tangentOut); 
        }
        readonly public Vector2 ControlIn => anchor + tangentIn;

        [SerializeField] Vector2 tangentOut;
        public Vector2 TangentOut {
            readonly get => tangentOut; 
            set => tangentOut = SplinePoint.ConstrainTangent(tangentMode, value, tangentIn); 
        } 
        readonly public Vector2 ControlOut => anchor + tangentOut; 

        [HideInInspector] public bool hasTangentIn;
        [HideInInspector] public bool hasTangentOut;

        #region Constructors
        public SplinePoint2D(Vector3 _anchor, Vector3 _tangentOut, Vector3 _tangentIn, float? _size = null, SplinePoint.TangentMode _mode = SplinePoint.TangentMode.Corner) {
            anchor = _anchor;
            size = _size ?? 0.0f;
            
            tangentMode = _mode; tangentIn = _tangentIn; tangentOut =_tangentOut;
            hasTangentIn = false; hasTangentOut = false;

            tangentOut = SplinePoint.ConstrainTangent(_mode, tangentOut, tangentIn);
        }
        #endregion
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
        public SplinePath2D() : this(16) {}
        #endregion

        #region Spline Path
        public int GetResolution() => SegmentCount * segmentResolution;
        #endregion

        #region Point Path
        protected override SplinePoint2D ResetPoint(SplinePoint2D _point) {
            _point.hasTangentIn = false;
            _point.hasTangentOut = false;
            return _point;
        }

        protected override void ApplyPointNeighbour(SplinePoint2D _start, SplinePoint2D _end, out SplinePoint2D _newStart, out SplinePoint2D _newEnd) {
            _start.hasTangentOut = true;
            _end.hasTangentIn = true;
            
            _newStart = _start;
            _newEnd = _end;
        }

        protected override void ApplyPointsToSegment(BezierPath2D _segment, SplinePoint2D _start, SplinePoint2D _end) {
            _segment.SetBezierPoints(_start.Anchor, _start.ControlOut, _end.ControlIn, _end.Anchor);
        }
        #endregion

        #region Static Constructors
        public SplinePath UnitCircle => new(new List<SplinePoint>()
            {
                
            });
        #endregion
    }
}