using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry;
using MaroonSeal.DataStructures.Graphs;

namespace MaroonSeal.DataStructures.Grids
{
    /// <summary>
    /// A generic grid data structure used to store data in each cell and in the edges between the cells
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    abstract public class Grid<TValue, TEdge, TTopology, TGeometry> : IGrid<TValue, TEdge> 
        where TTopology : GridTopology, new() 
        where TGeometry : IGridGeometry2D, new()
    {
        public Vector2Int Size { get; }

        readonly Dictionary<Vector2Int, Cell> cells;

        private class Cell
        {
            private readonly TEdge[] edges;
            public TValue Value { get; set; }

            public TEdge this[int _edge] {
                get => edges[_edge];
                set => edges[_edge] = value;
            }

            #region Constructors
            public Cell(int _edgeCount) {
                edges = new TEdge[_edgeCount]; 
            }
            #endregion
        }

        public TTopology topology;
        public TGeometry geometry;

        public TValue this[Vector2Int _cell]
        {
            get => cells[_cell].Value;
            set => cells[_cell].Value = value;
        }

        public bool IsInBounds(Vector2Int _coord) => _coord.x >= 0 && _coord.x < Size.x && _coord.y >= 0 && _coord.y < Size.y;

        #region Constructor/Destructor
        public Grid(Vector2Int _size, TTopology _topology, TGeometry _geometry) {
            _size.x = Mathf.Max(0, _size.x); 
            _size.y = Mathf.Max(0, _size.y); 

            Size = _size;

            topology = _topology;
            geometry = _geometry;

            cells = new Dictionary<Vector2Int, Cell>();
            
            for(int y = 0; y < Size.y; y++)
            {
                for(int x = 0; x < Size.x; x++)
                {
                    cells[new(x,y)] = new(topology.EdgeCount);
                }
            }
        }

        public Grid(Vector2Int _size) : this(_size, new(), new()) {}

        public Grid() : this(Vector2Int.zero, new(), new()) {}

        ~Grid() {}
        #endregion

        #region Edges
        public TEdge GetEdge(Vector2Int _cell, int _edgeIndex) => cells[_cell][_edgeIndex];
        public IEnumerable<TEdge> GetEdges(Vector2Int _cell) {
            for(int i = 0; i < topology.EdgeCount; i++) { yield return GetEdge(_cell, i); }
        }

        public void SetEdge(Vector2Int _cell, int _edgeIndex, TEdge _edge) => cells[_cell][_edgeIndex] = _edge;
        #endregion

        #region Neighbour
        public TValue GetNeighbour(Vector2Int _cell, int _index, out Vector2Int _neighbour) {
            _neighbour = _cell + topology.GetNeighbour(_cell, _index);
            if (!IsInBounds(_neighbour)) { return default; }
            return this[_cell];
        }

        public TValue GetNeighbour(Vector2Int _cell, int _index) => GetNeighbour(_cell, _index, out Vector2Int neighbour);

        public IEnumerable<TValue> GetNeighbours(Vector2Int _cell) {
            for(int i = 0; i < topology.EdgeCount; i++) {
                TValue neighbour = GetNeighbour(_cell, i);
                yield return neighbour; 
            }
        }
        #endregion
    }
}
