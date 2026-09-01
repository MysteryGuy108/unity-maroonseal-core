using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry {

    public interface IPolygon<TVertices, TEdge> : IShape where TEdge : ICurve<TVertices>
    {
        public int VertexCount { get; }
        public IEnumerable<TVertices> GetVertices();
        public IEnumerable<TEdge> GetEdges();
    }

    public interface IPolygon2D : IPolygon<Vector2, Line2D>, IShape2D {}

    public interface IPolygon3D : IPolygon<Vector3, Line>, IShape3D {}
}