using MaroonSeal.Maths.DataStructures.Graphs;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    /// <summary>
    /// A generic grid data structure used to store data in each cell and in the edges between the cells
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    abstract public class GridBase<TCell, TValue, TEdge> where TCell : CellBase<TValue, TEdge>
    {
        readonly TCell[,] gridLookup;

        #region Constructor/Destructor
        public GridBase(Vector2Int _size) { gridLookup = new TCell[_size.x, _size.y]; }

        ~GridBase() {
            
        }
        #endregion

        private TCell GetCellContainer(int _x, int _y) => gridLookup[_x, _y];
        
    }
}
