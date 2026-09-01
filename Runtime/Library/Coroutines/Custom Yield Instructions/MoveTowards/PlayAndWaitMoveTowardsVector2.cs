using System;
using UnityEngine;

namespace MaroonSeal.Routines
{
    public class PlayAndWaitMoveTowardsVector2 : PlayAndWaitMoveTowardsBase<Vector2>
    {
        #region Constructors
        public PlayAndWaitMoveTowardsVector2(Func<Vector2> _getPredicate, Action<Vector2> _setPredicate, Vector2 _target, float _step) 
            : base(_getPredicate, _setPredicate, _target, _step, (a, b, t) => Vector2.MoveTowards(a, b, t))
        {

        }
        #endregion
    }
}