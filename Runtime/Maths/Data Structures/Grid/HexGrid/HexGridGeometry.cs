using UnityEngine;

using MaroonSeal.Maths.Geometry;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public struct HexGridGeometry : IGridGeometry
    {
        const float sqrt3 = 1.73205080757f; // sqrt(3)
        
        public float CellRadius { get; set; }

        #region Constructors
        public HexGridGeometry(float _cellRadius) { CellRadius = _cellRadius; }
        #endregion

        #region IGridGeometry
        public readonly Vector2Int WorldToCell(Vector2 _worldPoint) { 
            // Axial Coordinates
            float q = 2f / 3f * _worldPoint.x / CellRadius;
            float r = (-1f / 3f * _worldPoint.x + sqrt3 / 3f * _worldPoint.y) / CellRadius;

            // Cube Coordinates
            Vector3 cube = new(q, -q-r, r);

            // Cube Rounding
            Vector3Int cubeR = Vector3Maths.RoundToInt(cube);
            Vector3 cubeD = Vector3Maths.Abs(cubeR - cube);

            // Error Checking (rx + ry + rz = 0)
            if (cubeD.x > cubeD.y && cubeD.x > cubeD.z) { cubeR.x = -cubeR.y - cubeR.z; }
            else if (cubeD.y > cubeD.z) { cubeR.y = -cubeR.x - cubeR.z; }
            else { cubeR.z = -cubeR.x - cubeR.y; }

            return new Vector2Int(
                cubeR.x,
                cubeR.z + (cubeR.x - (cubeR.x & 1)) / 2 
            );
        }

        public Vector2 GetCellCentre(Vector2Int _cell) {
            float y = _cell.y * sqrt3 * CellRadius;

            if ((_cell.x & 1) == 1)
            {
                y += sqrt3 * CellRadius / 2;
            }

            float x = _cell.x * 1.5f * CellRadius;

            return new Vector2(x, y);
        }

        public IShape2D GetCellShape(Vector2Int _cell)
        {
            return new Hexagon2D(new Transform2D(GetCellCentre(_cell)), CellRadius);
        }
        #endregion
    }
}
