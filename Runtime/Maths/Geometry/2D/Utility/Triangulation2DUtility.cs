using System.Collections.Generic;
using MaroonSeal.Maths.Geometry.Shapes;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    static public class Triangulation2DUtility
    {
        static public List<Triangle2D> CalculateTriangles(List<Vector2> _points)
        {

            if (_points.Count < 3) { return new(); }

            Triangle2D superTriangle = GetSuperTriangle(_points);
            List<Triangle2D> triangles = new() { superTriangle };

            for(int i = 0; i < _points.Count; i++) {
                InsertPoint(i, _points, triangles);
            }

            triangles.RemoveAll(tri => 
                tri.ContainsVertex(superTriangle.Point1) ||
                tri.ContainsVertex(superTriangle.Point2) ||
                tri.ContainsVertex(superTriangle.Point3));

            return triangles;
        }

        static private void InsertPoint(int _pointIndex, List<Vector2> _points, List<Triangle2D> _triangles)
        {
            Vector2 point = _points[_pointIndex];
            List<Triangle2D> badTriangles =  new();

            // Find bad triangles.
            foreach(Triangle2D tri in _triangles) {
                if (tri.GetCircumcircle().Contains(point)) {
                    badTriangles.Add(tri);
                }
            }

            // Find new Edges
            List<Line2D> polygonEdges = new();
            foreach(Triangle2D badTri in badTriangles) { 
                List<Line2D> edges = new(badTri.GetEdges());

                foreach(Line2D edge in edges) {
                    int count = 0;
                    foreach(Triangle2D badTri2 in badTriangles) { 
                        if (badTri2.ContainsVertex(edge.p1) && badTri2.ContainsVertex(edge.p2)) { count++; }
                    }

                    if (count <= 1) { polygonEdges.Add(edge); }
                }
            }

            // Remove bad triangles.
            for(int i = _triangles.Count-1; i >=0; i--) {
                foreach(Triangle2D badTri in badTriangles) {
                    if (_triangles[i] == badTri) {
                        _triangles.RemoveAt(i);
                        break;
                    }
                }
            }

            badTriangles.Clear();

            // Build new triangles.
            foreach(Line2D edge in polygonEdges) {
                Triangle2D newTri = new(point, edge.p1, edge.p2);
                _triangles.Add(newTri);
            }
            polygonEdges.Clear();

            /*
            Vector2 point = _points[_pointIndex];
            List<int> badTriangles = new();

            Dictionary<Line2D, int> edgeCounts = new();

            // Find all bad triangles.
            for(int i = 0; i < _triangles.Count; i++)
            {
                Triangle2D triangle = _triangles[i];
                if (!triangle.GetCircumcircle().IsPositionInRadius(point)) { continue; }
                badTriangles.Add(i);

                AddEdge(triangle.GetEdgeAB());
                AddEdge(triangle.GetEdgeBC());
                AddEdge(triangle.GetEdgeCA());
            }

            // Remove bad triangles.
            for (int i = badTriangles.Count - 1; i >= 0; i--) {
                _triangles.RemoveAt(badTriangles[i]);
            }

            // Add new triangles.
            foreach (KeyValuePair<Line2D, int> pair in edgeCounts)
            {
                // Interior edges appear twice.
                if (pair.Value != 1) { continue; }

                Triangle triangle = new(pair.Key.p1, pair.Key.p2, point);

                _triangles.Add(triangle);
            }

            void AddEdge(Line2D _edge)
            {
                if (edgeCounts.TryGetValue(_edge, out int count)) { edgeCounts[_edge]++; }
                else { edgeCounts.Add(_edge, 1); }
            }
            */
        }

        static public Triangle2D GetSuperTriangle(List<Vector2> _points)
        {
            Bounds bounds = new();
            foreach(Vector2 point in _points) { bounds.Encapsulate(point); }

            float dx = bounds.max.x - bounds.min.x;
            float dy = bounds.max.y - bounds.min.y;
            float delta = Mathf.Max(dx, dy) * 20.0f;

            Vector2 p1 = new (bounds.center.x - delta, bounds.center.y - delta);
            Vector2 p2 = new (bounds.center.x, bounds.center.y + delta * 2.0f);
            Vector2 p3 = new (bounds.center.x + delta, bounds.center.y - delta);

            return new Triangle2D(p1, p2, p3);
        }
    }
}
