using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public struct RectGridTopology : IGridTopology
    {
        readonly static Vector2Int[] edgeCoordinates = new Vector2Int[4] 
        {
            Vector2Int.up, Vector2Int.right, -Vector2Int.up, -Vector2Int.right
        };

        public readonly int EdgeCount => 4;

        public readonly Vector2Int GetNeighbourCoordinateOffset(Vector2Int _coord, int _edgeIndex) => edgeCoordinates[_edgeIndex];
    }
}
