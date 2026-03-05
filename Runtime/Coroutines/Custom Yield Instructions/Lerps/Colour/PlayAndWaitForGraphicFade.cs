using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MaroonSeal
{
    public class PlayAndWaitForGraphicFade : PlayAndWaitForGraphicColourShift
    {
        #region Constructors
        public PlayAndWaitForGraphicFade(Graphic _graphic, float _duration, float _targetAlpha, float? _startAlpha = null) 
            : base( _graphic, _duration, _graphic.color.FromAlpha(_targetAlpha), _startAlpha == null ? _graphic.color : _graphic.color.FromAlpha(_startAlpha.Value))
        {

        }
        #endregion
    }
}