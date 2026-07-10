using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public interface IGridTopology
    {
        public int EdgeCount { get; }

        public Vector2Int GetNeighbourCoordinateOffset(Vector2Int _coord, int _edgeIndex);
    }
}
