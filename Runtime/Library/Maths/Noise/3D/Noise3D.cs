using UnityEngine;

namespace MaroonSeal.Maths
{
    public abstract class Noise3D : INoise<Vector3>
    {
        [field : SerializeField] public uint Seed { get; set; }

        public float amplitude;
        public float scale;
        public Vector3 offset;

        #region Constructor
        protected Noise3D(uint _seed) { Seed = _seed; }
        #endregion

        protected abstract float GetRawSample(Vector3 _sample);

        public float Sample(Vector3 _sample) => GetRawSample((_sample + offset) * scale) * amplitude;
        public float Sample(float _x, float _y, float _z) => GetRawSample(new(_x, _y, _z));
    }
}
