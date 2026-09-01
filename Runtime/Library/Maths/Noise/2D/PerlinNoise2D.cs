using System;
using UnityEngine;

namespace MaroonSeal.Maths
{
    [System.Serializable]
    public class PerlinNoise2D : Noise2D
    {
        #region Sampling
        protected override float GetRawSample(Vector2 _sample)
        {
            // Integer Lattice
            Vector2Int s0 = Vector2Maths.FloorToInt(_sample);
            Vector2Int s1 = s0 + Vector2Int.one;

            // Local Coordinates
            Vector2 local = _sample - s0;

            // Dot products at each corner
            float n00 = GradientDot(Seed, s0.x, s0.y, local.x, local.y);
            float n10 = GradientDot(Seed, s1.x, s0.y, local.x-1, local.y);
            float n01 = GradientDot(Seed, s0.x, s1.y, local.x, local.y-1);
            float n11 = GradientDot(Seed, s1.x, s1.y, local.x-1, local.y-1);

            // Smooth interpolation
            float u = Fade(local.x);
            float v = Fade(local.y);

            float nx0 = Mathf.Lerp(n00, n10, u);
            float nx1 = Mathf.Lerp(n01, n11, u);

            return Mathf.Lerp(nx0, nx1, v);
        }
        #endregion

        // 6t^5 - 15t^4 + 10t^3
        private static float Fade(float _t) => _t * _t * _t * (_t * (_t * 6 - 15) + 10);

        private static float GradientDot(uint _seed, int _ix, float _iy, float dx, float dy)
        {
            NoiseHasher hasher = new(_seed);
            hasher.Add(_ix); hasher.Add(_iy);
            uint hash = hasher.Finish();

            float angle = (hash / (float)uint.MaxValue) * Mathf.PI * 2.0f;

            float gx = MathF.Cos(angle);
            float gy = MathF.Sin(angle);

            return gx * dx + gy * dy;
        }
    }
}
