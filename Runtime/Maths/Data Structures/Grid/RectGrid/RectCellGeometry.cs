using MaroonSeal.Maths.Geometry.Shapes;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class RectCellGeometry : IGridGeometry
    {
        [field : SerializeField] Vector2 CellSize { get; set; }

        #region Constructors
        public RectCellGeometry(Vector2 _cellSize) { CellSize = _cellSize; }
        #endregion

        #region IGridGeometry
        public Vector2 CellToWorld(Vector2Int _cell) => _cell * CellSize;

        public Vector2Int WorldToCell(Vector2Int _gridSize, Vector2 _worldPoint) { 
            return new(
                (int)(_worldPoint.x / _gridSize.x), 
                (int)(_worldPoint.y / _gridSize.y)
            );
        }
        #endregion
    }
}
