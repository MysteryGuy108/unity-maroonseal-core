using System;
using UnityEngine;

namespace MaroonSeal.Routines
{
    public class PlayAndWaitMoveTowardsQuaternion : PlayAndWaitMoveTowardsBase<Quaternion>
    {
        #region Constructors
        public PlayAndWaitMoveTowardsQuaternion(Func<Quaternion> _getPredicate, Action<Quaternion> _setPredicate, Quaternion _target, float _step) 
            : base(_getPredicate, _setPredicate, _target, _step, (a, b, t) => Quaternion.RotateTowards(a, b, t))
        { }
        #endregion
    }
}