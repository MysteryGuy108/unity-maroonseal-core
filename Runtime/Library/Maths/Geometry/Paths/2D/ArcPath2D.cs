using System;
using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths 
{
    [System.Serializable]
    public class ArcPath2D : PrimativePath2D
    {
        [SerializeField] private Arc2D arc;

        public override bool IsLoop => arc.DegreesDelta == 360.0f;
        public override float Length => arc.Length;

        #region Constructors
        public ArcPath2D() { arc = new(Transform2D.Origin, 0.0f, 0.0f, 0.0f); }
        public ArcPath2D(Arc2D _arc) { arc = _arc; }
        public ArcPath2D(Transform2D _transform, float _radius, float _startTheta, float _endTheta) {
            arc = new Arc2D(_transform, _radius, _startTheta, _endTheta);
        }
        #endregion

        #region Path3D
        public override Transform2D EvaluateTime(float _t) {
            Vector2 position = arc.EvaluatePointAtTime(_t);
            Vector3 right = arc.EvaluateTangentAtTime(_t);
            Transform2D point = new(position) { Right = right };
            return point;
        }

        public override float TimeToDistance(float _t) => Length * Mathf.Clamp01(_t);

        public override float DistanceToTime(float _distance) => Mathf.Clamp01(_distance / Length);

        public override float ClosestTimeToPoint(Vector2 _position) {
            Vector2 projectedPosition = arc.Transform.InverseTransformPoint(_position);
            PolarVector2 polar = new(projectedPosition);
            return Mathf.Clamp(polar.theta * Mathf.Rad2Deg, arc.startDegrees, arc.endDegrees) / arc.Length;
        }
        #endregion

        public Arc2D GetArc() => arc;
        public void SetArc(Arc2D _arc) => arc = _arc;
    }
}
