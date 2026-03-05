using System;
using UnityEngine;


namespace MaroonSeal
{
    public class PlayAndWaitForLerpColour : PlayAndWaitForLerp
    {
        Color startColour;
        Color targetColour;

        Action<Color> colourSetPredicate;

        #region Constructors
        public PlayAndWaitForLerpColour(Action<Color> _colourSetPredicate, float _duration, Color _startColour, Color _targetColour) : base(null, _duration)
        {
            colourSetPredicate = _colourSetPredicate;

            startColour = _startColour;
            targetColour = _targetColour;

            lerpFunction = (cntx) => colourSetPredicate.Invoke(Color.Lerp(startColour, targetColour, cntx));
        }
        #endregion

    }
}