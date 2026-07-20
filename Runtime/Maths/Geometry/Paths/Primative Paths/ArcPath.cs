using System;
using UnityEngine;

using MaroonSeal.Maths;
using MaroonSeal.Maths.Geometry.Shapes;


namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    sealed public class ArcPath : ShapePath
    {
        [SerializeField] private Arc arc;

        public override bool IsLoop => false;
        public override float Length => arc.Length;

        #region Constructors
        public ArcPath() {}
        public ArcPath(Arc _arc) { arc = _arc; }
        public ArcPath(Transform3D _transform, float _radius, float _startTheta, float _endTheta) {
            arc = new Arc(_transform, _radius, _startTheta, _endTheta);
        }
        #endregion

        #region GeometryPath
        public override Transform3D GetPointAtTime(float _t) {
            Vector3 position = arc.EvaluatePointAtTime(_t);
            Vector3 forward = arc.EvaluateTangentAtTime(_t);
            Transform3D point = new(position, arc.Transform.EulerAngles) { Forward = forward };
            return point;
        }

        public override float GetDistanceAtTime(float _t) {
            return Length * Mathf.Clamp01(_t);
        }

        public override float GetTimeAtDistance(float _distance) {
            return Mathf.Clamp01(_distance / Length);
        }

        public override float GetTimeClosestToPosition(Vector3 _position) {
            throw new NotImplementedException();
            /*
            Vector3 projectedPosition = arc.transform.InverseTransformPosition(_position);
            projectedPosition.z = 0.0f;
            PolarVector2 polar = new(projectedPosition);
            return Mathf.Clamp(polar.theta * Mathf.Rad2Deg, arc.startDegrees, arc.endDegrees) / arc.GetLength();
            */
        }
        #endregion

        public Arc GetArc() => arc;
        public void SetArc(Arc _arc) => arc = _arc;
    }
}