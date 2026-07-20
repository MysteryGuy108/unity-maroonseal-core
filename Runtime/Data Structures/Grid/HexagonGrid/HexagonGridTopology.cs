using UnityEngine;

namespace MaroonSeal.DataStructures.Grid
{
    public class HexagonGridTopology : GridTopology
    {
        readonly static Vector2Int[] evenRowEdges = new Vector2Int[6] {
            new( 1, 0), new( 0, 1), new(-1, 1), 
            new(-1, 0), new(-1,-1), new( 0,-1)
        };

        readonly static Vector2Int[] oddRowEdges = new Vector2Int[6] {
            new( 1, 0), new( 1, 1), new( 0, 1), 
            new(-1, 0), new( 0,-1), new( 1,-1),  
        };

        public override int EdgeCount => 6;

        public override Vector2Int GetNeighbour(Vector2Int _coord, int _edgeIndex)
        {
            Vector2Int[] directions = _coord.y % 2 == 0 ? evenRowEdges : oddRowEdges;
            return _coord + directions[_edgeIndex];
        }

        #region Rotation
        public override Vector2Int RotateClockwise(Vector2Int _cell)
        {
            Vector3Int cube = ToCube(_cell);
            cube = new(-cube.z, -cube.x, cube.y);
            return FromCube(cube);
        }

        public override Vector2Int RotateAntiClockwise(Vector2Int _cell)
        {
            Vector3Int cube = ToCube(_cell);
            cube = new(-cube.y, -cube.z, cube.x);
            return FromCube(cube);
        }
        #endregion

        #region Cube Space
        private Vector3Int ToCube(Vector2Int _cell) {
            int x = _cell.x;
            int z = _cell.y - (_cell.x - (_cell.x & 1)) / 2;
            return new(x, -x-z, z);
        }

        private Vector2Int FromCube(Vector3Int _cube) => new(_cube.x, _cube.z + (_cube.x - (_cube.x & 1)) / 2);
        #endregion
    }
}
