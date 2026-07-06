using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class TriGrid<TValue, TEdge> : Grid<TValue, TEdge>
    {
        readonly static Vector2Int[] upTriEdges = new Vector2Int[3] {
            Vector2Int.up, Vector2Int.right, -Vector2Int.right
        };

        readonly static Vector2Int[] downTriEdges = new Vector2Int[3] {
            Vector2Int.right, -Vector2Int.up, -Vector2Int.right
        };

        
        #region Constructor
        public TriGrid(Vector2Int _size) : base(3, _size) {}
        #endregion

        public bool GetIsUpTri(Vector2Int _coord) => ((_coord.x + _coord.y) & 1) == 0;

        protected override Vector2Int GetEdgeCoordinate(Vector2Int _coord, int _index) => 
            GetIsUpTri(_coord) ? upTriEdges[_index] : downTriEdges[_index];
    }
}
