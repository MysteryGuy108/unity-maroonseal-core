
using UnityEngine;

using MaroonSeal.Maths;
using MaroonSeal.DataStructures;
using MaroonSeal.DataStructures.LUTs;
using System;

namespace MaroonSeal.Maths.Geometry.Paths
{
    abstract public class LUTPath : ShapePath
    {
        public override float Length => distanceLUT == null ? 0.0f : distanceLUT[^1].Value;
        [Space]
        [SerializeField][Min(2)] protected int lutResolution;
        [SerializeField] protected FloatLookupTable distanceLUT = new();


        public override float GetDistanceAtTime(float _t) => distanceLUT.EvaluateKey(_t);
        public override float GetTimeAtDistance(float _distance) => distanceLUT.EvaluateValue(_distance);

        public override float GetTimeClosestToPosition(Vector3 _position) => FindLocalMinimum(0.0f, 1.0f, _position);

        override public void Refresh() {
            distanceLUT ??= new();

            //distanceLUT.AddPath(this, lutResolution);
        }

        override public void Clear() => distanceLUT?.Clear();

        #region PathLUT
        public float FindLocalMinimum(float _minT, float _maxT, Func<float, float> _timeWeightFunction, float _e = 0.0001f) {
            float min = _minT;
            float max = _maxT;
            float mid;
        
            do {
                mid = (max + min) / 2.0f;

                float minWeight = _timeWeightFunction(mid-_e);
                float maxWeight = _timeWeightFunction(mid+_e);

                if (minWeight < maxWeight) { max = mid; }
                else{ min = mid; }

            } while (max - min > _e);

            return mid;
        }

        public float FindLocalMinimum(float _minT, float _maxT, Vector3 _position, float _e = 0.0001f) {
            return this.FindLocalMinimum(_minT, _maxT, GetSqrDistanceAtTime, _e);

            float GetSqrDistanceAtTime(float _t) => (this.GetPositionAtTime(_t) - _position).sqrMagnitude;
        }
        #endregion
    }
}