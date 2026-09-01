using UnityEngine;
using UnityEngine.UI;

namespace MaroonSeal
{
    public class PlayAndWaitForGraphicColourShift : PlayAndWaitForLerpColour
    {
        #region Constructors
        public PlayAndWaitForGraphicColourShift(Graphic _graphic, float _duration, Color _targetColour, Color? _startColour = null) 
            : base((cntx) => _graphic.color = cntx, _duration, _startColour == null ? _graphic.color : _startColour.Value, _targetColour)
        {
            
        }
        #endregion
    }
}