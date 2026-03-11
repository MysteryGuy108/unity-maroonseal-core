
using UnityEngine;

using MaroonSeal.Maths;
using MaroonSeal.Maths.DataStructures;
using MaroonSeal.Maths.Geometry.Paths.LUTs;

namespace MaroonSeal.Maths.Geometry.Paths
{
    abstract public class LUTPath : ShapePath
    {
        public override float Length => distanceLUT == null ? 0.0f : distanceLUT[^1];

        [Space]
        [SerializeField][Min(2)] protected int lutResolution;
        [SerializeField] protected PathFloatLUT distanceLUT = new();


        public override float GetDistanceAtTime(float _t) => distanceLUT.EvaulateValueAtTime(_t);
        public override float GetTimeAtDistance(float _distance) => distanceLUT.EvaulateTimeAtValue(_distance);

        public override float GetTimeClosestToPosition(Vector3 _position) => distanceLUT.FindLocalMinimum(0.0f, 1.0f, this, _position);

        override public void Refresh() {
            distanceLUT ??= new();
            distanceLUT.AddPath(this, lutResolution);
        }

        override public void Clear() => distanceLUT?.Clear();
    }
}