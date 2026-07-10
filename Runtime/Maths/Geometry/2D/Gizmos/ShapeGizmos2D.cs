using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    public static class ShapeGizmos2D
    {
        public static void DrawLine(Line2D _line, float _pointRadii = 0.03125f) {
            Gizmos.DrawLine(_line.p1, _line.p2);
            if (_pointRadii <= 0.0f) { return; }
            Gizmos.DrawSphere(_line.p1, _pointRadii);
            Gizmos.DrawSphere(_line.p2, _pointRadii);
        }
    }
}
