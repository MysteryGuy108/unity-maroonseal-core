using UnityEngine;

using MaroonSeal.Maths.Geometry.SDFs;
using System.Collections.Generic;

namespace MaroonSeal.Maths.Geometry.Shapes
{
    [System.Serializable]
    public struct Hexagon2D : IPolygon2D, ISDFShape
    {
        [field : SerializeField] public Transform2D Transform {get; set; }
        readonly public int VertexCount => 6;

        #region IShape2D
        public bool Contains(Vector2 _point)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region IPolygon
        public IEnumerable<Vector2> GetVertices()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Line2D> GetEdges()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region ISDFShape
        public float GetSignedDistance(Vector3 _point)
        {
            throw new System.NotImplementedException();
        }


        #endregion
    }
}
