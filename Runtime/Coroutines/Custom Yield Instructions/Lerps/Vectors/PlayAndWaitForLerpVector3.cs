using System;
using UnityEngine;

namespace MaroonSeal.Routines
{
    public class PlayAndWaitForLerpVector3 : PlayAndWaitForLerp
    {
        Action<Vector3> vectorSetPredicate;

        Vector3 startVector;
        Vector3 targetVector;

        #region Constructors
        public PlayAndWaitForLerpVector3(Action<Vector3> _vectorSetPredicate, float _duration, Vector3 _startVector, Vector3 _targetVector) : base(null, _duration)
        {
            vectorSetPredicate = _vectorSetPredicate;

            startVector = _startVector;
            targetVector = _targetVector;

            lerpFunction = (cntx) => vectorSetPredicate.Invoke(Vector3.Lerp(startVector, targetVector, cntx));
        }
        #endregion
    }
}