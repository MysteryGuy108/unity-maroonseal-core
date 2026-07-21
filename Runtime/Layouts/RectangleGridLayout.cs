using UnityEngine;

using MaroonSeal.DataStructures.Grid;

using MaroonSeal.Maths;
using MaroonSeal.Maths.Geometry;

namespace MaroonSeal.Layouts
{
    public class RectangleGridLayout : MonoBehaviour
    {
        private RectangleGrid<float, bool> grid;
        
        public Vector2Int gridSize;
        public Vector2 cellSize;
        [Space]
        public Vector2 testCursor;
        [InspectorReadOnly] public Vector2Int cursorCell;

        private void OnValidate()
        {
            gridSize = Vector2Maths.FloorToInt(Vector2Maths.Max(Vector2.zero, gridSize));

            grid ??= new(gridSize, cellSize);
            if (grid.Size != gridSize) { grid = new(gridSize, cellSize); }

            grid.Geometry.CellSize = cellSize;
            cursorCell = grid.Geometry.WorldToCell(testCursor);
        }

        private void OnDrawGizmosSelected()
        {
            if (grid == null) { return; }
            Gizmos.color = Color.white;
            for(int y = 0; y < grid.Size.y; y++)
            {
                for(int x = 0; x < grid.Size.x; x++)
                {
                    Vector2Int cell = new(x, y);
                    if (cell == cursorCell) { continue; }
                    
                    GeometryGizmos2D.DrawPolygon((IPolygon2D)grid.Geometry.GetCellShape(cell));
                }
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(testCursor, 0.03125f);
            GeometryGizmos2D.DrawPolygon((IPolygon2D)grid.Geometry.GetCellShape(cursorCell));
        }
    }
}
