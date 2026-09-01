using UnityEngine;

namespace MaroonSeal.Maths
{
    public abstract class Noise1D : INoise<float>
    {
        [field : SerializeField] public uint Seed { get; set; }

        public float amplitude;
        public float scale;
        public float offset;

        #region INoise
        public float Sample(float _sample) => GetRawSample((_sample + offset) * scale) * amplitude;
        #endregion

        protected abstract float GetRawSample(float _sample);
    }
}
