using System;
using UnityEngine;

namespace MaroonSeal.Routines
{
    public class PlayAndWaitMoveTowards : PlayAndWaitMoveTowardsBase<float>
    {
        #region Constructors
        public PlayAndWaitMoveTowards(Func<float> _getPredicate, Action<float> _setPredicate, float _target, float _step) 
            : base(_getPredicate, _setPredicate, _target, _step, (a, b, t) => Mathf.MoveTowards(a, b, t))
        { }
        #endregion
    }
}