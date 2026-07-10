using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class TriCellTopology : IGridTopology
    {
        public int EdgeCount => 3;

        readonly static Vector2Int[] upTriEdges = new Vector2Int[3] {
            Vector2Int.up, Vector2Int.right, -Vector2Int.right
        };

        readonly static Vector2Int[] downTriEdges = new Vector2Int[3] {
            Vector2Int.right, -Vector2Int.up, -Vector2Int.right
        };

        public bool GetIsUpTri(int _x, int _y) => ((_x + _y) & 1) == 0;
        public bool GetIsUpTri(Vector2Int _coord) => GetIsUpTri(_coord.x, _coord.y);

        public Vector2Int GetNeighbourCoordinateOffset(Vector2Int _coord, int _edgeIndex) => 
            GetIsUpTri(_coord) ? upTriEdges[_edgeIndex] : downTriEdges[_edgeIndex];
    }
}
