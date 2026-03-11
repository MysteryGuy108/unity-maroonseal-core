using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    public static class ShapeGizmos
    {
        public static void DrawLine(Line _line, float _pointRadii = 0.03125f) {
            Gizmos.DrawLine(_line.start, _line.end);
            Gizmos.DrawSphere(_line.start, _pointRadii);
            Gizmos.DrawSphere(_line.end, _pointRadii);
        }

        public static void DrawLine(Line2D _line, float _pointRadii = 0.03125f) {
            Gizmos.DrawLine(_line.from, _line.to);
            Gizmos.DrawSphere(_line.from, _pointRadii);
            Gizmos.DrawSphere(_line.to, _pointRadii);
        }

        public static void DrawTriangle(Triangle _triangle, float _pointRadii = 0.03125f) {
            DrawPolygonShape(_triangle, _pointRadii);
        }

        public static void DrawBox(Box _box) {
            Gizmos.DrawWireCube(_box.transform.position, _box.dimensions);
        }

        public static void DrawCircle(Circle _circle, int _resolution = 32, float _pointSize = 0.125f) {
            DrawInterpolationShape(_circle, _resolution);
            ExtraGizmos.DrawAxis(_circle.transform.position, _circle.transform.Rotation, _circle.radius * _pointSize * Vector2.one);
        }

        public static void DrawCircle(Circle2D _circle, int _resolution = 32, float _pointSize = 0.125f) {
            DrawInterpolationShape(_circle, _resolution);
            ExtraGizmos.DrawAxis(_circle.transform.position,  _circle.transform.Rotation, _circle.radius * _pointSize * Vector2.one);
        }

        public static void DrawArc(Arc _arc, int _resolution = 32, float _pointRadii = 0.03125f, float _axisSize = 0.125f) {
            DrawInterpolationShape(_arc, _resolution);
            ExtraGizmos.DrawAxis(_arc.transform.position, _arc.transform.Rotation, _arc.radius * _axisSize * Vector2.one);

            Gizmos.DrawSphere(_arc.EvaluatePositionAtTime(0.0f), _pointRadii);
            Gizmos.DrawSphere(_arc.EvaluatePositionAtTime(1.0f), _pointRadii);
        }

        public static void DrawConicSection(ConicSection _conicSection, int _resolution = 32, float _pointSize = 0.125f) {
            DrawInterpolationShape(_conicSection, _resolution);
            (Vector3, Vector3) foci = _conicSection.GetFoci();
            
            Vector2 size = _conicSection.minRadius * _pointSize * Vector2.one;
            ExtraGizmos.DrawAxis(foci.Item1, _conicSection.transform.Rotation, size);
            ExtraGizmos.DrawAxis(foci.Item2, _conicSection.transform.Rotation, size);
        }

        public static void DrawSphere(Sphere _sphere, float _pointSize = 0.125f) {
            Gizmos.DrawSphere(_sphere.transform.position, _sphere.radius);
            ExtraGizmos.DrawAxis(_sphere.transform.position, _sphere.transform.Rotation, Vector3.one * _sphere.radius * _pointSize);
        }

        public static void DrawCubicBezier(CubicBezier _bezier, int _resolution = 32, float _pointRadii = 0.03125f) {
            DrawInterpolationShape(_bezier, _resolution);

            Gizmos.DrawLine(_bezier.anchorA, _bezier.controlA);
            Gizmos.DrawSphere(_bezier.anchorA, _pointRadii);
            Gizmos.DrawSphere(_bezier.controlA, _pointRadii);
            
            Gizmos.DrawLine(_bezier.anchorB, _bezier.controlB);
            Gizmos.DrawSphere(_bezier.controlB, _pointRadii);
            Gizmos.DrawSphere(_bezier.anchorB, _pointRadii);
        }

        public static void DrawPointTransform(PointTransform _transform) {
            ExtraGizmos.DrawAxisArrows(_transform.position, _transform.Rotation, _transform.scale, Color.red, Color.green, Color.blue);
        }

        #region Interface Drawers
        public static void DrawPolygonShape(IPolygon _polygon, float _pointRadii = 0.03125f) {
            Line[] edges = _polygon.GetEdges();
            foreach(Line edge in edges) { 
                DrawLine(edge, _pointRadii);
            }
        }

        public static void DrawInterpolationShape(IInterpolationShape _shape, int _resolution = 32) {
            float timeStep = 1.0f / (_resolution-1);
            Vector3 prevPoint = _shape.EvaluatePositionAtTime(0.0f);
            for(int i = 1; i < _resolution; i++) {
                Vector3 currentPoint = _shape.EvaluatePositionAtTime(i * timeStep);
                Gizmos.DrawLine(prevPoint, currentPoint);
                prevPoint = currentPoint;
            }
        }
        #endregion
    }
}