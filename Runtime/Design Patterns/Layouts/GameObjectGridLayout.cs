using UnityEngine;

namespace MaroonSeal
{
    public class GameObjectGridLayout<TGrid, TValue, TEdge> : GameObjectLayout where TGrid : IGrid<TValue, TEdge>
    {
        protected TGrid grid;
    }
}
