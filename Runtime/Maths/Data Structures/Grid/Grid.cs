using System.Collections.Generic;
using MaroonSeal.Maths.DataStructures.Graphs;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    /// <summary>
    /// A generic grid data structure used to store data in each cell and in the edges between the cells
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    abstract public class Grid<TValue, TEdge>
    {
        readonly GridCell<TValue, TEdge>[,] cells;
        public Vector2Int Size { get; }
        public int CellEdgeCount { get; }

        public TValue this[int _x, int _y]
        {
            get => cells[_x, _y].Value;
            set => cells[_x, _y].Value = value;
        }

        public TValue this[Vector2Int _coord]
        {
            get => this[_coord.x, _coord.y];
            set => this[_coord.x, _coord.y] = value;
        }

        public bool IsInBounds(Vector2Int _coord) => _coord.x >= 0 && _coord.x < Size.x && _coord.y >= 0 && _coord.y < Size.y;

        #region Constructor/Destructor
        public Grid(int _cellEdgeCount, Vector2Int _size) {

            CellEdgeCount = _cellEdgeCount;
            Size = _size;
            
            cells = new GridCell<TValue, TEdge>[_size.x, _size.y];

            for(int y = 0; y < _size.y; y++)
            {
                for(int x = 0; x < _size.x; x++)
                {
                    cells[x,y] = new(_cellEdgeCount);
                }
            }
        }

        ~Grid() {}
        #endregion

        #region Edges
        public TEdge GetEdge(Vector2Int _coord, int _edgeIndex) {
            return cells[_coord.x, _coord.y][_edgeIndex];
        }

        public IEnumerable<TEdge> GetEdges(Vector2Int _coord) {
            for(int i = 0; i < CellEdgeCount; i++) { yield return GetEdge(_coord, i); }
        }

        public void SetEdge(Vector2Int _coord, int _edgeIndex, TEdge _edge) {
            cells[_coord.x, _coord.y][_edgeIndex] = _edge;
        }

        abstract protected Vector2Int GetEdgeCoordinate(Vector2Int _coord, int _index);
        #endregion

        #region Neighbour
        public KeyValuePair<Vector2Int, TValue> GetNeighbour(Vector2Int _coord, int _index) {
            Vector2Int neighbourCoord = _coord + GetEdgeCoordinate(_coord, _index);
            if (!IsInBounds(neighbourCoord)) { return new(_coord, default); }
            return new(neighbourCoord, this[_coord]);
        }

        public IEnumerable<KeyValuePair<Vector2Int, TValue>> GetNeighbours(Vector2Int _coord) {
            for(int i = 0; i < CellEdgeCount; i++) {
                KeyValuePair<Vector2Int, TValue> neighbour = GetNeighbour(_coord, i);
                if (neighbour.Key == _coord) { continue; }
                yield return neighbour; 
            }
        }
        #endregion
    }
}
