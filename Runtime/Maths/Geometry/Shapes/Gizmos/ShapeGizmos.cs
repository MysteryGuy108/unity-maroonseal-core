using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    public static class ShapeGizmos
    {
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

        #region Line
        public static void DrawLine(Line _line, float _pointRadii = 0.03125f) {
            Gizmos.DrawLine(_line.start, _line.end);
            if (_pointRadii <= 0.0f) { return; }
            Gizmos.DrawSphere(_line.start, _pointRadii);
            Gizmos.DrawSphere(_line.end, _pointRadii);
        }

        public static void DrawLine(Line2D _line, float _pointRadii = 0.03125f) {
            Gizmos.DrawLine(_line.from, _line.to);
            if (_pointRadii <= 0.0f) { return; }
            Gizmos.DrawSphere(_line.from, _pointRadii);
            Gizmos.DrawSphere(_line.to, _pointRadii);
        }
        #endregion

        #region Triangle
        public static void DrawTriangle(Triangle _triangle, float _pointRadii = 0.03125f) {
            DrawPolygonShape(_triangle, _pointRadii);
        }
        #endregion

        #region Box
        public static void DrawBox(Box _box) {
            Gizmos.DrawWireCube(_box.transform.position, _box.dimensions);
        }
        #endregion

        #region Circles
        public static void DrawCircle(Circle _circle, int _resolution = 32, float _pointSize = 0.125f) {
            DrawInterpolationShape(_circle, _resolution);
            ExtraGizmos.DrawAxis(_circle.transform.position, _circle.transform.Rotation, _circle.radius * _pointSize * Vector2.one);
        }

        public static void DrawCircle(Circle2D _circle, int _resolution = 32, float _pointSize = 0.125f) {
            DrawInterpolationShape(_circle, _resolution);
            ExtraGizmos.DrawAxis(_circle.transform.position,  _circle.transform.Rotation, _circle.radius * _pointSize * Vector2.one);
        }
        #endregion

        #region Arc
        public static void DrawArc(Arc _arc, int _resolution = 32, float _pointRadii = 0.03125f, float _axisSize = 0.125f) {
            DrawInterpolationShape(_arc, _resolution);
            ExtraGizmos.DrawAxis(_arc.transform.position, _arc.transform.Rotation, _arc.radius * _axisSize * Vector2.one);

            Gizmos.DrawSphere(_arc.EvaluatePositionAtTime(0.0f), _pointRadii);
            Gizmos.DrawSphere(_arc.EvaluatePositionAtTime(1.0f), _pointRadii);
        }
        #endregion

        #region Conic Section
        public static void DrawConicSection(ConicSection _conicSection, int _resolution = 32, float _pointSize = 0.125f) {
            DrawInterpolationShape(_conicSection, _resolution);
            (Vector3, Vector3) foci = _conicSection.GetFoci();
            
            Vector2 size = _conicSection.minRadius * _pointSize * Vector2.one;
            ExtraGizmos.DrawAxis(foci.Item1, _conicSection.transform.Rotation, size);
            ExtraGizmos.DrawAxis(foci.Item2, _conicSection.transform.Rotation, size);
        }
        #endregion

        #region Sphere
        public static void DrawSphere(Sphere _sphere, float _pointSize = 0.125f) {
            Gizmos.DrawWireSphere(_sphere.transform.position, _sphere.radius);
            ExtraGizmos.DrawAxis(_sphere.transform.position, _sphere.transform.Rotation, _pointSize * _sphere.radius * Vector3.one);
        }
        #endregion

        #region Capsule
        public static void DrawCapsule(Capsule _capsule, int _resolution = 32, float _pointSize = 0.125f)
        {
            (Vector3, Vector3) foci = _capsule.GetFoci();
            Vector3 delta = foci.Item1 - foci.Item2;

            Vector3 upVector = _capsule.axis == Capsule.Axis.ZAxis ? _capsule.transform.Up : _capsule.transform.Forward;
            PointTransform hemisphere1 = new(foci.Item1, Quaternion.LookRotation(upVector, delta), _capsule.transform.scale);
            PointTransform hemisphere2 = new(foci.Item2, Quaternion.LookRotation(upVector, -delta), _capsule.transform.scale);

            Circle circle1 = DrawHemisphere(hemisphere1, _capsule.radius, _resolution, _pointSize);
            Circle circle2 = DrawHemisphere(hemisphere2, _capsule.radius, _resolution, _pointSize);

            Gizmos.DrawLine(circle1.EvaluatePositionAtTime(0.0f), circle2.EvaluatePositionAtTime(0.5f));
            Gizmos.DrawLine(circle1.EvaluatePositionAtTime(0.25f), circle2.EvaluatePositionAtTime(0.25f));
            Gizmos.DrawLine(circle1.EvaluatePositionAtTime(0.5f), circle2.EvaluatePositionAtTime(0.0f));
            Gizmos.DrawLine(circle1.EvaluatePositionAtTime(0.75f), circle2.EvaluatePositionAtTime(0.75f));
        }

        static Circle DrawHemisphere(PointTransform _transform, float _radius, int _resolution = 32, float _pointSize = 0.125f)
        {
            PointTransform circleTransform = new(_transform.position, _transform.Rotation, _transform.scale) {
                Forward = _transform.Up,
                Up = _transform.Forward
            };
            Circle circle = new(circleTransform, _radius);
            ShapeGizmos.DrawCircle(circle, _resolution, _pointSize);

            Arc arc1 = new(_transform, _radius, 00.0f, 180.0f);
            arc1.transform.Forward = _transform.Right;

            Arc arc2 = new(_transform, _radius, 00.0f, 180.0f);
            arc2.transform.Forward = _transform.Forward;

            ShapeGizmos.DrawArc(arc1, _resolution /2, 0.0f, 0.0f);
            ShapeGizmos.DrawArc(arc2, _resolution /2, 0.0f, 0.0f);

            return circle;
        }
        #endregion

        #region Cubic Bezier
        public static void DrawCubicBezier(CubicBezier _bezier, int _resolution = 32, float _pointRadii = 0.03125f) {
            DrawInterpolationShape(_bezier, _resolution);

            Gizmos.DrawLine(_bezier.anchorA, _bezier.controlA);
            Gizmos.DrawSphere(_bezier.anchorA, _pointRadii);
            Gizmos.DrawSphere(_bezier.controlA, _pointRadii);
            
            Gizmos.DrawLine(_bezier.anchorB, _bezier.controlB);
            Gizmos.DrawSphere(_bezier.controlB, _pointRadii);
            Gizmos.DrawSphere(_bezier.anchorB, _pointRadii);
        }
        #endregion

        #region Point Transform
        public static void DrawPointTransform(PointTransform _transform) {
            ExtraGizmos.DrawAxisArrows(_transform.position, _transform.Rotation, _transform.scale, Color.red, Color.green, Color.blue);
        }
        #endregion
    }
}