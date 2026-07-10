using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class HexCellTopology : IGridTopology
    {
        readonly static Vector2Int[] evenRowEdges = new Vector2Int[6] 
        {
            new(-1, 0), new(1, 0), new(-1, -1), new(0, -1), new(-1, 1), new(0, 1)
        };

        readonly static Vector2Int[] oddRowEdges = new Vector2Int[6]
        {
            new(-1, 0), new(1, 0), new(0,-1), new(1,-1), new(0, 1), new(1, 1)
        };

        public int EdgeCount => 6;

        public Vector2Int GetNeighbourCoordinateOffset(Vector2Int _coord, int _edgeIndex)
        {
            Vector2Int[] directions = _coord.y % 2 == 0 ? evenRowEdges : oddRowEdges;
            return directions[_edgeIndex];
        }
    }
}
