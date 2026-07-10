using MaroonSeal.Maths.Geometry.Shapes;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Graphs
{
    public class Vector3Graph : WeightedGraph<Vector3>
    {
        public void AddPoints(Vector3 _p1, Vector3 _p2)
        {
            AddNode(_p1);
            AddNode(_p2);

            float length = Vector3.Distance(_p1, _p2);
            AddEdge(_p1, _p2, length);
            AddEdge(_p2, _p1, length);
        }

        public void AddLine(Line _line) => AddPoints(_line.p1, _line.p2);

        public void AddTriangle(Triangle _triangle)
        {
            AddPoints(_triangle.Point1, _triangle.Point2);
            AddPoints(_triangle.Point2, _triangle.Point3);
            AddPoints(_triangle.Point3, _triangle.Point1);
        }
    }
}
