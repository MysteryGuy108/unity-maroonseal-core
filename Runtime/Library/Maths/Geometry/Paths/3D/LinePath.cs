
using UnityEngine;

using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    sealed public class LinePath : PrimativePath
    {
        public Line line;
        public bool flipTangent;

        public override bool IsLoop => false;
        public override float Length => line.Length;

        #region Constructors
        public LinePath() { }
        public LinePath(Line _line) { line = _line; }
        public LinePath(Vector3 _start, Vector3 _end) : this(new(_start, _end)) { }
        #endregion

        #region GeometryPath
        public override Transform3D EvaluateTime(float _t)
        {
            Vector3 position = line.EvaluatePointAtTime(_t);
            Vector3 forward = line.EvaluateTangentAtTime(_t);
            forward = forward == Vector3.zero ? Vector3.forward : forward;
            forward = flipTangent ? -forward : forward;
            return new(position) { Forward = forward };
        }

        public override float TimeToDistance(float _time) => Length * Mathf.Clamp01(_time);

        public override float DistanceToTime(float _distance) => Mathf.Clamp01(_distance / Length);

        public override float ClosestTimeToPoint(Vector3 _position)
        {
            Vector3 AB = line.p2 - line.p1;
            Vector3 AV = _position - line.p1;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
        #endregion

        #region LinePath
        public Line GetLine() => line;
        public void SetLine(Line _line) => line = _line;

        public void SetStartPosition(Vector3 _startPosition) { line.p1 = _startPosition; }

        public void SetEndPosition(Vector3 _endPosition) { line.p2 = _endPosition; }
        #endregion
    }
}