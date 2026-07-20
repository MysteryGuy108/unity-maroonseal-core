using UnityEngine;

namespace MaroonSeal.DataStructures.Grid
{
    public class TriangleGridTopology : GridTopology
    {
        public override int EdgeCount => 3;

        readonly static Vector2Int[] upTriNeighbours = new Vector2Int[3] {
            Vector2Int.up, 
            Vector2Int.right, 
            -Vector2Int.right
        };

        readonly static Vector2Int[] downTriNeighbours = new Vector2Int[3] {
            Vector2Int.right, 
            -Vector2Int.up,
            -Vector2Int.right
        };

        static public bool GetIsUpTri(int _x, int _y) => ((_x + _y) & 1) == 0;
        static public bool GetIsUpTri(Vector2Int _coord) => GetIsUpTri(_coord.x, _coord.y);

        #region Neighbours
        public override Vector2Int GetNeighbour(Vector2Int _coord, int _edgeIndex) => 
            GetIsUpTri(_coord) ? upTriNeighbours[_edgeIndex] : downTriNeighbours[_edgeIndex]; 
        #endregion

        #region Rotation
        public override Vector2Int RotateClockwise(Vector2Int _cell)
        {
            Vector2Int lattice = ToLattice(_cell);
            lattice = new(lattice.y, -lattice.x - lattice.y);
            return FromLattice(lattice);
        }

        public override Vector2Int RotateAntiClockwise(Vector2Int _cell)
        {
            Vector2Int lattice = ToLattice(_cell);
            lattice = new(-lattice.x - lattice.y, -lattice.y);
            return FromLattice(lattice);
        }
        #endregion

        #region Lattice
        protected Vector2Int ToLattice(Vector2Int _cell) => new(_cell.x / 2, _cell.y);
        protected Vector2Int FromLattice(Vector2Int _lattice) => new(_lattice.x * 2, _lattice.y);
        #endregion
    }
}
