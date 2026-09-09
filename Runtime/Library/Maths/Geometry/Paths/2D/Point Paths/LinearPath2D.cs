using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths
{
    [System.Serializable]
    public struct LinearPoint2D {
        public Vector2 position;
        public float size;

        public LinearPoint2D(Vector2 _position, float _roll = 0.0f, float? _size = null) {
            this.position = _position;
            this.size = _size ?? 0.0f;
        }
    }

    [System.Serializable]
    public class LinearPath2D : PointPath2D<LinearPoint2D, LinePath2D>
    {
        #region Constructors
        public LinearPath2D() : base() {}
        public LinearPath2D(List<LinearPoint2D> _points) : base(_points) {}
        #endregion
        
        #region Point Path
        protected override void ApplyPointsToSegment(LinePath2D _segment, LinearPoint2D _start, LinearPoint2D _end) {
            _segment.SetStartPosition(_start.position);
            _segment.SetEndPosition(_end.position);
        }
        #endregion
    }
}
