using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths
{
    [System.Serializable]
    public class LinePath2D : PrimativePath2D
    {
        public Line2D line;
        public bool flipTangent;

        public override bool IsLoop => false;
        public override float Length => line.Length;

        #region Constructors
        public LinePath2D() { }
        public LinePath2D(Line2D _line) { line = _line; }
        public LinePath2D(Vector2 _start, Vector2 _end) : this(new(_start, _end)) { }
        #endregion

        #region GeometryPath
        public override Transform2D EvaluateTime(float _t)
        {
            Vector2 position = line.EvaluatePointAtTime(_t);
            Vector2 right = line.EvaluateTangentAtTime(_t);
            right = right == Vector2.zero ? Vector3.right : right;

            right = flipTangent ? -right : right;
            return new(position) { Right = right };
        }

        public override float TimeToDistance(float _time) => Length * Mathf.Clamp01(_time);

        public override float DistanceToTime(float _distance) => Mathf.Clamp01(_distance / Length);

        public override float ClosestTimeToPoint(Vector2 _position)
        {
            Vector2 AB = line.p2 - line.p1;
            Vector2 AV = _position - line.p1;
            return Vector2.Dot(AV, AB) / Vector2.Dot(AB, AB);
        }
        #endregion

        #region LinePath
        public Line2D GetLine() => line;
        public void SetLine(Line2D _line) => line = _line;

        public void SetStartPosition(Vector2 _startPosition) { line.p1 = _startPosition; }
        public void SetEndPosition(Vector2 _endPosition) { line.p2 = _endPosition; }
        #endregion
    }
}
