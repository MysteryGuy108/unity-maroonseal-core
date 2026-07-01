using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class SquareGrid<TValue, TEdge> : GridBase<SquareCell<TValue, TEdge>, TValue, TEdge>
    {
        public SquareGrid(Vector2Int _size) : base(_size) { }
    }
}
