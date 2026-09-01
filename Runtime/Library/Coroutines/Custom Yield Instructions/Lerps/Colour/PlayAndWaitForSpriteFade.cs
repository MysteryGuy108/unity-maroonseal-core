using UnityEngine;

namespace MaroonSeal.Routines
{
    public class PlayAndWaitForSpriteRendererFade : PlayAndWaitForSpriteColourShift
    {
        public PlayAndWaitForSpriteRendererFade(SpriteRenderer _sprite, float _duration, float _targetAlpha, float? _startAlpha = null) 
            : base(
                _sprite, _duration, _sprite.color.FromAlpha(_targetAlpha), _startAlpha == null ? _sprite.color : _sprite.color.FromAlpha(_startAlpha.Value))
        {
        }
    }
}