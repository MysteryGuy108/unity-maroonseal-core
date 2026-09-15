
using UnityEngine;

using MaroonSeal.DataStructures.LUTs;
using MaroonSeal.Maths.Algorithms;

namespace MaroonSeal.Maths.Geometry.Paths 
{
    [System.Serializable]
    public abstract class SamplePathBase<TVector, TTransform> : PathBase<TVector, TTransform> where TTransform : ITransform<TVector>
    {
        public sealed override float Length => Resolution == 0 ? 0.0f : sampleTable.TotalLength;

        [SerializeField][Min(2)] private int resolution = 16;
        public int Resolution => resolution;
        [SerializeField][HideInInspector] CumulativeDistanceTable sampleTable;

        #region Constructor
        public SamplePathBase(int _resolution) {
            resolution = Mathf.Max(2, _resolution);
            sampleTable = new();
        }

        public SamplePathBase() : this(16) {}
        #endregion

        #region PathBase
        public override float TimeToDistance(float _time)
        {
            EnsureClean();
            return sampleTable.EvaluateTime(_time);
        }

        public override float DistanceToTime(float _distance)
        {
            EnsureClean();
            return sampleTable.EvaluateDistance(_distance);
        }

        public override float ClosestTimeToPoint(TVector _point) => NumericMaths.FindLocalMinimum(0.0f, 1.0f, (cntx) => EvaluateTime(cntx).SqrDistanceTo(_point));
        
        protected override void OnEnsureClean() => sampleTable.RebuildSamples(resolution, DistanceBetweenSamples);

        override public void Clear() => sampleTable.Clear();
        #endregion

        protected float DistanceBetweenSamples(int _s0, int _s1)
        {
            float t0 = sampleTable.TimeAt(_s0);
            float t1 = sampleTable.TimeAt(_s1);

            return DistanceBetweenPathPoints(EvaluateTime(t0), EvaluateTime(t1));
        }

        abstract protected float DistanceBetweenPathPoints(TTransform _p0, TTransform _p1);
    }

    [System.Serializable]
    abstract public class SamplePath : SamplePathBase<Vector3, Transform3D>, IPath3D
    {
        [SerializeField][Range(-180.0f, 180.0f)] protected float startRoll;
        [SerializeField][Range(-180.0f, 180.0f)] protected float endRoll;

        #region Constructor
        public SamplePath(int _resolution) : base(_resolution){}
        public SamplePath() : this(2) {}
        #endregion

        sealed override protected float DistanceBetweenPathPoints(Transform3D _p0, Transform3D _p1) =>
            Vector3.Distance(_p0.position, _p1.position);
    }

    [System.Serializable]
    abstract public class SamplePath2D : SamplePathBase<Vector2, Transform2D>, IPath2D
    {
        #region Constructor
        public SamplePath2D(int _resolution) : base(_resolution){}
        public SamplePath2D() : this(2) {}
        #endregion

        sealed override protected float DistanceBetweenPathPoints(Transform2D _p0, Transform2D _p1) =>
            Vector2.Distance(_p0.position, _p1.position);
    }
}
