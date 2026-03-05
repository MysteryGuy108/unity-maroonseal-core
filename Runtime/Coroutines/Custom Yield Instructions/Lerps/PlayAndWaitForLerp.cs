using System;
using System.Collections;
using UnityEngine;

namespace MaroonSeal
{
    public class PlayAndWaitForLerp : CustomYieldInstruction
    {
        protected Action<float> lerpFunction;
        private float duration;
        private float time;

        #region Constructors
        public PlayAndWaitForLerp(Action<float> _lerpFunction, float _duration)
        {
            lerpFunction = _lerpFunction;
            duration = _duration;
            time = 0.0f;
        }
        #endregion

        public override bool keepWaiting { 
            get {
                float normalisedTime = time / duration;
                lerpFunction?.Invoke(normalisedTime);

                if (time >= duration) { return false; }
                time = Mathf.Min(time + Time.deltaTime, duration);

                return true; 
            } 
        }
    }
}