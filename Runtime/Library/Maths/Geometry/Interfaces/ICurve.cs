using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface ICurve<TVector> : IGeometry
    {
        public bool IsLoop { get; }

        public TVector EvaluatePositionAtTime(float _t);
        public TVector EvaluateTangentAtTime(float _t);

        public float ClosestTimeToPosition(TVector _position);
        public TVector ClosestPointToPosition(TVector _position) => EvaluatePositionAtTime(ClosestTimeToPosition(_position));
    }

    public interface IPolarCurve<TVector> : ICurve<TVector>
    {
        public TVector EvaluatePointAtTheta(float _theta);
        public TVector EvaluateTangentAtTheta(float _theta);

        static public float TimeToTheta(float _time) => Mathf.PI * 2.0f * Mathf.Clamp01(_time);
        static public float ThetaToTime(float _theta) => Mathf.Clamp01(_theta / (Mathf.PI * 2.0f));
    }

    public interface IArcLengthCurve<TVector> : ICurve<TVector>
    {
        public float Length { get; }

        public float TimeToDistance(float _time) => this.Length * Mathf.Clamp01(_time);
        public float DistanceToTime(float _distance) => Mathf.Clamp01(_distance / Length);
    }

    public interface IParametricCurve<TVector> : ICurve<TVector>
    {
        new public float ClosestTimeToPosition(TVector _position)
            => NumericMaths.FindLocalMinimum(0.0f, 1.0f, (time) => GetDistanceToPointAtTime(time, _position));
        
        public float GetDistanceToPointAtTime(float _time, TVector _position);
    }
}
