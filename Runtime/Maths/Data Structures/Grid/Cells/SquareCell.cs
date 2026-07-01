using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    [System.Serializable]
    sealed public class SquareCell<TValue, TEdge> : CellBase<TValue, TEdge>
    {
        public override int EdgeCount => 4;

    }
}
