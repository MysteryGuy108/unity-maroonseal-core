using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaroonSeal.Maths.Shapes;

namespace MaroonSeal.Maths.DataStructures.NodeGraphs.Generators {
    public class DelaunayTriangulatorGraphCalculator
    {
        static public NodeGraphWeighted<Vector2> CalculateGraph(List<Vector2> _points, float _pointsSquareBounds) {

            NodeGraphWeighted<Vector2> graph = new();
            
            // Adding points to graph.
            graph.PushNodeRange(_points);

            List<Triangle2D> triangles = CaluclateTrianglesFromPoints(_points, _pointsSquareBounds);

            for(int i = 0; i < triangles.Count; i++) {
                
                int indexA = graph[triangles[i].pointA];
                int indexB = graph[triangles[i].pointB];
                int indexC = graph[triangles[i].pointC];
                
                if (indexA < 0 || indexB < 0 || indexC < 0) { continue; }
                if (indexA == indexB || indexB == indexC || indexC == indexA) { continue; }
                
                graph.SetEdgeWeight(indexA, indexB, triangles[i].GetLengthAB());
                graph.SetEdgeWeight(indexB, indexC, triangles[i].GetLengthBC());
                graph.SetEdgeWeight(indexC, indexA, triangles[i].GetLengthCA());
            }

            return graph;
        }

        static private List<Triangle2D> CaluclateTrianglesFromPoints(List<Vector2> _points, float _pointsSquareBounds) {
            
            List<Triangle2D> triangles = new() {
                GetSuperTriangle(_pointsSquareBounds)
            };

            foreach(Vector2 point in _points) {
                
                List<Triangle2D> badTriangles =  new();

                foreach(Triangle2D tri in triangles) {
                    if (tri.GetCircumcircle().IsPositionInRadius(point)) {
                        badTriangles.Add(tri);
                    }
                }

                List<Line2D> polygonEdges = new();

                foreach(Triangle2D badTri in badTriangles) { 
                    List<Line2D> edges = new(badTri.GetEdges());

                    foreach(Line2D edge in edges) {
                        int count = 0;
                        foreach(Triangle2D badTri2 in badTriangles) { 
                            if (badTri2.ContainsPoint(edge.from) && badTri2.ContainsPoint(edge.to)) { count++; }
                        }

                        if (count <= 1) { polygonEdges.Add(edge); }
                    }
                }

                for(int i = triangles.Count-1; i >=0; i--) {
                    foreach(Triangle2D badTri in badTriangles) {
                        if (triangles[i] == badTri) {
                            triangles.RemoveAt(i);
                            break;
                        }
                    }
                }

                badTriangles.Clear();

                foreach(Line2D edge in polygonEdges) {
                    Triangle2D newTri = new(point, edge.from, edge.to);
                    triangles.Add(newTri);
                }
                polygonEdges.Clear();
            }

            return triangles;
        }

        static private Triangle2D GetSuperTriangle(float _squareSize) {

            float triangleSideLength = _squareSize / 0.464f;

            Vector2Int triA = new((int)(_squareSize * 0.5f - triangleSideLength * 0.5f), 0);
            Vector2Int triB = new((int)(_squareSize * 0.5f + triangleSideLength * 0.5f), 0);

            Vector2Int triC = new((int)(_squareSize * 0.5f), (int)(0.86602540378f * triangleSideLength));

            return new Triangle2D(triA, triB, triC);
        }
    }
}