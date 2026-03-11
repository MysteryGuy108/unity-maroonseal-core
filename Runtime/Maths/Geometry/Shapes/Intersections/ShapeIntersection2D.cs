using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Shapes {
    /*
    public static class ShapeIntersection2D
    {
        // Line and Line
        //https://rosettacode.org/wiki/Find_the_intersection_of_two_lines#C#
        static public Vector2 Calculate(Line _lineA, Line _lineB) {
            float a1 = _lineA.pointB.y - _lineA.pointA.y;
            float b1 = _lineA.pointA.x - _lineA.pointB.x;
            float c1 = a1 * _lineA.pointA.x + b1 * _lineA.pointA.y;

            float a2 = _lineB.pointB.y - _lineB.pointA.y;
            float b2 = _lineB.pointA.x - _lineB.pointB.x;
            float c2 = a2 * _lineB.pointA.x + b2 * _lineB.pointA.y;

            float delta = a1 * b2 - a2 * b1;

            if (delta == 0.0f) { return Vector2.one * Mathf.Infinity; }

            Vector2 intersectPoint = new((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);

            if (!_lineA.IsPositionInBounds(intersectPoint) ||
                !_lineB.IsPositionInBounds(intersectPoint)) {
                return Vector2.one * Mathf.Infinity;
            }

            return intersectPoint;
        }

        // Line and Circle
        //https://stackoverflow.com/questions/23016676/line-segment-and-circle-intersection
        static public List<Vector2> Calculate(Line2D _line, Circle2D _circle) {
            
            float dx, dy, A, B, C, det;

            dx = _line.pointB.x - _line.pointA.x;
            dy = _line.pointB.y - _line.pointA.y;

            A = dx * dx + dy * dy;
            B = 2 * (dx * (_line.pointA.x - _circle.centre.x) + dy * (_line.pointA.y - _circle.centre.y));
            C = (_line.pointA.x - _circle.centre.x) * (_line.pointA.x - _circle.centre.x) + 
                (_line.pointA.y - _circle.centre.y) * (_line.pointA.y - _circle.centre.y) - _circle.radius * _circle.radius;

            det = B * B - 4 * A * C;

            if ((A <= 0.0000001) || (det < 0)) { // No real solutions.
                return new();
            }
            else if (det == 0)  { // One solution.
                float t = -B / (2 * A);
                Vector2 point = new (_line.pointA.x + t * dx, _line.pointA.x + t * dy);
                if (_line.IsPointInBounds(point)) { return new List<Vector2>(1) { point }; }
                else { return new(); }
            }

            float t1 = (-B + Mathf.Sqrt(det)) / (2 * A);
            float t2 = (-B - Mathf.Sqrt(det)) / (2 * A);

            List<Vector2> points = new();

            Vector2 point1 = new(_line.pointA.x + t1 * dx, _line.pointA.y + t1 * dy);
            if (_line.IsPointInBounds(point1)) { points.Add(point1); }

            Vector2 point2 = new(_line.pointA.x + t2 * dx, _line.pointA.y + t2 * dy);
            if (_line.IsPointInBounds(point2)) { points.Add(point2); }

            return points;
        }

        /*
        static public List<Vector2> LineIntersectionWithCapsule(Vector2 _ls, Vector2 _le, Vector2 _cs, Vector2 _ce, float _r) {
            List<Vector2> intersectPoints = new List<Vector2>();

            Vector2 cDelta = (_ce - _cs).normalized;

            // Start circle points
            List<Vector2> circlePoints = Calculate(_ls, _le, _cs, _r);

            if (circlePoints != null) { 
                for(int i = circlePoints.Count-1; i >= 0; i--) {
                    Vector2 circleDirection = (circlePoints[i] - _cs).normalized;
                    if (Vector2.Dot(circleDirection, cDelta) >= 0.0f) {
                        circlePoints.RemoveAt(i);
                    }
                }

                intersectPoints.AddRange(circlePoints); 
            }
            
            // End circle points
            circlePoints = Calculate(_ls, _le, _ce, _r);

            if (circlePoints != null) { 
                for(int i = circlePoints.Count-1; i >= 0; i--) {
                    Vector2 pDir = (circlePoints[i] - _ce).normalized;

                    if (Vector2.Dot(pDir, cDelta) < 0.0f) {
                        circlePoints.RemoveAt(i);
                    }
                }
                intersectPoints.AddRange(circlePoints); 
            }

            // Connecting Lines
            Vector2 tangent = Vector3.Cross(_ce-_cs, Vector3.forward).normalized;

            Vector2 cl1s =  _cs + tangent * _r;
            Vector2 cl1e =  _ce + tangent * _r;

            Vector2 lineIntersectPoint = LineIntersectionWithLine(_ls, _le, cl1s, cl1e);
            if (lineIntersectPoint.magnitude != Mathf.Infinity) { intersectPoints.Add(lineIntersectPoint); }

            Vector2 cl2s =  _cs - tangent * _r;
            Vector2 cl2e =  _ce - tangent * _r;

            lineIntersectPoint = LineIntersectionWithLine(_ls, _le, cl2s, cl2e);
            if (lineIntersectPoint.magnitude != Mathf.Infinity) { intersectPoints.Add(lineIntersectPoint); }

            return intersectPoints.Count == 0 ? null : intersectPoints;
        }
       
    }
     */
}

