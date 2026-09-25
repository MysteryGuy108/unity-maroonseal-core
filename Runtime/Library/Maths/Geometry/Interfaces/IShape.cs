using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    
    public interface IShape<TVector, TTransform> : IGeometry where TTransform : ITransform<TVector>
    {
        public TTransform Transform { get; set; }
        public bool ContainsPoint(TVector _point);
    }

    public interface IPolygon<TVertices, TTransform, TEdge> : IShape<TVertices, TTransform> where TEdge : ICurve<TVertices>
        where TTransform : ITransform<TVertices>
    {
        public int VertexCount { get; }
        public IEnumerable<TVertices> GetVertices();
        public IEnumerable<TEdge> GetEdges();
    }
}