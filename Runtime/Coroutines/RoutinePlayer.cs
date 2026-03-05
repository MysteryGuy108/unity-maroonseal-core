using System.Collections;
using UnityEngine;

namespace MaroonSeal
{
    public class RoutinePlayer : MonoBehaviour
    {
        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }

        [SerializeField] private bool allowCoroutineInterrupts = true;
        IEnumerator currentRoutine;

        public bool CanStartCoroutine => allowCoroutineInterrupts || !IsRunning;

        #region MonoBehaviour
        new public Coroutine StartCoroutine(IEnumerator _enumerator)
        {
            if (!CanStartCoroutine) { return null; }
            if (_enumerator == null) { return null; }
            StopAllCoroutines();

            return base.StartCoroutine(BaseRoutine(_enumerator));
        }

        new public void StopAllCoroutines()
        {
            base.StopAllCoroutines();
            IsRunning = false;
            IsPaused = false;
        }
        #endregion

        private IEnumerator BaseRoutine(IEnumerator _routine)
        {
            IsRunning = true;
            
            currentRoutine = _routine;
            yield return currentRoutine;
            
            IsRunning = false;
        }

        public void Play(IEnumerator _enumerator)
        {
            if (IsRunning) { IsPaused = false; }
            else { StartCoroutine(_enumerator); }
        }

        public void Play() => Play(currentRoutine);

        public void Pause() => IsPaused = IsRunning;

        public void Stop() => StopAllCoroutines();
    }
}