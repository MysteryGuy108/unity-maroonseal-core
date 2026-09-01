using UnityEngine;

namespace MaroonSeal.Layouts
{
    public class GameObjectGridLayout<TGrid, TValue, TEdge> : GameObjectLayout where TGrid : IGrid<TValue, TEdge>
    {
        protected TGrid grid;
    }
}
