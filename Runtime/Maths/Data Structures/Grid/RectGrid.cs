using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class RectGrid<TValue, TEdge> : Grid<TValue, TEdge>
    {
        readonly static Vector2Int[] edgeCoordinates = new Vector2Int[4] 
        {
            Vector2Int.up, Vector2Int.right, -Vector2Int.up, -Vector2Int.right
        };

        public RectGrid(Vector2Int _size) : base(4, _size) { }

        protected override Vector2Int GetEdgeCoordinate(Vector2Int _coord, int _edgeIndex) => edgeCoordinates[_edgeIndex];
    }
}
