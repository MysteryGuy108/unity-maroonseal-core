using UnityEngine;

namespace MaroonSeal.Maths
{
    [System.Serializable]
    public struct Cardinal16 : ICardinal
    {
        readonly public int DirectionCount => 16;

        public enum Direction { N, NNE, NE, ENE, E, ESE, SE, SSE, S, SSW, SW, WSW, W, WNW, NW, NNW }
        public Direction direction;
        
        readonly public int Index => (int)direction;
        readonly public float Theta => Index * 22.5f;

        #region Constructors
        public Cardinal16(Direction _dir) { direction = _dir; }

        public Cardinal16(int _index) { direction = Direction.N; direction = (Direction)(_index % DirectionCount); }
        public Cardinal16(float _theta) { direction = Direction.N; direction = (Direction)ICardinal.FromThetaToIndex(_theta, DirectionCount); }
        public Cardinal16(Vector2 _vector) { direction = Direction.N; direction = (Direction)ICardinal.FromVectorToIndex(_vector, DirectionCount); }
        #endregion

        #region Casting
        public static explicit operator Cardinal16(Cardinal8 _cardinal8) => new((int)_cardinal8.direction / 2);
        #endregion

        #region Static Constructors N, NNE, NE, ENE, E, ESE, SE, SSE, S, SSW, SW, WSW, W, WNW, NW, NNW
        public static Cardinal16 N => new(){ direction = Direction.N };
        public static Cardinal16 NNE => new(){ direction = Direction.NNE };
        public static Cardinal16 NE => new(){ direction = Direction.NE };
        public static Cardinal16 ENE => new(){ direction = Direction.ENE };

        public static Cardinal16 E => new(){ direction = Direction.E };
        public static Cardinal16 ESE => new() { direction = Direction.ESE };
        public static Cardinal16 SE => new() { direction = Direction.SE }; 
        public static Cardinal16 SSE => new() { direction = Direction.SSE };

        public static Cardinal16 S => new() { direction = Direction.S }; 
        public static Cardinal16 SSW => new() { direction = Direction.SSW }; 
        public static Cardinal16 SW => new(){ direction = Direction.SW };
        public static Cardinal16 WSW => new(){ direction = Direction.WSW };

        public static Cardinal16 W => new() { direction = Direction.W };
        public static Cardinal16 WNW => new() { direction = Direction.WNW }; 
        public static Cardinal16 NW => new() { direction = Direction.NW }; 
        public static Cardinal16 NNW => new() { direction = Direction.NNW }; 
        #endregion

        #region Operators
        public static bool operator ==(Cardinal16 x, Cardinal16 y) => x.direction == y.direction;
        
        public static bool operator !=(Cardinal16 x, Cardinal16 y) => x.direction != y.direction;
        readonly public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj is not Cardinal16) { return false; }
            return (Cardinal16)obj == this;
        }
        readonly public override int GetHashCode() {
            unchecked { return direction.GetHashCode(); }
        }
        #endregion
        
        #region Rotation
        readonly private int GetRotatedIndex(int _rotateAmount) => (int)Mathf.Repeat(Index + _rotateAmount, DirectionCount);

        public void Rotate(int _rotateAmount) => direction = (Direction)GetRotatedIndex(_rotateAmount);
        public void Rotate(Direction _rotateAmount) => Rotate((int)_rotateAmount);
        public void Rotate(Cardinal16 _rotateAmount) => Rotate(_rotateAmount.direction); 

        readonly public Cardinal16 GetRotated(int _index) => new((Direction)GetRotatedIndex(_index));
        readonly public Cardinal16 GetRotated(Direction _direction) => GetRotated((int)_direction);
        readonly public Cardinal16 GetRotated(Cardinal16 _cardinal) => GetRotated(_cardinal.direction);

        readonly public Cardinal16 Opposite => new((Direction)GetRotatedIndex(DirectionCount/2));
        readonly public Cardinal16 Clockwise  => new((Direction)GetRotatedIndex(1));
        readonly public Cardinal16 Anticlockwise => new((Direction)GetRotatedIndex(-1));
        #endregion
    }
}