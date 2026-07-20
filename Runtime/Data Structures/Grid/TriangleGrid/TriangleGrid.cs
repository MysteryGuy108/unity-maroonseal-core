using UnityEngine;

using MaroonSeal.Maths.Geometry;

namespace MaroonSeal.DataStructures.Grid
{
    public class TriangleGrid<TValue, TEdge> : Grid<TValue, TEdge, TriangleGridTopology, Triangle2DGridGeometry>
    {
        #region Constructor
        public TriangleGrid(Vector2Int _size, float _cellSize) : base(_size, new TriangleGridTopology(), new Triangle2DGridGeometry(_cellSize)) {}
        #endregion
    }
}
