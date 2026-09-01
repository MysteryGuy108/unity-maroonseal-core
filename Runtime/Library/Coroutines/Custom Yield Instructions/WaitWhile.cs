using System;
using UnityEngine;

namespace MaroonSeal {
    public class WaitWhile : CustomYieldInstruction
    {
        Func<bool> predicate;

        #region Constructor
        public WaitWhile(Func<bool> _predicate) => predicate = _predicate;
        #endregion

        public override bool keepWaiting => predicate.Invoke();
    }
}