using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class TriGrid<TValue, TEdge> : Grid<TValue, TEdge, TriGridTopology, TriGridGeometry>
    {
        #region Constructor
        public TriGrid(Vector2Int _size, float _cellSize) : base(_size, new TriGridTopology(), new TriGridGeometry(_cellSize)) {}
        #endregion

    }
}
