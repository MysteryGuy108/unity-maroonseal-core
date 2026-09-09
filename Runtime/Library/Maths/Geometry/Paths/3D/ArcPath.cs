using System;
using UnityEngine;

using MaroonSeal.Maths;
using MaroonSeal.Maths.Geometry.Shapes;


namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    sealed public class ArcPath : PrimativePath
    {
        [SerializeField] private Arc arc;

        public override bool IsLoop => false;
        public override float Length => arc.Length;

        #region Constructors
        public ArcPath() { arc = new(Transform3D.Origin, 0.0f, 0.0f, 0.0f); }
        public ArcPath(Arc _arc) { arc = _arc; }
        public ArcPath(Transform3D _transform, float _radius, float _startTheta, float _endTheta) {
            arc = new Arc(_transform, _radius, _startTheta, _endTheta);
        }
        #endregion

        #region Path3D
        public override Transform3D EvaluateTime(float _t) {
            Vector3 position = arc.EvaluatePointAtTime(_t);
            Vector3 forward = arc.EvaluateTangentAtTime(_t);
            Transform3D point = new(position) { Forward = forward };
            return point;
        }

        public override float TimeToDistance(float _t) => Length * Mathf.Clamp01(_t);

        public override float DistanceToTime(float _distance) => Mathf.Clamp01(_distance / Length);

        public override float ClosestTimeToPoint(Vector3 _position) {
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