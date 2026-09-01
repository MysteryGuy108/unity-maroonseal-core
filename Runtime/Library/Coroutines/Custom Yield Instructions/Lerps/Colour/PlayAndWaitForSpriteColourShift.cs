using UnityEngine;

namespace MaroonSeal
{
    public class PlayAndWaitForSpriteColourShift : PlayAndWaitForLerpColour
    {
        public PlayAndWaitForSpriteColourShift(SpriteRenderer _sprite, float _duration, Color _targetColour, Color? _startColour = null) 
            : base((cntx) => _sprite.color = cntx, _duration, _startColour == null ? _sprite.color : _startColour.Value, _targetColour)
        {

        }
    }
}