using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths {
    [System.Serializable]
    public struct Cardinal2D
    {
        public enum CardinalIndex { North, East, South, West }
        public CardinalIndex cardinalDirection;

        readonly public CardinalIndex Direction { get { return cardinalDirection; }}
        readonly public int Index {  get { return (int)cardinalDirection; } }

        #region Constructors
        public Cardinal2D(CardinalIndex _dir) { cardinalDirection = _dir; }

        public Cardinal2D(int _dir) { cardinalDirection = (CardinalIndex)(_dir % 4); }

        public Cardinal2D(Vector2Int _dir) { cardinalDirection = GetIndexFromVector(_dir); }

        public Cardinal2D(Vector2 _dir) { cardinalDirection = GetIndexFromVector(_dir); }
        #endregion

        #region Static Constructors
        public static Cardinal2D North { get { return new Cardinal2D { cardinalDirection = CardinalIndex.North }; } }
        public static Cardinal2D East { get { return new Cardinal2D { cardinalDirection = CardinalIndex.East }; } }
        public static Cardinal2D South { get { return new Cardinal2D { cardinalDirection = CardinalIndex.South }; } }
        public static Cardinal2D West { get { return new Cardinal2D { cardinalDirection = CardinalIndex.West }; } }

        public static Cardinal2D Forward { get { return new Cardinal2D { cardinalDirection = CardinalIndex.North }; } }
        public static Cardinal2D Right { get { return new Cardinal2D { cardinalDirection = CardinalIndex.East }; } }
        public static Cardinal2D Back { get { return new Cardinal2D { cardinalDirection = CardinalIndex.South }; } }
        public static Cardinal2D Left { get { return new Cardinal2D { cardinalDirection = CardinalIndex.West }; } }
        
        public static Cardinal2D[] Directions { get { return new Cardinal2D[4] { North, East, South, West }; } }
        #endregion

        #region Conversions
        readonly public Vector2Int Vec2Int {
            get {
                return cardinalDirection switch {
                    CardinalIndex.North => Vector2Int.up,
                    CardinalIndex.East => Vector2Int.right,
                    CardinalIndex.South => -Vector2Int.up,
                    CardinalIndex.West => -Vector2Int.right,
                    _ => Vector2Int.zero,
                };
            }
        }

        readonly public Vector2 Vec2 {
            get {
                return cardinalDirection switch {
                    CardinalIndex.North => Vector2.up,
                    CardinalIndex.East => Vector2.right,
                    CardinalIndex.South => -Vector2.up,
                    CardinalIndex.West => -Vector2.right,
                    _ => Vector2.zero,
                };
            }
        }

        readonly public Vector3 Vec3 {
            get {
                return cardinalDirection switch {
                    CardinalIndex.North => new Vector3(0, 0, 1),
                    CardinalIndex.East => new Vector3(1, 0, 0),
                    CardinalIndex.South => new Vector3(0, 0, -1),
                    CardinalIndex.West => new Vector3(-1, 0, 0),
                    _ => (Vector3)Vector3Int.zero,
                };
            }
        }
        readonly public float Theta {
            get {
                return cardinalDirection switch
                {
                    CardinalIndex.North => 0.0f,
                    CardinalIndex.East => 270.0f,
                    CardinalIndex.South => 180.0f,
                    CardinalIndex.West => 90.0f,
                    _ => 0.0f,
                };
            }
        }
        readonly public Quaternion Rotation {
            get {
                return Quaternion.Euler(0.0f, -Theta, 0.0f); 
            }
        }
        
        static private CardinalIndex GetIndexFromVector(Vector2Int _vec) {
            
            if (_vec.y > 0) { return CardinalIndex.North;}
            if (_vec.y < 0) { return CardinalIndex.South; }

            if (_vec.x > 0) { return CardinalIndex.East; }
            if (_vec.x < 0) { return CardinalIndex.West; }

            return CardinalIndex.North;
        }

        static private CardinalIndex GetIndexFromVector(Vector2 _vec) {
            _vec.Normalize();

            float closestDot = -1.0f;
            CardinalIndex closestDirectionIndex = CardinalIndex.North;

            for(int i = 0; i < 4; i++) {
                Cardinal2D direction = new Cardinal2D(i);

                float dot = Vector2.Dot(_vec, direction.Vec2);
                if (dot >= closestDot) { 
                    closestDot = dot;
                    closestDirectionIndex = direction.Direction; 
                }
            }

            return closestDirectionIndex;
        }
        #endregion

        readonly public Cardinal2D Opposite { get { return new Cardinal2D((CardinalIndex)(((int)cardinalDirection + 2) % 4)); } }
        readonly public Cardinal2D Clockwise { get { return new Cardinal2D((CardinalIndex)(((int)cardinalDirection + 1) % 4)); } }
        readonly public Cardinal2D Anticlockwise { get { return new Cardinal2D((CardinalIndex)Mathf.Repeat((int)cardinalDirection - 1, 4)); } }

        public void Rotate(int _rotateAmount) { cardinalDirection = (CardinalIndex)Mathf.Repeat(Index + _rotateAmount, 4); }
        public void Rotate(CardinalIndex _rotateAmount) { Rotate((int)_rotateAmount); }
        public void Rotate(Cardinal2D _rotateAmount) { Rotate(_rotateAmount.Direction); }
        

        public static Cardinal2D operator +(Cardinal2D x, Cardinal2D y) {
            return new Cardinal2D { cardinalDirection = (CardinalIndex)(((int)x.cardinalDirection + (int)y.cardinalDirection) % 4) };
        }

        public static Cardinal2D operator +(Cardinal2D x, int y) {
            return new Cardinal2D { cardinalDirection = (CardinalIndex)(((int)x.cardinalDirection + y )% 4) };
        }

        public static Cardinal2D operator -(Cardinal2D x, Cardinal2D y) {
            return new Cardinal2D { cardinalDirection = (CardinalIndex)Mathf.Repeat((int)x.cardinalDirection + (int)y.cardinalDirection, 4) };
        }

        public static bool operator ==(Cardinal2D x, Cardinal2D y) {
            return x.cardinalDirection == y.cardinalDirection;
        }
        
        public static bool operator !=(Cardinal2D x, Cardinal2D y) {
            return x.cardinalDirection != y.cardinalDirection;
        }
        readonly public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj is not Cardinal2D) { return false; }
            return (Cardinal2D)obj == this;
        }
        readonly public override int GetHashCode() {
            unchecked { return cardinalDirection.GetHashCode(); }
        }
    }
}
