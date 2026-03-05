using System;
using UnityEngine;

namespace MaroonSeal
{
    public class PlayAndWaitForLerpQuaternion : PlayAndWaitForLerp
    {
        Action<Quaternion> quaternionSetPredicate;

        Quaternion startQuaternion;
        Quaternion targetQuaternion;

        #region Constructors
        public PlayAndWaitForLerpQuaternion(Action<Quaternion> _vectorSetPredicate, float _duration, Quaternion _startQuaternion, Quaternion _targetQuaternion) : base(null, _duration)
        {
            quaternionSetPredicate = _vectorSetPredicate;

            startQuaternion = _startQuaternion;
            targetQuaternion = _targetQuaternion;

            lerpFunction = (cntx) => quaternionSetPredicate.Invoke(Quaternion.Lerp(startQuaternion, targetQuaternion, cntx));
        }
        #endregion
    }
}