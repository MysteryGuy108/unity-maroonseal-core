using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths {
    
    [System.Serializable]
    public struct LinearPoint {
        public Vector3 position;
        [Range(-180.0f, 180.0f)] public float roll;
        public Vector2 size;

        public LinearPoint(Vector3 _position, float _roll = 0.0f, Vector2? _size = null) {
            this.position = _position;
            this.roll = _roll;
            this.size = _size ?? Vector2.zero;
        }
    }

    /// <summary>
    /// Point path comprised of a series of linear path segments.
    /// </summary>
    [System.Serializable]
    sealed public class LinearPath : PointPath<LinearPoint, LinePath>
    {
        #region Constructors
        public LinearPath() : base() {}
        public LinearPath(List<LinearPoint> _points) : base(_points) {}
        #endregion

        #region Point Path
        protected override void ApplyPointsToSegment(LinePath _segment, LinearPoint _start, LinearPoint _end) {
            _segment.SetStartPosition(_start.position);
            _segment.SetEndPosition(_end.position);
        }
        #endregion
    }
}
