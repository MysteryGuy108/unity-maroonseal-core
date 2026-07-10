using System;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class HexCellGeometry : IGridGeometry
    {
        [field : SerializeField] float CellRadius { get; set; }

        #region Constructors
        public HexCellGeometry(float _cellRadius) { CellRadius = _cellRadius; }
        #endregion

        #region IGridGeometry
        public Vector2 CellToWorld(Vector2Int _cell) => Vector2.zero;

        public Vector2Int WorldToCell(Vector2Int _gridSize, Vector2 _worldPoint) { 
            return new(
                (int)(_worldPoint.x / _gridSize.x), 
                (int)(_worldPoint.y / _gridSize.y)
            );
        }
        #endregion
    }
}
