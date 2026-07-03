using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    sealed public class TriangleCell<TValue, TEdge> : CellBase<TValue, TEdge>
    {
        public override int EdgeCount => 3;
        public override int NeighbourCount => 12;
    }
}
