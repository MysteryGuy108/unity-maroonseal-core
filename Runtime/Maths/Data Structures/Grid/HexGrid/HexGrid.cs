using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class HexGrid<TValue, TEdge> : Grid<TValue, TEdge, HexGridTopology, HexGridGeometry>
    {
        #region Constructor
        public HexGrid(Vector2Int _size, float _cellRadius) : base(_size, new(), new HexGridGeometry(_cellRadius)) {}
        #endregion
        
    }
}
