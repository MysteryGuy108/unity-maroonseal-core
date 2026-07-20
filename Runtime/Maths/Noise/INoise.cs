using UnityEngine;

namespace MaroonSeal.Maths
{
    public interface INoise
    {
        public static readonly float epsilon = 0.001f;
        public uint Seed { get; set; }
    }

    public interface INoise<TSample> : INoise
    {
        public float Sample(TSample _sample);
    }
}
