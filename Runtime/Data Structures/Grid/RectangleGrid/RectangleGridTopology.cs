using System;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.DataStructures.Grid
{
    public class RectangleGridTopology : GridTopology
    {
        readonly static Vector2Int[] edgeCoordinates = new Vector2Int[4] 
        {
            Vector2Int.up, Vector2Int.right, -Vector2Int.up, -Vector2Int.right
        };

        public override int EdgeCount => 4;

        public override Vector2Int GetNeighbour(Vector2Int _cell, int _edgeIndex) => edgeCoordinates[_edgeIndex];

        public override Vector2Int RotateClockwise(Vector2Int _cell) => new(-_cell.y, _cell.x);
        public override Vector2Int RotateAntiClockwise(Vector2Int _cell) => new(_cell.y, -_cell.x);
    }
}
