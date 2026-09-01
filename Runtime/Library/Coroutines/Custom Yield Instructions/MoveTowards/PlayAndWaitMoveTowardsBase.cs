using System;
using UnityEngine;

namespace MaroonSeal.Routines
{
    abstract public class PlayAndWaitMoveTowardsBase<TValue> : CustomYieldInstruction
    {
        private Func<TValue> getPredicate;
        private Action<TValue> setPredicate;

        private TValue target;

        protected Func<TValue, TValue, float, TValue> moveTowardsPredicate;
        private float step;

        #region Constructors
        public PlayAndWaitMoveTowardsBase(Func<TValue> _getPredicate, Action<TValue> _setPredicate, TValue _target, float _step, Func<TValue, TValue, float, TValue> _moveTowardsPredicate)
        {
            getPredicate = _getPredicate;
            setPredicate = _setPredicate;
            moveTowardsPredicate = _moveTowardsPredicate;

            target = _target;

            step = _step;
        }
        #endregion

        public override bool keepWaiting
        {
            get
            {
                TValue current = getPredicate.Invoke();
                current = moveTowardsPredicate.Invoke(current, target, step * Time.deltaTime);
                setPredicate.Invoke(current);
                return !current.Equals(target);
            }
        }
    }
}