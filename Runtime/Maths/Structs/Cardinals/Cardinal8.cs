using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths {
    /// <summary>
    /// A struct used to represent a cardinal direction (North, South, East, West).
    /// </summary>
    [System.Serializable]
    public struct Cardinal8 : ICardinal
    {
        public static readonly int Count = 8;

        public enum Direction { N, NE, E, SE, S, SW, W, NW }
        public Direction direction;
        readonly public int Index {  get { return (int)direction; } }
        readonly public float Theta => Index * 45.0f;
        
        #region Constructors
        public Cardinal8(Direction _dir) { direction = _dir; }

        public Cardinal8(int _index) { direction = (Direction)(_index % Count); }
        public Cardinal8(float _theta) { direction = (Direction)ICardinal.FromThetaToIndex(_theta, Count); }
        public Cardinal8(Vector2 _vector) { direction = (Direction)ICardinal.FromVectorToIndex(_vector, Count); }
        #endregion

        #region Casting
        public static explicit operator Cardinal8(Cardinal4 _cardinal8) => new((int)_cardinal8.direction * 2);
        #endregion

        #region Static Constructors
        public static Cardinal8 N => new(){ direction = Direction.N };
        public static Cardinal8 NE => new(){ direction = Direction.NE };
        public static Cardinal8 E => new(){ direction = Direction.E };
        public static Cardinal8 SE => new() { direction = Direction.SE }; 
        public static Cardinal8 S => new() { direction = Direction.S }; 
        public static Cardinal8 SW => new(){ direction = Direction.SW }; 
        public static Cardinal8 W => new() { direction = Direction.W };
        public static Cardinal8 NW => new() { direction = Direction.NW }; 
        
        public static Cardinal8[] Directions { get { return new Cardinal8[8] { N, NE, E, SE, S, SW, W, NW }; } }
        #endregion

        #region Operators
        public static bool operator ==(Cardinal8 x, Cardinal8 y) =>  x.direction == y.direction;
        
        public static bool operator !=(Cardinal8 x, Cardinal8 y) => x.direction != y.direction;

        readonly public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj is not Cardinal8) { return false; }
            return (Cardinal8)obj == this;
        }
        readonly public override int GetHashCode() {
            unchecked { return direction.GetHashCode(); }
        }
        #endregion

        #region Rotation
        readonly private int GetRotatedIndex(int _rotateAmount) => (int)Mathf.Repeat(Index + _rotateAmount, Count);

        public void Rotate(int _rotateAmount) { direction = (Direction)Mathf.Repeat(Index + _rotateAmount, Count); }
        public void Rotate(Direction _rotateAmount) { Rotate((int)_rotateAmount); }
        public void Rotate(Cardinal8 _rotateAmount) { Rotate(_rotateAmount.direction); }


        readonly public Cardinal8 GetRotated(int _index) => new((Direction)GetRotatedIndex(_index));
        readonly public Cardinal8 GetRotated(Direction _direction) => GetRotated((int)_direction);
        readonly public Cardinal8 GetRotated(Cardinal8 _cardinal) => GetRotated(_cardinal.direction);

        readonly public Cardinal8 Opposite => new((Direction)GetRotatedIndex(4));
        readonly public Cardinal8 Clockwise  => new((Direction)GetRotatedIndex(1));
        readonly public Cardinal8 Anticlockwise => new((Direction)GetRotatedIndex(-1));
        #endregion
    }
}
