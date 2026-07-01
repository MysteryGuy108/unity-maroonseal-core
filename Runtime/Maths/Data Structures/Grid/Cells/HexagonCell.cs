using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    sealed public class HexagonCell<TValue, TEdge> : CellBase<TValue, TEdge>
    {
        public override int EdgeCount => 6;
    }
}
