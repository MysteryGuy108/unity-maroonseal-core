using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry;
using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.DataStructures.Graphs
{
    static public class GraphGenerator
    {
        #region Deluanay Triangulation
        static public Vector2Graph CreateTriangulation2DGraph(List<Vector2> _points)
        {
            List<Triangle2D> triangles = Triangulation2DUtility.CalculateTriangles(_points);
            Vector2Graph triangulationGraph = new();
            foreach(Triangle2D triangle in triangles)
            {
                triangulationGraph.AddTriangle2D(triangle);
            }
            return triangulationGraph;
        }
        #endregion
    }
}
