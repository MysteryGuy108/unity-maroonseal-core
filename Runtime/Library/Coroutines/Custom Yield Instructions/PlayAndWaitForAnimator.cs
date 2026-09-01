using UnityEngine;

namespace MaroonSeal
{
    public class PlayAndWaitForAnimator : CustomYieldInstruction
    {
        Animator animator;
        int stateHash;
        int layer;

        bool hasStarted;

        #region Constructors
        public PlayAndWaitForAnimator(Animator _animator, int _stateHash, int _layer = 0)
        {
            animator = _animator;
            stateHash = _stateHash;
            layer = _layer;
            hasStarted = false;

            animator.Play(stateHash);
        }

        public PlayAndWaitForAnimator(Animator _target, string _stateName, int _layer = 0) :
            this(_target, Animator.StringToHash(_stateName), _layer)
        { }

        #endregion

        #region Custom Yield Instruction
        public override bool keepWaiting
        {
            get
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);

                if (!hasStarted)
                {
                    hasStarted = stateInfo.shortNameHash == stateHash;
                    return true;
                }

                if (stateInfo.speedMultiplier >= 0)
                {
                    return stateInfo.shortNameHash == stateHash && stateInfo.normalizedTime < 1.0f;
                }
                else
                {
                    return stateInfo.shortNameHash == stateHash && stateInfo.normalizedTime > 0.0f;
                }
                
            }
        }
        #endregion
    }
}