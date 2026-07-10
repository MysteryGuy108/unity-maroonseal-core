using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public interface IGridGeometry
    {
        public Vector2Int WorldToCell(Vector2Int _gridSize, Vector2 _worldPoint);
        public Vector2 CellToWorld(Vector2Int _cell);
    }
}
