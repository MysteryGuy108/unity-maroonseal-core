using UnityEngine;

using MaroonSeal.Maths.DataStructures.Grid;
using MaroonSeal.Maths.Geometry;

namespace MaroonSeal
{
    public class RectangleGridLayout : MonoBehaviour
    {
        private RectGrid<float, bool> grid;
        
        public Vector2Int gridSize;
        public Vector2 cellSize;
        [Space]
        public Vector2 testCursor;
        [EditorReadOnly] public Vector2Int cursorCell;

        private void OnValidate()
        {
            gridSize.x = Mathf.Max(0, gridSize.x); 
            gridSize.y = Mathf.Max(0, gridSize.y); 

            if (grid == null) { 
                grid = new(gridSize, cellSize);
            }
            else if (grid.Size != gridSize) { 
                grid = new(gridSize, cellSize); 
            }

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
