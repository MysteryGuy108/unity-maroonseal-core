using UnityEngine;

namespace MaroonSeal.Maths.Geometry {
    public static class GeometryGizmos2D
    {
        #region ICurve2D
        public static void DrawLine(Line2D _line, float _pointRadii = 0.03125f) {
            Gizmos.DrawLine(_line.p1, _line.p2);
            if (_pointRadii <= 0.0f) { return; }
            Gizmos.DrawSphere(_line.p1, _pointRadii);
            Gizmos.DrawSphere(_line.p2, _pointRadii);
        }

        public static void DrawCurve(ICurve2D _curve, int _segments = 24, float _endPointRadii = 0.03125f)
        {
            float timeIncrement = 1.0f / _segments;

            for(int i = 0; i < _segments; i++)
            {
                float t1 = timeIncrement * i;
                float t2 = timeIncrement * (i+1);

                Gizmos.DrawLine(_curve.EvaluatePointAtTime(t1), _curve.EvaluatePointAtTime(t2));
            }

            if (_endPointRadii <= 0.0f) { return; }
            Gizmos.DrawSphere(_curve.EvaluatePointAtTime(0.0f), _endPointRadii);
            Gizmos.DrawSphere(_curve.EvaluatePointAtTime(1.0f), _endPointRadii);
        }

        public static void DrawCurve(Circle2D _circle, int _segments = 24, float _endPointRadii = 0f) => 
            DrawCurve((ICurve2D)_circle, _segments, _endPointRadii);

        public static void DrawCurve(CubicBezier2D _bezier, int _segments = 24, float _endPointRadii = 0.03125f)
        {
            DrawCurve((ICurve2D)_bezier, _segments, _endPointRadii);

            if (_endPointRadii <= 0.0f) { return; }
            Gizmos.DrawLine(_bezier.anchorA, _bezier.controlA);
            Gizmos.DrawLine(_bezier.anchorB, _bezier.controlB);

            Gizmos.DrawSphere(_bezier.controlA, _endPointRadii);
            Gizmos.DrawSphere(_bezier.controlB, _endPointRadii);
        }
        #endregion

        #region IShape2D
        public static void DrawPolygon(IPolygon2D _polygon, float _pointRadii = 0.03125f)
        {
            foreach(Line2D edge in _polygon.GetEdges()) {
                DrawLine(edge, 0.0f);
            }

            foreach(Vector2 vertex in _polygon.GetVertices())
            {
                Gizmos.DrawSphere(vertex, _pointRadii);
            }
        }
        #endregion
    }
}
