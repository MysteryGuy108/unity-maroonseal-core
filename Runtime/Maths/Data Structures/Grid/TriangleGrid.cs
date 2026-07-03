using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public class TriangleGrid<TValue, TEdge> : GridBase<TriangleCell<TValue, TEdge>, TValue, TEdge>
    {
        public TriangleGrid(Vector2Int _size) : base(_size) {}
    }
}
