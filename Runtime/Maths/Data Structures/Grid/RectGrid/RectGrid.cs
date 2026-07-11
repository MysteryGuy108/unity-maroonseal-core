using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    [System.Serializable]
    public class RectGrid<TValue, TEdge> : Grid<TValue, TEdge, RectGridTopology, RectGridGeometry>
    {
        #region Construtor
        public RectGrid(Vector2Int _size, Vector2 _cellSize) : base(_size, new RectGridTopology(), new RectGridGeometry(_cellSize)) { }
        #endregion
    }
}
