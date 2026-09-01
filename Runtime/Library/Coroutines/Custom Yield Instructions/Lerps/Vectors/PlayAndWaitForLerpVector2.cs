using System;
using UnityEngine;

namespace MaroonSeal.Routines
{
    public class PlayAndWaitForLerpVector2 : PlayAndWaitForLerp
    {
        Action<Vector2> vectorSetPredicate;

        Vector2 startVector;
        Vector2 targetVector;

        #region Constructors
        public PlayAndWaitForLerpVector2(Action<Vector2> _vectorSetPredicate, float _duration, Vector2 _startVector, Vector2 _targetVector) : base(null, _duration)
        {
            vectorSetPredicate = _vectorSetPredicate;

            startVector = _startVector;
            targetVector = _targetVector;

            lerpFunction = (cntx) => vectorSetPredicate.Invoke(Vector2.Lerp(startVector, targetVector, cntx));
        }
        #endregion
    }
}