using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class RectangleGrid<TValue, TEdge> : GridBase<RectangleCell<TValue, TEdge>, TValue, TEdge>
    {
        public RectangleGrid(Vector2Int _size) : base(_size) { }
    }
}
