using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    [System.Serializable]
    public struct Hexagon2D : IPolygon2D, ISDF2D
    {
        [field : SerializeField] public Transform2D Transform {get; set; }
        [SerializeField][Min(0.0f)] private float radius;
        readonly public int VertexCount => 6;

        public readonly Vector2 this[int _index] => Transform.TransformPoint(Vector2Maths.FromDegrees(60.0f * _index, radius));

        #region IShape2D
        public readonly bool ContainsPoint(Vector2 _point) => GetSignedDistance(_point) <= 0.0f;
        #endregion

        #region IPolygon
        public readonly IEnumerable<Vector2> GetVertices()
        {
            for(int i = 0; i < 6; i++) { yield return this[i]; }
        }

        public readonly IEnumerable<Line2D> GetEdges()
        {
            for(int i = 0; i < 6; i++) { yield return new Line2D(this[i], this[(i+1)%6]); }
        }
        #endregion

        #region ISDFShape
        public readonly float GetSignedDistance(Vector2 _point)
        {
            Vector3 k = new(-0.866025404f, 0.5f, 0.577350269f);

            _point = _point.Abs();
            _point -= 2.0f * Mathf.Min(Vector2.Dot(k.ToXY(),_point),0.0f) * k.ToXY();

            _point -= new Vector2(Mathf.Clamp(_point.x, -k.z*radius, k.z*radius), radius);
            return _point.magnitude*Mathf.Sign(_point.y);
        }
        #endregion
    }
}
