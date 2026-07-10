using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class RectGrid<TValue, TEdge> : Grid<TValue, TEdge>
    {
        #region Construtor
        public RectGrid(Vector2Int _size, Vector2 _cellSize) : base(_size, new RectCellTopology(), new RectCellGeometry(_cellSize)) { }
        #endregion
    }
}
