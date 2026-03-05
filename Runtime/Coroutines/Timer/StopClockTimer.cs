using System;
using System.Collections;

using UnityEngine;

namespace MaroonSeal.Routines
{
    public class StopClockTimer : ITimer
    {
        [SerializeField][EditorReadonly] private float elapsedTime;
        private bool isStopped;

        public event Action OnStart;
        public event Action OnStop;
        public event Action OnReset;

        public StopClockTimer(float _startTime)
        {
            elapsedTime = Mathf.Max(_startTime, 0.0f);
        }

        public void Step(float _deltaTime)
        {
            if (isStopped) { OnStop?.Invoke(); return; }
            if (elapsedTime <= 0.0f) { OnStart?.Invoke(); }

            elapsedTime += _deltaTime;
        }

        public void Stop() => isStopped = true;

        #region IEnumerator
        public object Current => null;

        public bool MoveNext()
        {
            Step(Time.deltaTime);
            return isStopped;
        }

        public void Reset()
        {
            isStopped = false;
            elapsedTime = 0.0f;

            OnReset?.Invoke();
        }
        #endregion
    }
}
