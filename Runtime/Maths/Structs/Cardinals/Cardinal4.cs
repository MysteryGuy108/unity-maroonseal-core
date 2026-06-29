using UnityEngine;

namespace MaroonSeal.Maths
{
    [System.Serializable]
    public struct Cardinal4 : ICardinal
    {
        readonly public int DirectionCount => 4;

        public enum Direction { North, East, South, West }
        public Direction direction;
        
        readonly public int Index => (int)direction;
        readonly public float Theta => Index * 90.0f;

        #region Constructors
        public Cardinal4(Direction _dir) { direction = _dir; }

        public Cardinal4(int _index) { direction = Direction.North; direction = (Direction)(_index % DirectionCount); }
        public Cardinal4(float _theta) { direction = Direction.North; direction = (Direction)ICardinal.FromThetaToIndex(_theta, DirectionCount); }
        public Cardinal4(Vector2 _vector) { direction = Direction.North; direction = (Direction)ICardinal.FromVectorToIndex(_vector, DirectionCount); }
        #endregion

        #region Casting
        public static explicit operator Cardinal4(Cardinal8 _cardinal8) => new((int)_cardinal8.direction / 2);
        #endregion

        #region Static Constructors
        public static Cardinal4 North { get { return new Cardinal4 { direction = Direction.North }; } }
        public static Cardinal4 East { get { return new Cardinal4 { direction = Direction.East }; } }
        public static Cardinal4 South { get { return new Cardinal4 { direction = Direction.South }; } }
        public static Cardinal4 West { get { return new Cardinal4 { direction = Direction.West }; } }
        #endregion

        #region Operators
        public static bool operator ==(Cardinal4 x, Cardinal4 y) => x.direction == y.direction;
        
        public static bool operator !=(Cardinal4 x, Cardinal4 y) => x.direction != y.direction;
        readonly public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj is not Cardinal4) { return false; }
            return (Cardinal4)obj == this;
        }
        readonly public override int GetHashCode() {
            unchecked { return direction.GetHashCode(); }
        }
        #endregion
        
        #region Rotation
        readonly private int GetRotatedIndex(int _rotateAmount) => (int)Mathf.Repeat(Index + _rotateAmount, DirectionCount);

        public void Rotate(int _rotateAmount) => direction = (Direction)GetRotatedIndex(_rotateAmount);
        public void Rotate(Direction _rotateAmount) => Rotate((int)_rotateAmount);
        public void Rotate(Cardinal4 _rotateAmount) => Rotate(_rotateAmount.direction); 


        readonly public Cardinal4 GetRotated(int _index) => new((Direction)GetRotatedIndex(_index));
        readonly public Cardinal4 GetRotated(Direction _direction) => GetRotated((int)_direction);
        readonly public Cardinal4 GetRotated(Cardinal4 _cardinal) => GetRotated(_cardinal.direction);

        readonly public Cardinal4 Opposite => new((Direction)GetRotatedIndex(DirectionCount/2));
        readonly public Cardinal4 Clockwise  => new((Direction)GetRotatedIndex(1));
        readonly public Cardinal4 Anticlockwise => new((Direction)GetRotatedIndex(-1));
        #endregion
    }
}
