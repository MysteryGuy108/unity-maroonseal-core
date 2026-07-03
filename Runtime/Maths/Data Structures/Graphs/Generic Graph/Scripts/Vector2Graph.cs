using UnityEngine;

using MaroonSeal.Maths.Geometry;
using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.DataStructures.Graphs
{
    public class Vector2Graph : WeightedGraph<Vector2>
    {
        public void AddPoints(Vector2 _p1, Vector2 _p2)
        {
            AddNode(_p1);
            AddNode(_p2);

            float length = Vector2.Distance(_p1, _p2);
            AddEdge(_p1, _p2, length);
            AddEdge(_p2, _p1, length);
        }

        public void AddLine2D(Line2D _line) => AddPoints(_line.p1, _line.p2);

        public void AddTriangle2D(Triangle2D _triangle)
        {
            AddPoints(_triangle.p1, _triangle.p2);
            AddPoints(_triangle.p2, _triangle.p3);
            AddPoints(_triangle.p3, _triangle.p1);
        }
    }
}
