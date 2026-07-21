using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal {
    static public class ExtraGizmos
    {
        #region Arrows
        public static void DrawArrowHead(Vector3 _position, Vector3 _direction, float _headLength = 1.5f, float _headAngle = 30.0f, Vector3? _up = null) {

            Quaternion directionRotation;
            Vector3 arrowUp = _up ?? Vector3.up;
            directionRotation = _direction.magnitude <= 0.0f ? Quaternion.identity : Quaternion.LookRotation(_direction, arrowUp);

            Vector3 right = directionRotation * Quaternion.Euler(0,180+_headAngle,0) * new Vector3(0,0,1);
            Vector3 left = directionRotation * Quaternion.Euler(0,180-_headAngle,0) * new Vector3(0,0,1);
            
            Gizmos.DrawRay(_position, right * _headLength);
            Gizmos.DrawRay(_position, left * _headLength);
        }

        public static void DrawArrow(Vector3 _position, Vector3 _target, float _arrowHeadLength = 1.5f, float _arrowHeadAngle = 30.0f, Vector3? _up = null) {
            Vector3 delta = _target - _position;

            if (delta.magnitude <= 0.0f) { return; }

            Gizmos.DrawRay(_position, delta);
            
            DrawArrowHead(_target, delta.normalized, _arrowHeadLength, _arrowHeadAngle, _up);
        }

        public static void DrawBiDirectionalArrow(Vector3 pos, Vector3 target, float arrowHeadLength = 1.5f, float arrowHeadAngle = 30.0f, Vector3? _up = null) {
            Vector3 midPoint = Vector3.Lerp(pos, target, 0.5f);
            DrawArrow(midPoint, target, arrowHeadLength, arrowHeadAngle, _up);
            DrawArrow(midPoint, pos, arrowHeadLength, arrowHeadAngle, _up);
        }

        public static void DrawArrowTarget(Vector3 _pos) {
            DrawArrow((Vector3.right * 1.5f) + _pos, (Vector3.right * 0.125f) + _pos, 0.5f);
            DrawArrow((Vector3.right * -1.5f) + _pos, (Vector3.right * -0.125f) + _pos, 0.5f);
            DrawArrow((Vector3.forward * 1.5f) + _pos, (Vector3.forward * 0.125f) + _pos, 0.5f);
            DrawArrow((Vector3.forward * -1.5f) + _pos, (Vector3.forward * -0.125f) + _pos, 0.5f);
        }
        #endregion

        #region Cross
        public static void DrawCross(Vector3 _pos, float _size) {
            Vector3 tr = new Vector3(1.0f, 0.0f, 1.0f).normalized * _size;
            Vector3 tl = new Vector3(-1.0f, 0.0f, 1.0f).normalized * _size;

            Vector3 br = new Vector3(1.0f, 0.0f, -1.0f).normalized * _size;
            Vector3 bl = new Vector3(-1.0f, 0.0f, -1.0f).normalized * _size;

            Gizmos.DrawLine(_pos + bl, _pos + tr);
            Gizmos.DrawLine(_pos + br, _pos + tl);
        }

        public static void DrawCircleCross(Vector3 _pos, float _crossSize = 1.0f, float _circleRadius = 1.0f, int _resolution = 32) {
            DrawWireCircle(_pos, _circleRadius, _resolution);
            DrawCross(_pos, _crossSize);
        }
        #endregion

        #region Axis
        public static void DrawAxis(Vector3 _pos, Quaternion? _rotation = null, Vector3? _size = null, 
                                    Color? _xColour = null, Color? _yColour = null, Color? _zColour = null,
                                    bool? _negativeX = null, bool? _negativeY = null, bool? _negativeZ = null) {
            
            Quaternion rotation = _rotation ?? Quaternion.identity;
            Vector3 size = _size ?? Vector3.one;

            Vector3 right = rotation * (size.x * 0.5f * Vector3.right);
            Vector3 up = rotation * (size.y * 0.5f * Vector3.up);
            Vector3 forward = rotation * (size.z * 0.5f * Vector3.forward);
            
            Gizmos.color = _xColour ?? Gizmos.color;
            if (_negativeX ?? true) { Gizmos.DrawLine(_pos - right, _pos + right); }
            else { Gizmos.DrawLine(_pos, _pos + right); }
            
            Gizmos.color = _yColour ?? Gizmos.color;
            if (_negativeY ?? true) { Gizmos.DrawLine(_pos - up, _pos + up); }
            else { Gizmos.DrawLine(_pos, _pos + up); }

            Gizmos.color = _zColour ?? Gizmos.color;
            if (_negativeZ ?? true) { Gizmos.DrawLine(_pos - forward, _pos + forward); }
            else { Gizmos.DrawLine(_pos, _pos + forward); }
        }

        public static void DrawAxisArrows(Vector3 _pos, Quaternion? _rotation = null, Vector3? _size = null, Color? _xColour = null, Color? _yColour = null, Color? _zColour = null) {
            Quaternion rotation = _rotation ?? Quaternion.identity;
            Vector3 size = _size ?? Vector3.one;

            Vector3 right = rotation * (size.x * 0.5f * Vector3.right);
            Vector3 up = rotation * (size.y * 0.5f * Vector3.up);
            Vector3 forward = rotation * (size.z * 0.5f * Vector3.forward);
            
            Gizmos.color = _xColour ?? Gizmos.color;
            DrawArrow(_pos - right, _pos + right, 0.1f, 40.0f);
            Gizmos.color = _yColour ?? Gizmos.color;
            DrawArrow(_pos - up, _pos + up, 0.1f, 40.0f);
            Gizmos.color = _zColour ?? Gizmos.color;
            DrawArrow(_pos - forward, _pos + forward, 0.1f, 40.0f);
        }
        #endregion
 
        public static void DrawWireCircle(Vector3 _pos, float _radius, int _resolution = 32) {
            DrawWireArc(_pos, _radius, 0.0f, Mathf.PI * 2.0f, _resolution);
        }

        public static void DrawWireArc(Vector3 _pos, float _radius, float _startTheta, float _endTheta, int _resolution = 32, bool _inDegrees = true) {
            
            if (_inDegrees) { _startTheta *= Mathf.Deg2Rad; _endTheta *= Mathf.Deg2Rad; }

            float thetaDelta = Mathf.DeltaAngle(_startTheta, _endTheta);
            int segments = Mathf.Abs(thetaDelta) <= 0 ? _resolution : _resolution - 1;

            for(int i = 0; i < segments; i++) {
                float theta0 = i / (float)segments;
                float theta1 = (i + 1) % _resolution / (float)segments;

                theta0 = (theta0 * thetaDelta) + _startTheta;
                theta1 = (theta1 * thetaDelta) + _startTheta;

                Vector3 p0 = new Vector3(Mathf.Cos(theta0), 0.0f, Mathf.Sin(theta0)) * _radius;
                Vector3 p1 = new Vector3(Mathf.Cos(theta1), 0.0f, Mathf.Sin(theta1)) * _radius;
                
                Gizmos.DrawLine(_pos + p0, _pos + p1);
            }
        }

        public static void DrawFlag(Vector3 _pos, float _height, Vector2 _flagSize) {
            Vector3 poleTop = _pos + (Vector3.up * _height);
            Gizmos.DrawLine(_pos, poleTop);
            
            Vector3 flagBoxSize = new(_flagSize.x, _flagSize.y, 0.0f);
            Vector3 flagBoxCentre = poleTop + (flagBoxSize / 2.0f);
            Gizmos.DrawWireCube(flagBoxCentre, flagBoxSize);
        }
    }
}