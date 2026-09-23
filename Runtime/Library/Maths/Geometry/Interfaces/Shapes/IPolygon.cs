using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry {

    public interface IPolygon<TVertices, TTransform, TEdge> : IShape<TVertices, TTransform> where TEdge : ICurve<TVertices>
        where TTransform : ITransform<TVertices>
    {
        public int VertexCount { get; }
        public IEnumerable<TVertices> GetVertices();
        public IEnumerable<TEdge> GetEdges();
    }
}