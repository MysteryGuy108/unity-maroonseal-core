using UnityEngine;

using MaroonSeal.Maths.Geometry;

namespace MaroonSeal.DataStructures.Grid
{
    [System.Serializable]
    public class RectangleGrid<TValue, TEdge> : Grid<TValue, TEdge, RectangleGridTopology, Rectangle2DGridGeometry>
    {
        #region Construtor
        public RectangleGrid(Vector2Int _size, Vector2 _cellSize) : base(_size, new RectangleGridTopology(), new Rectangle2DGridGeometry(_cellSize)) { }
        #endregion
    }
}
