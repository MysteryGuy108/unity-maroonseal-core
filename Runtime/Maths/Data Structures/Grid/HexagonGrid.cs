using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class HexagonGrid<TValue, TEdge> : GridBase<HexagonCell<TValue, TEdge>, TValue, TEdge>
    {
        public HexagonGrid(Vector2Int _size) : base(_size) {}
    }
}
