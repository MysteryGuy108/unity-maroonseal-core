using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.DataStructures.Grid
{
    public abstract class GridTopology
    {
        abstract public int EdgeCount { get; }

        #region Neighbours
        public abstract Vector2Int GetNeighbour(Vector2Int _cell, int _edgeIndex);

        public IEnumerable<Vector2Int> GetNeighbours(Vector2Int _cell)
        {
            for(int i = 0; i < this.EdgeCount; i++) {
                yield return this.GetNeighbour(_cell, i);
            }
        }
        #endregion
        
        #region Rotation
        public abstract Vector2Int RotateClockwise(Vector2Int _cell);
        public abstract Vector2Int RotateAntiClockwise(Vector2Int _cell);

        public Vector2Int Rotate(Vector2Int _cell, int _amount) {
            for(int i = 0; i < Mathf.Abs(_amount); i++) {
                _cell = _amount > 0 ? RotateClockwise(_cell) : RotateAntiClockwise(_cell);
            }
            return _cell;
        }

        public Vector2Int Rotate(Vector2Int _cell, Vector2Int _pivot, int _amount) {
            Vector2Int relative = _cell - _pivot;
            relative = Rotate(relative, _amount);
            return relative + _pivot;
        }
        #endregion
    }
}
