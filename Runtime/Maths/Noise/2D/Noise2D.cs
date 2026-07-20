using System;
using UnityEngine;

namespace MaroonSeal.Maths
{
    public abstract class Noise2D : INoise<Vector2>
    {
        [field : SerializeField] public uint Seed { get; set; }

        public float amplitude;
        public float scale;
        public Vector2 offset;

        #region Constructor
        public Noise2D(uint _seed) { Seed = _seed; }
        public Noise2D(int _seed) : this((uint)_seed) {}
        public Noise2D() : this(NoiseHasher.Combine(0, DateTime.Now.Millisecond)) {}
        #endregion
        
        #region INoise
        public float Sample(Vector2 _sample) => GetRawSample((_sample + offset) * scale) * amplitude;
        public float Sample(float _x, float _y) => Sample(new(_x, _y));
        #endregion

        #region Noise2D
        protected abstract float GetRawSample(Vector2 _sample);

        virtual public Vector3 SampleNormal(Vector2 _sample)
        {
            float sL = Sample(_sample.x - INoise.epsilon, _sample.y);
            float sR = Sample(_sample.x + INoise.epsilon, _sample.y);
            float sB = Sample(_sample.x , _sample.y - INoise.epsilon);
            float sT = Sample(_sample.x, _sample.y + INoise.epsilon);

            float dx = (sR - sL) / (2.0f * INoise.epsilon);
            float dy = (sT - sB) / (2.0f * INoise.epsilon);

            return new Vector3(-dx, 1.0f, -dy).normalized;
        }
        #endregion
    }
}
