
using UnityEngine;

using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.Geometry.Paths {
    [System.Serializable]
    sealed public class LinePath : ShapePath
    {
        [SerializeField] private Line line;
        public bool flipTangent;

        public override bool IsLoop => false;
        public override float Length => line.GetLength();

        #region Constructors
        public LinePath() { }
        public LinePath(Line _line) { line = _line; }
        public LinePath(Vector3 _start, Vector3 _end) : this(new(_start, _end)) { }
        #endregion

        #region GeometryPath
        public override PointTransform GetPointAtTime(float _t)
        {
            Vector3 position = line.EvaluatePositionAtTime(_t);
            Vector3 forward = line.EvaluateTangentAtTime(_t);
            forward = forward == Vector3.zero ? Vector3.forward : forward;
            forward = flipTangent ? -forward : forward;
            return new(position) { Forward = forward };
        }

        public override float GetDistanceAtTime(float _t)
        {
            return Length * Mathf.Clamp01(_t);
        }

        public override float GetTimeAtDistance(float _distance)
        {
            return Mathf.Clamp01(_distance / Length);
        }

        public override float GetTimeClosestToPosition(Vector3 _position)
        {
            Vector3 AB = line.end - line.start;
            Vector3 AV = _position - line.start;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
        #endregion

        #region LinePath
        public Line GetLine() => line;
        public void SetLine(Line _line) => line = _line;

        public void SetStartPosition(Vector3 _startPosition) { line.start = _startPosition; }

        public void SetEndPosition(Vector3 _endPosition) { line.end = _endPosition; }
        #endregion
    }
}