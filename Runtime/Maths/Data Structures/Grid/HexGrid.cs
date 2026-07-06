using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class HexGrid<TValue, TEdge> : Grid<TValue, TEdge>
    {
        readonly static Vector2Int[] evenRowEdges = new Vector2Int[6] 
        {
            new(-1, 0), new(1, 0), new(-1, -1), new(0, -1), new(-1, 1), new(0, 1)
        };

        readonly static Vector2Int[] oddRowEdges = new Vector2Int[6]
        {
            new(-1, 0), new(1, 0), new(0,-1), new(1,-1), new(0, 1), new(1, 1)
        };

        #region Constructor
        public HexGrid(Vector2Int _size) : base(6, _size) {}
        #endregion
        
        protected override Vector2Int GetEdgeCoordinate(Vector2Int _coord, int _index)
        {
            Vector2Int[] directions = _coord.y % 2 == 0 ? evenRowEdges : oddRowEdges;
            return directions[_index];
        }
    }
}
