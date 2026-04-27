using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct CylindricalCoordinate
    {
        public float theta;
        public float radius;
        public float height;

        public enum Axis {x, y ,z}
        [SerializeField] private Axis axis;

        public CylindricalCoordinate(float _theta, float _radius, float _height, Axis _axis) {
            theta = _theta;
            radius = _radius;
            height = _height;
            axis = _axis;
        }

        public Vector3 Point { 
            get
            {
                float radTheta = theta * Mathf.Deg2Rad;

                switch(axis) {
                    case Axis.x:
                        return new Vector3(
                            height, 
                            Mathf.Sin(radTheta) * radius,
                            Mathf.Cos(radTheta) * radius);

                    case Axis.y:
                        return new Vector3(
                            Mathf.Cos(radTheta) * radius,
                            height, 
                            Mathf.Sin(radTheta) * radius);

                    case Axis.z:
                        return new Vector3(
                            Mathf.Cos(radTheta) * radius,
                            Mathf.Sin(radTheta) * radius,
                            height);
                    default:
                        return Vector3.zero;
                }
            }
        }

        public Quaternion Orientation {
            get 
            { 
                switch(axis) {
                    case Axis.x:
                        return Quaternion.LookRotation(-Point, Vector3.right);
                    case Axis.z:
                        return Quaternion.LookRotation(-Point, Vector3.forward);
                    default:
                        return Quaternion.LookRotation(-Point, Vector3.up);
                }
            }
        }

        static public CylindricalCoordinate Lerp(CylindricalCoordinate _a, CylindricalCoordinate _b, float _t) {

            return new CylindricalCoordinate(
                Mathf.LerpAngle(_a.theta, _b.theta, _t),
                Mathf.Lerp(_a.radius, _b.radius, _t),
                Mathf.Lerp(_a.height, _b.height, _t), _a.axis);
        }

        static public CylindricalCoordinate PointToCylinderCoordinate(Vector3 _point, Axis _axis) {
            return _axis switch {
                Axis.x => new CylindricalCoordinate(
                                        Mathf.Atan2(_point.y, _point.z) * Mathf.Rad2Deg,
                                        Mathf.Sqrt(_point.y * _point.y + _point.z * _point.z),
                                        _point.x, Axis.x),
                Axis.y => new CylindricalCoordinate(
                                        Mathf.Atan2(_point.z, _point.x) * Mathf.Rad2Deg,
                                        Mathf.Sqrt(_point.x * _point.x + _point.z * _point.z),
                                        _point.y, Axis.y),
                Axis.z => new CylindricalCoordinate(
                                        Mathf.Atan2(_point.y, _point.x) * Mathf.Rad2Deg,
                                        Mathf.Sqrt(_point.x * _point.x + _point.y * _point.y),
                                        _point.z, Axis.z),
                _ => new CylindricalCoordinate(0.0f, 0.0f, 0.0f, Axis.y),
            };
        }

        static public CylindricalCoordinate GetLocalCoordinate(Transform _transform, CylindricalCoordinate _coord) {
            Vector3 point = _transform.InverseTransformPoint(_coord.Point);
            return PointToCylinderCoordinate(point, _coord.axis);
        }

        static public CylindricalCoordinate GetGlobalCoordinate(Transform _transform, CylindricalCoordinate _coord, Axis _axis) {
            Vector3 point = _transform.TransformPoint(_coord.Point);
            return PointToCylinderCoordinate(point, _axis);
        }
    }
}


