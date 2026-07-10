using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class RectCellTopology : IGridTopology
    {
        readonly static Vector2Int[] edgeCoordinates = new Vector2Int[4] 
        {
            Vector2Int.up, Vector2Int.right, -Vector2Int.up, -Vector2Int.right
        };

        public int EdgeCount => 4;

        public Vector2Int GetNeighbourCoordinateOffset(Vector2Int _coord, int _edgeIndex) => edgeCoordinates[_edgeIndex];
    }
}
