using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    [System.Serializable]
    sealed public class RectangleCell<TValue, TEdge> : CellBase<TValue, TEdge>
    {
        public override int EdgeCount => 4;
        public override int NeighbourCount => 8;
    }
}
