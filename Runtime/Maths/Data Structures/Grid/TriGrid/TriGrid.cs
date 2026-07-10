using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class TriGrid<TValue, TEdge> : Grid<TValue, TEdge>
    {

        
        #region Constructor
        public TriGrid(Vector2Int _size, float _cellRadius) : base(_size, new TriCellTopology(), new TriCellGeometry(_cellRadius)) {}
        #endregion

    }
}
