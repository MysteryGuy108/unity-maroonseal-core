using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    public interface IGridGeometry2D : IGeometry
    {
        public Vector2Int WorldToCell(Vector2 _worldPoint);
        public Vector2 GetCellCentre(Vector2Int _cell);
        public IShape2D GetCellShape(Vector2Int _cell);
    }
}
