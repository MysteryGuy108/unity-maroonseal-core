using System;
using System.Collections;

using UnityEngine;

namespace MaroonSeal.Routines
{
    [System.Serializable]
    public class CountdownTimer : ITimer, ISerializationCallbackReceiver
    {
        [SerializeField] private float setTime;
        [SerializeField] private float secondsLeft;

        public float SecondsLeft => secondsLeft;
        public float NormalisedSecondsLeft { get; private set; }

        public float SecondsElapsed => setTime - secondsLeft;
        public float NormalisedSecondsElapsed => 1.0f - NormalisedSecondsLeft;

        public event Action OnStart;
        public event Action OnEnd;
        public event Action OnReset;

        public bool HasFinished => secondsLeft == 0.0f;

        public CountdownTimer(float _startTime)
        {
            setTime = _startTime;
            Reset();
        }

        public void Step(float _deltaTime)
        {
            if (HasFinished) { return; }
            if (secondsLeft == setTime) { OnStart?.Invoke(); }

            secondsLeft = Mathf.Max(secondsLeft - _deltaTime, 0.0f);
            NormalisedSecondsLeft = Mathf.InverseLerp(0.0f, setTime, SecondsLeft);

            if (HasFinished) { OnEnd?.Invoke(); }
        }

        public void SetFinished() => secondsLeft = 0.0f;

        #region IEnumerator
        public object Current => null;

        public bool MoveNext()
        {
            Step(Time.deltaTime);
            return secondsLeft == 0.0f;
        }

        public void Reset()
        {
            setTime = Mathf.Max(setTime, 0.0f);
            secondsLeft = setTime;

            OnReset?.Invoke();
        }
        #endregion

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize()
        {
            setTime = Mathf.Max(setTime, 0.0f);
            secondsLeft = setTime;
        }
        #endregion
    }
}
