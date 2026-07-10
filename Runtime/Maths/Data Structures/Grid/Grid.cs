using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry.Shapes;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    /// <summary>
    /// A generic grid data structure used to store data in each cell and in the edges between the cells
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class Grid<TValue, TEdge>
    {
        readonly Cell<TValue, TEdge>[,] cells;
        public Vector2Int Size { get; }
        
        public IGridTopology Topology { get; }
        public IGridGeometry Geometry { get; }

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
        public Grid(Vector2Int _size, IGridTopology _topology, IGridGeometry _geometry) {
            
            Size = _size;
            cells = new Cell<TValue, TEdge>[_size.x, _size.y];

            Topology = _topology;
            Geometry = _geometry;

            for(int y = 0; y < _size.y; y++)
            {
                for(int x = 0; x < _size.x; x++)
                {
                    cells[x,y] = new(Topology.EdgeCount);
                }
            }
        }

        ~Grid() {}
        #endregion

        #region Edges
        public TEdge GetEdge(Vector2Int _coord, int _edgeIndex) {
            return cells[_coord.x, _coord.y][_edgeIndex];
        }

        public IEnumerable<TEdge> GetCellEdges(Vector2Int _coord) {
            for(int i = 0; i < Topology.EdgeCount; i++) { yield return GetEdge(_coord, i); }
        }

        public void SetEdge(Vector2Int _coord, int _edgeIndex, TEdge _edge) {
            cells[_coord.x, _coord.y][_edgeIndex] = _edge;
        }
        #endregion

        #region Neighbour
        public KeyValuePair<Vector2Int, TValue> GetNeighbour(Vector2Int _coord, int _index) {
            Vector2Int neighbourCoord = _coord + Topology.GetNeighbourCoordinateOffset(_coord, _index);
            if (!IsInBounds(neighbourCoord)) { return new(_coord, default); }
            return new(neighbourCoord, this[_coord]);
        }

        public IEnumerable<KeyValuePair<Vector2Int, TValue>> GetNeighbours(Vector2Int _coord) {
            for(int i = 0; i < Topology.EdgeCount; i++) {
                KeyValuePair<Vector2Int, TValue> neighbour = GetNeighbour(_coord, i);
                if (neighbour.Key == _coord) { continue; }
                yield return neighbour; 
            }
        }
        #endregion
    }
}
