using System;
using UnityEngine;

namespace MaroonSeal.Routines
{
    public class PlayAndWaitMoveTowardsVector3 : PlayAndWaitMoveTowardsBase<Vector3>
    {
        #region Constructors
        public PlayAndWaitMoveTowardsVector3(Func<Vector3> _getPredicate, Action<Vector3> _setPredicate, Vector3 _target, float _step) 
            : base(_getPredicate, _setPredicate, _target, _step, (a, b, t) => Vector3.MoveTowards(a, b, t))
        {

        }
        #endregion
    }
}