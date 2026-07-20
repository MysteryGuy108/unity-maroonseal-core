using UnityEngine;

using MaroonSeal.Maths.Geometry;

namespace MaroonSeal.DataStructures.Grid
{
    public class HexagonGrid<TValue, TEdge> : Grid<TValue, TEdge, HexagonGridTopology, Hexagon2DGridGeometry>
    {
        #region Constructor
        public HexagonGrid(Vector2Int _size, float _cellRadius) : base(_size, new(), new Hexagon2DGridGeometry(_cellRadius)) {}
        #endregion
        
    }
}
