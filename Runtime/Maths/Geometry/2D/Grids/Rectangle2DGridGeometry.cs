using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public struct Rectangle2DGridGeometry : IGridGeometry2D
    {
        public Vector2 CellSize { get; set; }

        #region Constructors
        public Rectangle2DGridGeometry(Vector2 _cellSize) {
            CellSize = _cellSize.Abs(); 
        }
        #endregion

        #region IGridGeometry
        readonly public Vector2Int WorldToCell(Vector2 _worldPoint) { 
            return new(
                CellSize.x == 0.0f ? 0 : Mathf.FloorToInt(_worldPoint.x / CellSize.x), 
                CellSize.y == 0.0f ? 0 :Mathf.FloorToInt(_worldPoint.y / CellSize.y)
            );
        }

        readonly public Vector2 GetCellCentre(Vector2Int _cell) => (_cell * CellSize) + (CellSize * 0.5f);

        readonly public IShape2D GetCellShape(Vector2Int _cell)
        {
            return new Rectangle2D(new Transform2D(GetCellCentre(_cell)), CellSize);
        }
        #endregion
    }
}
