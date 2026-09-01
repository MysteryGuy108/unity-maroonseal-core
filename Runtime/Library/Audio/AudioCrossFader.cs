using UnityEngine;

namespace MaroonSeal.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioCrossFader : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource sourceA;
        [SerializeField] private AudioSource sourceB;

        [Header("Blend")]
        [Range(0f, 1f)]
        [SerializeField] private float blend = 0f; // 0 = fully A, 1 = fully B
        [SerializeField] private bool useEqualPowerCrossfade = true;
        
        [Header("Volume Scaling")]
        [SerializeField] private float maxVolumeA = 1f;
        [SerializeField] private float maxVolumeB = 1f;

        public float Blend
        {
            get => blend;
            set
            {
                blend = Mathf.Clamp01(value);
                ApplyBlend();
            }
        }

        public AudioClip ClipA
        {
            get => sourceA.clip;
            set
            {
                sourceA.clip = value;
                sourceA.time = sourceB.time;
            }
        }

        public AudioClip ClipB
        {
            get => sourceB.clip;
            set
            {
                sourceB.clip = value;
                sourceB.time = sourceA.time;
            }
        }

        #region MonoBehaviour
        private void Awake()
        {
            ApplyBlend();
        }

        private void OnValidate()
        {
            // Keeps things updated when you tweak the slider in the Inspector
            ApplyBlend();
        }
        #endregion

        #region Audio
        public void Play()
        {
            if (sourceA == null || sourceB == null) { return; }
            sourceA.Play();
            sourceB.Play();
        }

        public void Stop()
        {
            if (sourceA == null || sourceB == null) { return; }
            sourceA.Stop();
            sourceB.Stop();
        }
        #endregion

        private void ApplyBlend()
        {
            if (sourceA == null || sourceB == null) { return; }

            float volA, volB;

            if (useEqualPowerCrossfade)
            {
                // Equal-power crossfade avoids a perceived volume dip at the midpoint
                float angle = blend * (Mathf.PI / 2f);
                volA = Mathf.Cos(angle);
                volB = Mathf.Sin(angle);
            }
            else
            {
                // Simple linear crossfade
                volA = 1f - blend;
                volB = blend;
            }

            sourceA.volume = volA * maxVolumeA;
            sourceB.volume = volB * maxVolumeB;
        }

        // Optional convenience methods
        public void SetSources(AudioSource a, AudioSource b)
        {
            sourceA = a;
            sourceB = b;
            ApplyBlend();
        }
    }
}