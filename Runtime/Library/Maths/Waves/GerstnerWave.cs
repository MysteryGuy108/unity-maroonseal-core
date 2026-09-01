using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.Waves {

    [System.Serializable]
    public class GerstnerWave
    {
        [SerializeField] private float steepness;
        [SerializeField] private float wavelength;
        [SerializeField] private float gravity;
        [SerializeField] private Vector2 direction;
        public Vector2 Direction { get { return direction.normalized; } }

        public void SetDirection(Vector2 _direction) {
            direction = _direction;
        }

        public Vector3 GetOffset(Vector3 _position, float _time) {
            return GerstnerWaveOffset(_position, steepness, wavelength, gravity, direction, _time);
        }

        public float GetWavePeriod() {
            float k = Mathf.PI * 2.0f / wavelength;
            float c = Mathf.Sqrt(gravity / k);
            return 1.0f / (c / wavelength);
        }

        static public Vector3 GerstnerWaveOffset(Vector3 _position, float _steepness, float _waveLength, float _gravity, Vector2 _direction, float _time) {
            _direction.Normalize();
            float k = Mathf.PI * 2.0f / _waveLength;
            float c = Mathf.Sqrt(_gravity / k);
            float d = Vector2.Dot(new Vector2(_position.x, _position.z), _direction); 
            float f = k * (d - c * _time);
            float a = _steepness / k;

            return new Vector3() {
                x = _direction.x * (a * Mathf.Cos(f)),
                y = a * Mathf.Sin(f),
                z = _direction.y * (a * Mathf.Cos(f)),
            };
        }
    }
}